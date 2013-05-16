using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Ischyrus.ProjectFileSynchronizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = args[0];
            FileInfo file = new FileInfo(source);
            if (!file.Exists)
            {
                Console.WriteLine("Unable to locate " + source);
                return;
            }

            string projectName = file.Directory.Name;
            string projectRoot = file.Directory.Parent.FullName;

            Mapper[] targets = 
            {
                new WindowsMapper 
                { 
                    Name="Windows", 
                    ProjectFile = String.Format(@"{0}\{1}.Windows\{1}.Windows.csproj", projectRoot, projectName),
                    ProjectName = projectName
                },
                new Mapper 
                { 
                    Name="Windows Phone", 
                    ProjectFile = String.Format(@"{0}\{1}.WindowsPhone\{1}.WindowsPhone.csproj", projectRoot, projectName),
                    ProjectName = projectName
                },
                new Mapper 
                { 
                    Name="iOS", 
                    ProjectFile = String.Format(@"{0}\{1}.iOS\{1}.iOS.csproj", projectRoot, projectName),
                    ProjectName = projectName
                },
                new AndroidMapper 
                { 
                    Name = "Android",  
                    ProjectFile = String.Format(@"{0}\{1}.Android\{1}.Android.csproj", projectRoot, projectName),
                    ProjectName = projectName
                }
            };
            targets = targets.Where(m => File.Exists(m.ProjectFile)).ToArray();

            List<string> contentFiles = GetContentFiles(source);
            List<string> compileFiles = GetCompileFiles(source);

            Console.WriteLine("{0} content files", contentFiles.Count);
            Console.WriteLine("{0} compile files", compileFiles.Count);

            foreach (var mapper in targets)
            {
                Console.Write("Updating {0} ... ", mapper.Name);
                bool hasChanges = false;

                foreach (string contentFile in contentFiles.Where(s => !mapper.HasResource(s)))
                {
                    hasChanges = true;
                    mapper.AddContentTarget(contentFile);
                    Console.WriteLine();
                    Console.Write("\tAdding {0}", contentFile);
                }

                foreach (string compileFile in compileFiles.Where(s => !mapper.HasCompileTarget(s)))
                {
                    hasChanges = true;
                    mapper.AddCompileTarget(compileFile);
                    Console.WriteLine();
                    Console.Write("\tAdding {0}", compileFile);
                }

                if (hasChanges)
                {
                    mapper.Save();
                    Console.WriteLine();
                }

                Console.WriteLine("done.");
            }
        }

        static List<string> GetCompileFiles(string source)
        {
            return GetFiles("Compile", source);
        }

        static List<string> GetContentFiles(string source)
        {
            return GetFiles("Content", source);
        }

        static List<string> GetFiles(string tag, string source)
        {
            List<string> contentEntries = new List<string>();
            XDocument sourceDoc = XDocument.Parse(File.ReadAllText(source));
            var ns = sourceDoc.Root.GetDefaultNamespace();
            foreach (var content in sourceDoc.Descendants(ns + tag))
            {
                string contentFile = content.Attribute("Include").Value;
                if (Blacklists.Windows8Blacklist.Contains(contentFile))
                {
                    continue;
                }

                contentEntries.Add(contentFile);
            }
            return contentEntries;
        }
    }


}

using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Ischyrus.ProjectFileSynchronizer
{
    public class Mapper
    {
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string ProjectFile { get; set; }
        protected XDocument doc;

        public virtual bool HasCompileTarget(string path)
        {
            this.doc = this.doc ?? this.Load();
            return this.doc.Descendants(this.doc.Root.GetDefaultNamespace() + "Link").Any(e => e.Value == path);
        }

        public virtual bool HasResource(string path)
        {
            return this.HasCompileTarget(path);
        }

        public virtual void AddCompileTarget(string path)
        {
            this.AddElement("Compile", path, null);
        }

        public virtual void AddContentTarget(string path)
        {
            this.AddElement("Content", path, null);
        }

        protected void AddElement(string node, string path, XElement childElement)
        {
            this.AddElement(node, path, path, childElement);
        }

        protected void AddElement(string node, string relativePath, string path, XElement childElement)
        {
            this.doc = this.doc ?? this.Load();

            XElement destination = null;
            foreach (var itemGroup in this.doc.Descendants(this.doc.Root.GetDefaultNamespace() + "ItemGroup"))
            {
                destination = itemGroup;

                foreach (var compile in itemGroup.Elements(this.doc.Root.GetDefaultNamespace() + node))
                {
                    // I wanted to put all of the linked items into the same item group.
                    // It does this by picking the first ItemGroup that has a Link'd attribute.
                    // If there isn't one, then the last ItemGroup encountered is used.
                    var link = compile.Element(this.doc.Root.GetDefaultNamespace() + "Link");
                    if (link != null)
                    {
                        break;
                    }
                }
            }

            XElement newNode = new XElement(this.doc.Root.GetDefaultNamespace() + node);
            newNode.Add(new XAttribute("Include", @"..\" + this.ProjectName + @"\" + relativePath));
            newNode.Add(new XElement(this.doc.Root.GetDefaultNamespace() + "Link", path));
            if (childElement != null)
            {
                newNode.Add(childElement);
            }
            destination.Add(newNode);
        }

        private XDocument Load()
        {
            return XDocument.Parse(File.ReadAllText(this.ProjectFile));
        }

        internal void Save()
        {
            //    StringWriter s = new StringWriter();
            //    this.doc.Save(s);
            //    string preview = s.ToString();

            this.doc.Save(File.OpenWrite(this.ProjectFile));
        }
    }
}

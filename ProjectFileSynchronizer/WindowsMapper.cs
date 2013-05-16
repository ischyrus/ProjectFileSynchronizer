using System.Xml.Linq;

namespace Ischyrus.ProjectFileSynchronizer
{
    public class WindowsMapper : Mapper
    {
        public override void AddContentTarget(string path)
        {
            this.AddElement("Content", path, new XElement(this.doc.Root.GetDefaultNamespace() + "CopyToOutputDirectory", "PreserveNewest"));
        }
    }
}

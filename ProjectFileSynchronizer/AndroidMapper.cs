namespace Ischyrus.ProjectFileSynchronizer
{
    public class AndroidMapper : Mapper
    {
        public override bool HasResource(string path)
        {
            // Android puts it's resources in a nested folder called Assets/
            return base.HasResource(@"Assets\" + path);
        }

        public override void AddContentTarget(string path)
        {
            // Android puts it's resources in a nested folder called Assets.
            this.AddElement("AndroidAsset", path, @"Assets\" + path, null);
        }
    }
}

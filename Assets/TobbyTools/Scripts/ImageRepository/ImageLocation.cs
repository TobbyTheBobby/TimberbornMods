using System.IO;

namespace TobbyTools.ImageRepository
{
    public class ImageLocation
    {
        public string Name { get; private set; }
        
        public string ImagePath { get; private set; }
        
        public string ParentFolder =>  ImagePath.Split(Path.DirectorySeparatorChar)[ImagePath.Split(Path.DirectorySeparatorChar).Length-2];

        public ImageLocation(string name, string path)
        {
            Name = name;
            ImagePath = path;
        }
    }
}
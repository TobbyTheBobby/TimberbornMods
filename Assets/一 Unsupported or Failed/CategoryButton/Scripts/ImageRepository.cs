using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimberApi.Common.SingletonSystem;
using TimberApi.ModSystem;

namespace CategoryButton
{
    public class ImageRepository :IEarlyLoadableSingleton
    {
        private readonly IModRepository _modRepository;

        public readonly Dictionary<string, string> Images = new();

        private ImageRepository(IModRepository modRepository)
        {
            _modRepository = modRepository;
        }

        public void EarlyLoad()
        {
            foreach (var mod in _modRepository.All())
            {
                IndexImagesInMod(mod);
            }
        }

        private void IndexImagesInMod(IMod mod)
        {
            ScanDirectory(mod.DirectoryPath);
        }

        private void ScanDirectory(string directoryPath)
        {
            foreach (var directory in Directory.GetDirectories(directoryPath))
            {
                ScanDirectory(directory);
            }

            if (!directoryPath.Contains("assets")) return;
            
            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                if (ShouldIndexFile(filePath))
                {
                    IndexFile(filePath);
                }
            }
        }

        private bool ShouldIndexFile(string filePath)
        {
            var flag1 = filePath.Contains(".png");
            var flag2 = filePath.Contains(".jpg");

            var flag3 = filePath.Contains(".meta");

            return (flag1 || flag2) && !flag3;
        }

        private void IndexFile(string filePath)
        {
            var splitPath = filePath.Split(Path.DirectorySeparatorChar);
            Images.Add(splitPath.Last(), filePath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TimberApi.Common.SingletonSystem;
using TimberApi.ModSystem;
using Timberborn.Common;
using UnityEngine;

namespace TobbyTools.ImageRepository
{
    public class ImageRepository : IEarlyLoadableSingleton
    {
        private readonly IModRepository _modRepository;

        public readonly List<ImageLocation> Images = new();
        
        public readonly Dictionary<ImageLocation, Texture2D> ImageCache = new();

        ImageRepository(IModRepository modRepository)
        {
            _modRepository = modRepository;
        }

        public Texture2D GetOrAdd(ImageLocation imageLocation, Func<Texture2D> supplier)
        {
            return ImageCache.GetOrAdd(imageLocation, supplier);
        }
        
        public void EarlyLoad()
        {
            var stopwatch = Stopwatch.StartNew();
            foreach (var mod in _modRepository.All())
            {
                IndexImagesInMod(mod);
            }
            stopwatch.Stop();
            Plugin.Log.LogInfo($"Finished indexing images in {stopwatch.ElapsedMilliseconds}ms.");

            // foreach (var pair in ImageCache)
            // {
            //     Plugin.Log.LogWarning(pair.Value);
            // }
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
            
            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                if (ShouldFileBeIndexed(filePath))
                {
                    IndexFile(filePath);
                }
            }
        }

        private bool ShouldFileBeIndexed(string filePath)
        {
            bool containsPng = filePath.Contains(".png");
            bool containsJpg = filePath.Contains(".jpg");

            bool containsMeta = filePath.Contains(".meta");

            return (containsPng || containsJpg) && !containsMeta;
        }

        private void IndexFile(string filePath)
        {
            var splitPath = filePath.Split(Path.DirectorySeparatorChar);
            
            Images.Add(new ImageLocation(splitPath.Last(), filePath));
        }
    }
}

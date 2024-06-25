using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TobbyTools.ImageRepository
{
    public class ImageRepositoryService
    {
        private readonly ImageRepository _imageRepository;
        
        ImageRepositoryService(ImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        
        public Texture2D GetByName(string search)
        {
            var imageLocations = _imageRepository.Images.Where(image => image.Name == search || image.ParentFolder == search).ToArray();

            var imageLocation = ValidateResults(imageLocations, search);
            
            return _imageRepository.GetOrAdd(imageLocation, () => LoadImage(imageLocation));
        }
        
        public Texture2D GetByName(string search, string parentFolder)
        {
            var imageLocations = _imageRepository.Images.Where(image => image.Name == search && image.ParentFolder == parentFolder).ToArray();

            var imageLocation = ValidateResults(imageLocations, search);
            
            return _imageRepository.GetOrAdd(imageLocation, () => LoadImage(imageLocation));
        }

        private ImageLocation ValidateResults(ImageLocation[] imageLocations, string search)
        {
            if (!imageLocations.Any())
                throw new Exception($"File '{search}' does not exist.");

            if (imageLocations.Length > 1)
                Plugin.Log.LogWarning($"Multiple files which were found based on '{search}'. First item was used.");

            return imageLocations.First();
        }

        private Texture2D LoadImage(ImageLocation imageLocation)
        {
            var spriteBytes = new byte[] { };
            if (File.Exists(imageLocation.ImagePath))
                spriteBytes = File.ReadAllBytes(imageLocation.ImagePath);
            var texture2D = new Texture2D(0, 0);
            texture2D.LoadImage(spriteBytes);
            return texture2D;
        }
    }
}
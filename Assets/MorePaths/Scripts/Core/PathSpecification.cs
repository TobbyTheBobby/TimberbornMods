using UnityEngine;

namespace MorePaths
{
    public class PathSpecification 
    {
        public PathSpecification(
            bool enabled,
            string name,
            string pathTexture,
            string railingTexture,
            string toolGroup,
            int toolOrder,
            string pathIcon,
            string displayNameLocKey,
            string descriptionLocKey,
            string flavorDescriptionLocKey,
            
            float mainTextureScale,
            float noiseTexScale,
            float mainColorRed,
            float mainColorGreen,
            float mainColorBlue,
            
            bool railingEnabled)
        {
            Enabled = enabled;
            Name = name;
            PathTexture = pathTexture;
            RailingTexture = railingTexture;
            ToolGroup = toolGroup;
            ToolOrder = toolOrder;
            PathIcon = pathIcon;
            DisplayNameLocKey = displayNameLocKey;
            DescriptionLocKey = descriptionLocKey;
            FlavorDescriptionLocKey = flavorDescriptionLocKey;

            MainTextureScale = mainTextureScale;
            NoiseTexScale = noiseTexScale;
            MainColorRed = mainColorRed;
            MainColorGreen = mainColorGreen;
            MainColorBlue = mainColorBlue;

            RailingEnabled = railingEnabled;
        }

        public bool Enabled { get;  }
        public string Name { get; }
        public string PathTexture { get; }
        public string RailingTexture { get; }
        public string ToolGroup { get; }
        public int ToolOrder { get; }
        public string PathIcon { get; }
        public string DisplayNameLocKey { get; }
        public string DescriptionLocKey { get;  }
        public string FlavorDescriptionLocKey { get; }
        
        public float MainTextureScale { get; }
        public float NoiseTexScale { get; }
        public float MainColorRed { get; }
        public float MainColorGreen { get; }
        public float MainColorBlue { get; }
        public bool RailingEnabled { get; }

        private Texture2D _groundTexture2D;
        public Texture2D GroundTexture2D(MorePathsCore morePathsCore)
        {
            if (_groundTexture2D == null)
            {
                _groundTexture2D = morePathsCore.TryLoadTexture(Name, PathTexture);
            }

            return _groundTexture2D;
        }
        
        private Texture2D _railingTexture2D;
        public Texture2D RailingTexture2D(MorePathsCore morePathsCore)
        {
            if (_railingTexture2D == null)
            {
                _railingTexture2D = morePathsCore.TryLoadTexture(Name, RailingTexture);
            }

            return _railingTexture2D;
        }
        
        private Texture2D _spriteTexture2D;
        public Texture2D SpriteTexture2D(MorePathsCore morePathsCore, int width = 1024, int height = 1024)
        {
            if (_spriteTexture2D == null)
            {
                _spriteTexture2D = morePathsCore.TryLoadTexture(Name, PathIcon, width, height);
            }

            return _spriteTexture2D;
        }
    }
}

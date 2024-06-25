using Steamworks;
using UnityEngine;

namespace TimberbornThunderkitExtension
{
    public class SteamPackage
    {
        private SteamUGCDetails_t _details;

        public string PreviewUrl { get; private set; }
        public Texture2D PreviewIcon { get; private set; } = Texture2D.blackTexture;
        
        public SteamPackage(SteamUGCDetails_t details, string previewUrl)
        {
            _details = details;
            PreviewUrl = previewUrl;    
        }

        public string Title => _details.m_rgchTitle;
        public string Description => _details.m_rgchDescription;
        public string FileName => _details.m_pchFileName;
        public string FileId => _details.m_nPublishedFileId.ToString();
        public string CreatorId => _details.m_nCreatorAppID.ToString();
        public string Url => _details.m_rgchURL;
        public string Tags => _details.m_rgchTags;

        public void SetImage(Texture2D image)
        {
            PreviewIcon = image;
        }
    }
}
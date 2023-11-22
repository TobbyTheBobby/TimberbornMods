using ThunderKit.Core.Manifests;
using UnityEngine;

namespace ThunderkitModioUpload
{ 
    public class ModIoData : ManifestDatum
    {
        public uint ModId; 
        public string Version;
        public bool SetLive;
        [TextArea(10, 10)]
        public string Description = "";
    }
}

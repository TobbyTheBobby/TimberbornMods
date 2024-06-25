using ThunderKit.Core.Manifests;
using UnityEngine;

namespace TimberbornThunderkitExtension
{ 
    public class ModIoData : ManifestDatum
    {
        public uint ModId; 
        public bool SetLive;
        [TextArea(10, 10)]
        public string Description = "";
    }
}

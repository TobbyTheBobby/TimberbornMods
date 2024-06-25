using System;
using ThunderKit.Common;
using ThunderKit.Core.Manifests;
using ThunderKit.Core.Utilities;
using UnityEditor;

namespace TimberbornThunderkitExtension
{
    public class TimberbornManifest : Manifest
    {
        [MenuItem(Constants.ThunderKitContextRoot + nameof(TimberbornManifest), priority = Constants.ThunderKitMenuPriority)]
        public new static void Create()
        {
            ScriptableHelper.SelectNewAsset(afterCreated: (Action<TimberbornManifest>)(manifest =>
            {
                manifest.Identity = CreateInstance<TimberbornManifestIdentity>();
                manifest.Identity.name = nameof(TimberbornManifestIdentity);
                manifest.InsertElement(manifest.Identity, 0);
            }));
        }
    }
}
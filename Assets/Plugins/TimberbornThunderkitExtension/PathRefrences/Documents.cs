using System;
using System.IO;
using ThunderKit.Core.Paths;
using ThunderKit.Core.Pipelines;
using UnityEngine;

namespace TimberbornThunderkitExtension.PathRefrences
{
    public class Documents : PathComponent
    {
        public string BuildDirectory = "ModsBuild";
        public string BuildName = "TimberbornMods";
        private static readonly string UserDataFolder =
            Application.platform == RuntimePlatform.OSXEditor
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Documents",
                    "Timberborn")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Timberborn");
        
        protected override string GetPathInternal(PathReference output, Pipeline pipeline)
        {
            return Path.Combine(UserDataFolder, BuildDirectory, BuildName);
        }
    }
}
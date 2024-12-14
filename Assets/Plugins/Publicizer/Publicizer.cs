using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Plugins.Publicizer
{
    public class Publicizer : EditorWindow
    {
        private static readonly string TimberbornDir = $@"{Application.dataPath}\Plugins\Timberborn"; 
        
        [MenuItem("Timberborn/Publicizing/Publicize Timberborn dlls", false, 0)]
        public static void Publicize()
        {
            InstallAssemblyPublicizer();
            
            Debug.Log("Publishing all Timberborn DLL files.");

            var timberbornDlls = GetDllPaths();

            // Create a backup of the original DLLs
            foreach (var dll in timberbornDlls)
            {
                var backupPath = dll + ".bak";
                File.Copy(dll, backupPath, true);
            }

            // Publicize the DLLs (assuming assembly-publicizer is a valid tool)
            foreach (var dll in timberbornDlls)
            {
                var publicizeCommand = $"assembly-publicizer --overwrite --publicize-compiler-generated \"{dll}\"";
                ExecuteCommand(publicizeCommand);
            }
        }
        
        [MenuItem("Timberborn/Publicizing/Revert Timberborn dlls", false, 0)]
        public static void Revert()
        {
            InstallAssemblyPublicizer();
            
            Debug.Log("Reverting all Timberborn DLL files.");
            
            var timberbornDlls = GetDllPaths();
            
            foreach (var dll in timberbornDlls)
            {
                var backupPath = dll + ".bak";
                File.Copy(backupPath, dll, true);
                File.Delete(backupPath);
            }
        }

        private static void InstallAssemblyPublicizer()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "CMD.exe",
                Arguments = "/C dotnet tool install -g BepInEx.AssemblyPublicizer.Cli",
                UseShellExecute = true,
                WorkingDirectory = Application.dataPath
            };
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            
        }

        private static List<string> GetDllPaths()
        {
            // Add DLLs to exclude here
            string[] excludedDlls =
            {
                // "Timberborn.BaseComponentSystem.dll"
            }; 

            // Get all Timberborn DLLs, excluding the specified ones
            return Directory.GetFiles(TimberbornDir, "Timberborn.*.dll", SearchOption.AllDirectories)
                .Where(dll => !excludedDlls.Contains(Path.GetFileName(dll)))
                .ToList();
        }

        private static void ExecuteCommand(string command)
        {
            // Run the command using a process
            var startInfo = new ProcessStartInfo
            {
                FileName = "CMD.exe",
                Arguments = "/C " + command,
                UseShellExecute = false,
            };
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
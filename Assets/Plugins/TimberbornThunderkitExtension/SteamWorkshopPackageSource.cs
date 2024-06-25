using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Modio;
using Modio.Models;
using SharpCompress.Archives;
using SharpCompress.Readers;
using Steamworks;
using ThunderKit.Core.Data;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using File = System.IO.File;
using Image = UnityEngine.UI.Image;

namespace TimberbornThunderkitExtension
{
    using PV = PackageVersion;

    public class SteamWorkshopPackageSource : PackageSource
    {
        private Dictionary<string, HashSet<string>> _dependencyMap;
        private Dictionary<string, PackageGroup> _groupMap;
        private List<SteamPackage> _mods;

        public uint GameID;
        public override string Name => "Steam Workshop";
        public override string SourceGroup => "Steam Workshop";

        private const string SettingsPath = "Assets/ThunderKitSettings";

        [InitializeOnLoadMethod]
        private static void CreateThunderKitExtensionSource()
        {
            EditorApplication.update += EnsureThunderKitExtensions;
        }

        private static void EnsureThunderKitExtensions()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                return;
            }

            EditorApplication.update -= EnsureThunderKitExtensions;

            var basePath = $"{SettingsPath}/ThunderKit Extensions.asset";
            var source = AssetDatabase.LoadAssetAtPath<ModIoPackageSource>(basePath);
            if (!source)
            {
                if (File.Exists(basePath))
                    File.Delete(basePath);
            }
        }

        protected override string VersionIdToGroupId(string dependencyId)
        {
            return dependencyId.Substring(0, dependencyId.LastIndexOf("-"));
        }

        protected override void OnLoadPackages()
        {
            foreach (var steamPackage in _mods)
            {
                var packageVersionInfo = new PackageVersionInfo(
                    steamPackage.FileName, 
                    $"{steamPackage.FileId}", 
                    new string[] { }, 
                    ConstructMarkdown(steamPackage));
                
                AddPackageGroup(new PackageGroupInfo
                {
                    Author = steamPackage.CreatorId,
                    Name = steamPackage.Title,
                    Description = steamPackage.Description,
                    DependencyId = $"{steamPackage.FileId}",
                    HeaderMarkdown = $"![]({steamPackage.PreviewIcon}){{ .icon }} {steamPackage.Title}{{ .icon-title .header-1 }}\r\n\r\n",
                    FooterMarkdown = $"",
                    // Versions = new List<PackageVersionInfo> { new(mod.Modfile?.Version, $"{mod.Id}", _modDependencies[mod.Id], ConstructMarkdown(mod)) },
                    Versions = new List<PackageVersionInfo> { packageVersionInfo },
                    Tags = new[]{steamPackage.Tags}
                });
            }

            SourceUpdated();
        }

        

        private static string ConstructMarkdown(SteamPackage steamUgcDetailsT)
        {
            var markdown = $"### Description\r\n\r\n{steamUgcDetailsT.Description}\r\n\r\n";
            if (!string.IsNullOrWhiteSpace(steamUgcDetailsT.Url))
                markdown += $"{{ .links }}";
            if (!string.IsNullOrWhiteSpace(steamUgcDetailsT.Url))
                markdown += $"[Mod.io]({steamUgcDetailsT.Url})";
            return markdown;
        }

        protected override async void OnInstallPackageFiles(PV version, string packageDirectory)
        {
            var steamUgcDetailsT = LookupPackage(version.version);
            // var filePath = Path.Combine(packageDirectory, $"{mod.Name}.zip");
            //
            // var settings = ThunderKitSetting.GetOrCreateSettings<ModIoConfiguration>();
            // var client = new Client(new Credentials(settings.ApiKey, settings.AuthToken));
            // await client.Download(mod.GameId, mod.Id, mod.Modfile.Id, new FileInfo(filePath));
            //
            // using (var archive = ArchiveFactory.Open(filePath))
            // {
            //     foreach (var entry in archive.Entries.Where(entry => entry.IsDirectory))
            //     {
            //         var path = Path.Combine(packageDirectory, entry.Key);
            //         Directory.CreateDirectory(path);
            //     }
            //
            //     var extractOptions = new ExtractionOptions { ExtractFullPath = true, Overwrite = true };
            //     foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
            //         entry.WriteToDirectory(packageDirectory, extractOptions);
            // }
            //
            // File.Delete(filePath);
        }

        private static CallResult<SteamUGCQueryCompleted_t> onUGCQueryCompleted;

        protected override Task ReloadPagesAsyncInternal()
        {
            if (!SteamManagerEditor.Initialize())
            {
                Debug.LogError("Failed to initialize Steamworks.");
                return Task.CompletedTask;
            }

            var queryHandle = SteamUGC.CreateQueryAllUGCRequest(
                EUGCQuery.k_EUGCQuery_RankedByVote,
                EUGCMatchingUGCType.k_EUGCMatchingUGCType_All,
                SteamAppId.AppId,
                SteamAppId.AppId,
                1);
            
            onUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(OnUGCQueryCompleted);

            var hAPICall = SteamUGC.SendQueryUGCRequest(queryHandle);
            SteamUGC.SetReturnTotalOnly(queryHandle, false);
            SteamUGC.SetAllowCachedResponse(queryHandle, 10);

            onUGCQueryCompleted.Set(hAPICall);
            
            return Task.CompletedTask;
        }
        
        private void OnUGCQueryCompleted(SteamUGCQueryCompleted_t param, bool biofailure)
        {
            ProcessQueryResults(param);
            LoadPackages();
        }

        private void ProcessQueryResults(SteamUGCQueryCompleted_t pCallback)
        {
            if (pCallback.m_eResult != EResult.k_EResultOK)
            {
                Debug.LogError("Failed to get workshop items.");
                return;
            }

            _mods = new List<SteamPackage>();

            Debug.Log("Number of matching items: " + pCallback.m_unNumResultsReturned);

            for (uint i = 0; i < pCallback.m_unNumResultsReturned; i++)
            {
                SteamUGCDetails_t details;
                var detailsSuccess = SteamUGC.GetQueryUGCResult(pCallback.m_handle, i, out details);
                uint test = 0;
                var previewUrlSuccess = SteamUGC.GetQueryUGCPreviewURL(pCallback.m_handle, i, out var previewUrl, test);
                var steamPackage = new SteamPackage(details, previewUrl);

                Task.Run(() => PopulateImage(steamPackage));
                
                if (detailsSuccess && previewUrlSuccess)
                {
                    _mods.Add(steamPackage);
                }
                else
                {
                    Debug.LogError("Failed to get details for item " + i);
                }
            }

            SteamUGC.ReleaseQueryUGCRequest(pCallback.m_handle);
        }
        
        private async Task PopulateImage(SteamPackage steamPackage)
        {
            var request = UnityWebRequestTexture.GetTexture(steamPackage.PreviewUrl);
            await request.SendWebRequest();
            // steamPackage.SetImage(((DownloadHandlerTexture)request.downloadHandler).texture);
            var packageGroup = Packages.First(packageGroup => packageGroup.DependencyId == steamPackage.FileId);

            packageGroup.HeaderMarkdown = $"![]({((DownloadHandlerTexture)request.downloadHandler).texture}){{ .icon }} {steamPackage.Title}{{ .icon-title .header-1 }}\r\n\r\n";
        } 

        private SteamPackage LookupPackage(string id)
        {
            return _mods.First(steamPackage =>  steamPackage.FileId == id);
        }
    }
}
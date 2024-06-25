using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Modio;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Writers;
using ThunderKit.Core.Attributes;
using ThunderKit.Core.Data;
using ThunderKit.Core.Paths;
using ThunderKit.Core.Pipelines;
using UnityEditor;

namespace TimberbornThunderkitExtension
{
    [PipelineSupport(typeof(Pipeline)), ManifestProcessor]
    public class UploadToModIo : PipelineJob
    {
        [PathReferenceResolver]
        public string Source;

        public override Task Execute(Pipeline pipeline)
        {
            try
            {
                var settings = ThunderKitSetting.GetOrCreateSettings<ModIoConfiguration>();

                if (!settings.ConfiguredCorrectly())
                {
                    pipeline.Log(LogLevel.Error, "Mod.io Settings have not been configured correctly.");
                    return Task.CompletedTask;
                }

                var timberbornManifestIdentity = pipeline.GetSingle<TimberbornManifestIdentity>();

                var source = Source.Resolve(pipeline, this);
                var output = Path.Combine(Path.GetDirectoryName(source), timberbornManifestIdentity.Name + ".zip");

                var modIoData = pipeline.GetSingle<ModIoData>();

                var client = new Client(new Credentials(settings.ApiKey, settings.AuthToken));
                var modClient = client.Games[(uint)settings.GameId].Mods[modIoData.ModId];

                var filesClient = modClient.Files;

                IReadOnlyList<Modio.Models.File> modFiles = null;

                var task = Task.Run(async () => { modFiles = await filesClient.Search().ToList(); });
                task.Wait();

                if (modFiles.Any())
                {
                    var mostRecentModFile = modFiles.Last();
                    
                    if (timberbornManifestIdentity.Version == mostRecentModFile.Version)
                    {
                        if (!EditorUtility.DisplayDialog("Same version number", "The most recent version of the mod has the same version number. You sure you want to continue?", "Yes", "No"))
                        {
                            pipeline.Log(LogLevel.Information, "Canceled Uploading.");
                            return Task.CompletedTask;
                        }
                    }
                }

                if (File.Exists(output))
                {
                    File.Delete(output);
                }

                var archive = ArchiveFactory.Create(ArchiveType.Zip);
                archive.AddAllFromDirectory(source, searchOption: SearchOption.AllDirectories);
                var options = new WriterOptions(CompressionType.Deflate);
                archive.SaveTo(output, options);
                
                var newFile = new NewFile(new FileInfo(output))
                {
                    Version = timberbornManifestIdentity.Version,
                    Changelog = modIoData.Description,
                    Active = modIoData.SetLive
                };
                
                task = Task.Run(async () => { await filesClient.Add(newFile); });
                task.Wait();

                if (task.IsCompletedSuccessfully)
                {
                    pipeline.Log(LogLevel.Information, "Success");
                }
                else
                {
                    pipeline.Log(LogLevel.Error, "Something went wrong: " + task.Exception);
                }
            }
            catch (Exception e)
            {
                pipeline.Log(LogLevel.Error, e + "");
                throw;
            }

            return Task.CompletedTask;
        }
    }
}

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThunderKit.Core.Attributes;
using ThunderKit.Core.Paths;
using ThunderKit.Core.Pipelines;
using UnityEditor;

namespace TimberbornThunderkitExtension
{
    [PipelineSupport(typeof(Pipeline))]
    public class ProjectBuilder : PipelineJob
    {
        [PathReferenceResolver]
        public string Destination;
        
        public override Task Execute(Pipeline pipeline)
        {
            var buildPath = Destination.Resolve(pipeline, this);
            if (!Directory.Exists(buildPath)) 
                Directory.CreateDirectory(buildPath);
            
            var buildOptions = new BuildPlayerOptions
            {
                target = BuildTarget.StandaloneWindows64,
                locationPathName = buildPath,
                scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray()
            };
            BuildPipeline.BuildPlayer(buildOptions);
            return Task.CompletedTask;
        }
    }
}
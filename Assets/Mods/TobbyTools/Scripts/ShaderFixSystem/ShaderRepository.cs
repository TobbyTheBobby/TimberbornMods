using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timberborn.SingletonSystem;
using Timberborn.Timbermesh;
using Timberborn.TimbermeshMaterials;
using UnityEngine;

namespace TobbyTools.ShaderFixSystem
{
    public class ShaderRepository : ILoadableSingleton
    {
        private readonly IMaterialRepository _materialRepository;

        public Dictionary<string, Shader> Shaders { get; } = new();

        public ShaderRepository(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public void Load()
        {
            switch (_materialRepository)
            {
                case MaterialRepository materialRepository:
                    ProcessMaterials(materialRepository._materials.Values.ToList());
                    break;
            }

            // foreach (var shader in Shaders.Values)
            // {
            //     var stringBuilder = new StringBuilder();
            //     stringBuilder.AppendLine($"Shader loaded: {shader.name}");
            //     stringBuilder.AppendLine($"     Shaderqueue: {shader.renderQueue}");
            //     for (var i = 0; i < shader.GetPropertyCount(); i++)
            //     {
            //         stringBuilder.AppendLine(shader.GetPropertyName(i));
            //     }
            //
            //     Debug.Log(stringBuilder);
            // }
        }

        private void ProcessMaterials(List<Material> materials)
        {
            foreach (var material in materials)
            {
                var shader = material.shader;
                Shaders.TryAdd(shader.name, shader);
            }
        }
    }
}
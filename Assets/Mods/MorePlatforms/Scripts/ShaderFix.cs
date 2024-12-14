using HarmonyLib;
using JetBrains.Annotations;
using TimberApi.DependencyContainerSystem;
using Timberborn.AssetSystem;
using Timberborn.GameFactionSystem;
using Timberborn.Timbermesh;
using Timberborn.TimbermeshMaterials;
using UnityEngine;

namespace MorePlatforms
{
    [HarmonyPatch(typeof(MaterialRepository), "Load")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class ShaderFix
    {
        private static void Postfix()
        {
            var materialName = $"BaseMetal.{DependencyContainer.GetInstance<FactionService>().Current.Id}";
            
            var material = DependencyContainer.GetInstance<IMaterialRepository>().GetMaterial(materialName);

            foreach (var loadedAsset in DependencyContainer.GetInstance<IAssetLoader>().LoadAll<Material>("CustomMaterials"))
            {
                loadedAsset.Asset.shader = material.shader;
            }
        }
    }
}
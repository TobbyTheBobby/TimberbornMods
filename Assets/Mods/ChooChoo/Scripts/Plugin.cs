using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using ChooChoo.Trains;
using ChooChoo.Wagons;
using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.BaseComponentSystem;
using Timberborn.Characters;
using Timberborn.GameDistricts;
using Timberborn.ModManagerScene;
using Timberborn.PrefabSystem;
using UnityEngine;

#pragma warning disable CS0618
[assembly: SecurityPermission(action: SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace ChooChoo
{
    public class Plugin : IModStarter
    {
        private const string PluginGuid = "Tobbert.ChooChoo";

        public void StartMod()
        {
            new Harmony(PluginGuid).PatchAll();
        }
    }

    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class MigrationTriggerPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("MigrationTrigger"), "RegisterDistributorToCheck", new[]
            {
                typeof(Citizen)
            });
        }

        private static bool Prefix(Citizen citizen)
        {
            if (citizen.TryGetComponentFast(out Train _))
            {
                return false;
            }

            if (citizen.TryGetComponentFast(out TrainWagon _))
            {
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class InstantiatorPatch
    {
        private static readonly List<string> PreventDecorators = new()
        {
            nameof(Citizen),
            nameof(CharacterTint),
            "StrandedStatus",
            "ControllableCharacter",
            "Hauler",
            "NeedManager",
            "WellbeingTierManager",
            "GoodCarrierAnimator",
            "NavMeshObserver",
            "Enterer",
            "RangedEffectReceiver"
        };

        private static readonly List<string> GameObjectsToCheck = new()
        {
            "Train",
            "Wagon"
        };

        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("Instantiator"), "AddComponent", new[]
            {
                typeof(GameObject), typeof(Type)
            });
        }

        private static bool Prefix(GameObject gameObject, MemberInfo componentType, ref Component __result)
        {
            if (gameObject == null)
                return true;
            var prefab = gameObject.GetComponent<Prefab>();
            if (prefab == null)
                return true;

            var prefabName = prefab.PrefabName;
            foreach (var nameToCheck in GameObjectsToCheck)
            {
                if (prefabName.Contains(nameToCheck))
                {
                    // Plugin.Log.LogWarning(gameObject + "      " + componentType.Name);
                    if (PreventDecorators.Contains(componentType.Name))
                    {
                        // Plugin.Log.LogError("Preventing");
                        __result = new Component();
                        return false;
                    }
                }
            }

            return true;
        }
    }

    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class GoodCarrierFragmentPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GoodCarrierFragment"), "ShowFragment", new[] { typeof(BaseComponent) });
        }

        private static bool Prefix(BaseComponent entity)
        {
            return !entity.TryGetComponentFast(out WagonManager _);
        }
    }

    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CharacterBatchControlTabPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("CharacterBatchControlTab"), "GetSortingKey", new[] { typeof(string) });
        }

        private static bool Prefix(string locKey, ref string __result)
        {
            if (locKey == "Tobbert.Train.PrefabName" || locKey == "Tobbert.Wagon.PrefabName")
            {
                __result = "4";
                return false;
            }

            return true;
        }
    }
    
    // [HarmonyPatch]
    // [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    // public class TestPatch
    // {
    //     public static MethodInfo TargetMethod()
    //     {
    //         return AccessTools.Method(AccessTools.TypeByName("StatusIconCyclerFactory"), "CreateAsChild", new[] { typeof(Transform) });
    //     }
    //
    //     private static void Prefix(Transform parent)
    //     {
    //         Debug.LogWarning(parent == null);
    //         Debug.LogWarning(parent);
    //         Debug.LogWarning(parent.parent ? parent.parent : null);
    //         Debug.LogWarning(parent.parent.parent ? parent.parent.parent : null);
    //     }
    // }
}
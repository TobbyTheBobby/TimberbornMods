using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.Beavers;
using Timberborn.RangedEffectSystem;
using Timberborn.WalkingSystem;

namespace Ladder
{
    [HarmonyPatch]
    public static class ApplyEffectsListener
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.TypeByName("Timberborn.RangedEffectSystem.RangedEffectReceiver").GetMethod("ApplyEffects", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        // private static void Postfix(RangedEffectReceiver instance)
        // private static void Postfix(RangedEffectReceiver __instance)
        // {
        //     Plugin.Log.LogFatal(__instance.GetComponent<WalkerSpeedManager>().Speed);
        //     // __instance.GetComponent<WalkerSpeedManager>().Speed = 0.1f;
        //     
        //     // IReadOnlyList<RangedEffect> affectingEffects = __instance.GetAffectingEffects();
        //     // foreach (RangedEffect rangedEffect in (IEnumerable<RangedEffect>) affectingEffects)
        //     // {
        //     //     // NeedManager needManager = this._needManager;
        //     //     // ContinuousEffect continuousEffect = rangedEffect.ToContinuousEffect();
        //     //     // ref ContinuousEffect local = ref continuousEffect;
        //     //     // double deltaTimeInHours2 = (double) deltaTimeInHours1;
        //     //     // needManager.ApplyEffect(in local, (float) deltaTimeInHours2);
        //     //     
        //     //     Plugin.Log.LogFatal(__instance.GetComponent<WalkerSpeedManager>().Speed);
        //     //     Plugin.Log.LogFatal("asd");
        //     // }
        //
        //     // CODE VOOR HET TOEVOEGEN AAN EEN BEAVER
        //     // var myAssetBundle = AssetBundle.LoadFromFile("Path/To/AssetBundle.bundle");
        //     // var myGameObjectPrefab = myAssetBundle.LoadAsset<GameObject>("yourGameObjectPrefab")
        //     // var targetPrefab = GetTargetGameObjectMethod();
        //     // var myGameObjectInstance = GameObject.Instantiate(myGameObjectPrefab);
        //     // myGameObjectInstance.transform.parent = targetPrefab.transform;
        //
        //
        // }
    }
}
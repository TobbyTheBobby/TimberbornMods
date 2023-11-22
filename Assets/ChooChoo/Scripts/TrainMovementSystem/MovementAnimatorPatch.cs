using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Timberborn.CharacterMovementSystem;

namespace ChooChoo
{
    [HarmonyPatch]
    public class MovementAnimatorPatch
    {
        public static readonly Dictionary<AnimatedPathFollower, bool> IsATrainOrWagon = new();

        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("MovementAnimator"), "InjectDependencies", new []
            {
                typeof(AnimatedPathFollower)
            });
        }

        static void Postfix(MovementAnimator __instance, AnimatedPathFollower ____animatedPathFollower)
        {
            IsATrainOrWagon.Add(____animatedPathFollower, __instance.TryGetComponentFast(out Train _) || __instance.TryGetComponentFast(out TrainWagon _));
        }
    }
    
    [HarmonyPatch]
    public class AnimatedPathFollowerPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("AnimatedPathFollower"), "UpdateVerticalMovement", new []
            {
                typeof(float)
            });
        }

        static bool Prefix(AnimatedPathFollower __instance, float verticalDistanceBetweenCorners)
        {
            if (!MovementAnimatorPatch.IsATrainOrWagon[__instance])
                return true;

            if (Math.Abs(verticalDistanceBetweenCorners) > 0.005)
            {
                ChooChooCore.SetInaccesibleProperty(__instance, "MovedUp", (double) verticalDistanceBetweenCorners > 0.0);
                ChooChooCore.SetInaccesibleProperty(__instance, "MovedDown", (double) verticalDistanceBetweenCorners < 0.0);
            }
            else
            {
                ChooChooCore.SetInaccesibleProperty(__instance, "MovedUp", false);
                ChooChooCore.SetInaccesibleProperty(__instance, "MovedDown", false);
            }

            return false;
        }
    }
}
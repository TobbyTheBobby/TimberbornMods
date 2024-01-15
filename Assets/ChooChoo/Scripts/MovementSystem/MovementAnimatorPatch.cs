using System;
using System.Collections.Generic;
using System.Reflection;
using ChooChoo.Trains;
using ChooChoo.Wagons;
using HarmonyLib;
using Timberborn.CharacterMovementSystem;
using TobbyTools.InaccessibilityUtilitySystem;
using TobbyTools.UsedImplicitlySystem;

namespace ChooChoo.MovementSystem
{
    [UsedImplicitlyHarmonyPatch]
    public class MovementAnimatorPatch
    {
        public static readonly Dictionary<AnimatedPathFollower, bool> IsATrainOrWagon = new();

        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("MovementAnimator"), "InjectDependencies", new[]
            {
                typeof(AnimatedPathFollower)
            });
        }

        private static void Postfix(MovementAnimator __instance, AnimatedPathFollower ____animatedPathFollower)
        {
            IsATrainOrWagon.Add(____animatedPathFollower,
                __instance.TryGetComponentFast(out Train _) || __instance.TryGetComponentFast(out TrainWagon _));
        }
    }

    [UsedImplicitlyHarmonyPatch]
    public class AnimatedPathFollowerPatch
    {
        private static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("AnimatedPathFollower"), "UpdateVerticalMovement", new[]
            {
                typeof(float)
            });
        }

        private static bool Prefix(AnimatedPathFollower __instance, float verticalDistanceBetweenCorners)
        {
            if (!MovementAnimatorPatch.IsATrainOrWagon[__instance])
                return true;

            if (Math.Abs(verticalDistanceBetweenCorners) > 0.005)
            {
                InaccessibilityUtilities.SetInaccessibleProperty(__instance, "MovedUp", verticalDistanceBetweenCorners > 0.0);
                InaccessibilityUtilities.SetInaccessibleProperty(__instance, "MovedDown", verticalDistanceBetweenCorners < 0.0);
            }
            else
            {
                InaccessibilityUtilities.SetInaccessibleProperty(__instance, "MovedUp", false);
                InaccessibilityUtilities.SetInaccessibleProperty(__instance, "MovedDown", false);
            }

            return false;
        }
    }
}
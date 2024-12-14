using System;
using System.Collections.Generic;
using System.Reflection;
using ChooChoo.Trains;
using ChooChoo.Wagons;
using HarmonyLib;
using JetBrains.Annotations;
using Timberborn.CharacterMovementSystem;

namespace ChooChoo.MovementSystem
{
    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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

    [HarmonyPatch]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
                __instance.MovedUp = verticalDistanceBetweenCorners > 0.0;
                __instance.MovedDown = verticalDistanceBetweenCorners < 0.0;
            }
            else
            {
                __instance.MovedUp = false;
                __instance.MovedDown = false;
            }

            return false;
        }
    }
}
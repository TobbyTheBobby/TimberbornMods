using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace ExampleMod.AnimationSystem
{
    [HarmonyPatch]
    public class LogginPatch
    {

        private static int _test;
        
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("ImportDetails"), "AddObject", new[] { typeof(GameObject), typeof(Timberborn.TimbermeshDTO.Node) });
        }

        public static void Prefix(GameObject createdObject, Timberborn.TimbermeshDTO.Node sourceNode)
        {
            if (_test  > 10)
                return;
            
            Debug.Log(createdObject.name);
            
            Debug.LogWarning(sourceNode.Name);
            foreach (var nodeAnimation in sourceNode.NodeAnimations)
            {
                Debug.LogError(nodeAnimation.Name);
                Debug.Log(nodeAnimation.Frames.Count);
            }
            foreach (var nodeAnimation in sourceNode.VertexAnimations)
            {
                Debug.LogError(nodeAnimation.Name);
                Debug.Log(nodeAnimation.Frames.Count);
            }
            ++_test;
        }
    }
}
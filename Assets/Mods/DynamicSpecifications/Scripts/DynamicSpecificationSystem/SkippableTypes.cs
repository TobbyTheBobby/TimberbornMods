using System;
using HarmonyLib;
using Timberborn.AssetSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace DynamicSpecifications
{
    public static class SkippableTypes
    {
        public static readonly Type[] Types = {
            typeof(MeshRenderer),
            typeof(UIDocument),
            typeof(MeshFilter),
            typeof(BinaryData),
            typeof(Transform),
            AccessTools.TypeByName("UniversalAdditionalCameraData"),
            AccessTools.TypeByName("UniversalAdditionalLightData"),
            AccessTools.TypeByName("ModelMetadata"),
            AccessTools.TypeByName("CanvasScaler"),
            
            // TODO Change fix that this class gets processed
            AccessTools.TypeByName("YielderSpecification"),
            AccessTools.TypeByName("Cuttable"),
        };
    }
}
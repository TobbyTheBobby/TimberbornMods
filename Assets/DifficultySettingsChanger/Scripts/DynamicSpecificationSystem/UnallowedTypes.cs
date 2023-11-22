using System;
using System.Collections.Generic;
using HarmonyLib;
using Timberborn.BlockSystem;
using Timberborn.PrefabOptimization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public static class UnallowedTypes
    {
        public static readonly Type[] Types = {
            // TODO Change fix that this class gets processed
            typeof(List<>).MakeGenericType(AccessTools.TypeByName("ProgressStep")),
            typeof(List<>).MakeGenericType(AccessTools.TypeByName("RecipeModel")),
            AccessTools.TypeByName("BlockObjectNavMeshEdgeSpecification").MakeArrayType(),
            AccessTools.TypeByName("NeedApplierEffectSpecification"),
            AccessTools.TypeByName("TransputSpecification"),
            AccessTools.TypeByName("BonusSpecification"),
            AccessTools.TypeByName("NeedSuspender"),
            AccessTools.TypeByName("FloatLimits"),
            typeof(List<string>),
            //
            
            
            typeof(AutoAtlasSpecification[]),
            typeof(List<Vector2Int>),
            typeof(ParticleSystem),
            typeof(List<Material>),
            typeof(PanelSettings),
            typeof(RectTransform),
            typeof(GameObject[]),
            typeof(BlockObject),        
            // typeof(StatusIcon),    
            typeof(List<Mesh>),
            typeof(UIDocument),
            typeof(GameObject),
            typeof(AudioMixer),
            typeof(Texture2D),
            typeof(Transform),
            typeof(Gradient),
            typeof(Material),
            typeof(Texture),
            typeof(Sprite),
            typeof(Image),
            typeof(Color),
            typeof(Mesh),
            AccessTools.TypeByName("MechanicalModelVariantSpecification").MakeArrayType(),
            AccessTools.TypeByName("WindRotator").MakeArrayType(),
            AccessTools.TypeByName("YielderSpecification"),
            AccessTools.TypeByName("RuinModelVariant"),
            AccessTools.TypeByName("PathModelVariant"),
            AccessTools.TypeByName("StatusIconCycler"),
            AccessTools.TypeByName("DayStageColors"),
            AccessTools.TypeByName("WindRotator"),
            AccessTools.TypeByName("Beaver"),
        };
    }
    
    public static class UnallowedFieldNames
    {
        public static readonly string[] Types = {
            "_toolOrder"
        };
    }
}
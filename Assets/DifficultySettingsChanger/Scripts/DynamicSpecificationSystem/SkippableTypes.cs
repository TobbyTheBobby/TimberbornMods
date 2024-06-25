using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TimberApi.Common.ConsoleSystem;
using TimberApi.ModSystem;
using Timberborn.AssetSystem;
using Timberborn.BlockSystem;
using Timberborn.SingletonSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger
{
    public static class SkippableTypes
    {
        private static Type[] _types;
        
        public static Type[] Types
        {
            get
            {
                if (_types != null) 
                    return _types;
                
                var unfilteredTypes = new List<Type>
                {
                    typeof(MeshRenderer),
                    typeof(WorldBlock),
                    typeof(UIDocument),
                    typeof(MeshFilter),
                    typeof(BinaryData),
                    typeof(StyleSheet),
                    typeof(Transform),
                    typeof(EventBus),
                    AccessTools.TypeByName("TimberApi.Core.ConsoleSystem.InternalConsoleWriter"),
                    AccessTools.TypeByName("UnityEngine.UIElements.StyleSheets.StyleValue"),
                    AccessTools.TypeByName("TimberApi.Core.LoggingSystem.Logger"),
                    AccessTools.TypeByName("UniversalAdditionalCameraData"),
                    AccessTools.TypeByName("ManualMigrationDistrictSetter"),
                    AccessTools.TypeByName("UniversalAdditionalLightData"),
                    AccessTools.TypeByName("UniversalAdditionalLightData"),
                    AccessTools.TypeByName("TickProgressPropertyUpdater"),
                    AccessTools.TypeByName("BepInExConsoleListener"),
                    AccessTools.TypeByName("ModelMetadata"),
                    AccessTools.TypeByName("CanvasScaler"),
                    AccessTools.TypeByName("Mod"),

                    // TODO Change fix that this class gets processed
                    AccessTools.TypeByName("YielderSpecification"),
                    AccessTools.TypeByName("Cuttable"),
                };

                _types = unfilteredTypes.Where(type => type != null).ToArray();
                return _types;
            }
        }
    }
}
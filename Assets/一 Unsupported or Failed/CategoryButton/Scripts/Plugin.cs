using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.Planting;
using Timberborn.ToolSystem;
using Timberborn.WaterSystemRendering;
using UnityEngine.UIElements;

namespace CategoryButton
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.categorybutton";
        
        public static IConsoleWriter Log;
        
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            Log = consoleWriter;
            
            try
            {
                new Harmony(PluginGuid).PatchAll();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
    [HarmonyPatch]
    [HarmonyPriority(399)]
    public class FactionObjectCollectionPatch
    {
        public static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("FactionObjectCollection"), "GetObjects");
        }

        private static void Postfix(ref IEnumerable<UnityEngine.Object> __result)
        {
            DependencyContainer.GetInstance<CategoryButtonService>().AddCategoryButtonsToObjectsPatch(ref __result);
        }
    }
    
    [HarmonyPatch]
    public class PreventInstantiatePatch
    {
        public static bool RunInstantiate = true;
        
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            var methodInfoList = new List<MethodInfo>
            {
                AccessTools.Method(AccessTools.TypeByName("BlockObject"), "Awake")
            };

            return methodInfoList;
        }

        private static bool Prefix()
        {
            return RunInstantiate;
        }
    }
    
    [HarmonyPatch]
    public class BlockObjectToolPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            var methodInfoList = new List<MethodInfo>
            {
                AccessTools.Method(AccessTools.TypeByName("BlockObjectToolButtonFactory"), "Create", new Type[]
                    {
                        typeof(PlaceableBlockObject), typeof(ToolGroup), typeof(VisualElement)
                    })
            };

            return methodInfoList;
        }

        private static bool Prefix(
            PlaceableBlockObject prefab,
            ToolGroup toolGroup,
            VisualElement buttonParent,
            BlockObjectToolDescriber ____blockObjectToolDescriber,
            ToolButtonFactory ____toolButtonFactory,
            ref ToolButton __result)
        {
            if (prefab.TryGetComponentFast(out CategoryButtonComponent toolBarCategory))
            {
                __result = DependencyContainer.GetInstance<CategoryButtonService>().CreateCategoryToolButton(prefab, toolGroup, buttonParent, toolBarCategory, ____blockObjectToolDescriber, ____toolButtonFactory);
                return false;
            }
            
            return true;
        }
    }
    
    [HarmonyPatch]
    public class ToolButtonFactoryPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            var methodInfoList = new List<MethodInfo>
            {
                AccessTools.Method(AccessTools.TypeByName("PlantingToolButtonFactory"), "CreatePlantingTool", new[]
                {
                    typeof(Plantable), typeof(VisualElement), typeof(ToolGroup)
                }),
                AccessTools.Method(AccessTools.TypeByName("BlockObjectToolButtonFactory"), "Create", new Type[]
                {
                    typeof(PlaceableBlockObject), typeof(ToolGroup), typeof(VisualElement)
                })
            };

            return methodInfoList;
        }

        private static void Postfix(ToolButton __result)
        {
            DependencyContainer.GetInstance<CategoryButtonService>().AddButtonToCategoryTool(__result);
        }
    }

    [HarmonyPatch(typeof(ToolManager), "SwitchTool", typeof(Tool))]
    public class ToolSwitcherPatch
    {
        private static void Prefix(ref ToolManager __instance, Tool tool, WaterOpacityToggle ____waterOpacityToggle)
        {
            DependencyContainer.GetInstance<CategoryButtonService>().SaveOrExitCategoryTool(__instance.ActiveTool, tool, ____waterOpacityToggle);
        }
    }
    
    [HarmonyPatch(typeof(ToolManager), "ExitTool", new Type[] {})]
    public class ExitToolPatch
    {
        private static bool Prefix(ref ToolManager __instance, WaterOpacityToggle ____waterOpacityToggle)
        {
            foreach (var toolBarCategoryTool in DependencyContainer.GetInstance<CategoryButtonService>().CategoryButtonTools)
            {
                if (__instance.ActiveTool == toolBarCategoryTool)
                {
                    ____waterOpacityToggle.ShowWater();
                    return false;
                }
            }

            return true;
        }
    }
    
    // [HarmonyPatch(typeof(ScreenSettingsController), "UpdateSettings", new Type[] {})]
    // public class ScreenSettingsPatch
    // {
    //     static void Postfix(ref ScreenSettingsController __instance, ScreenSettings ____screenSettings)
    //     {
    //         try { TimberAPI.DependencyContainer.GetInstance<CategoryButtonService>().UpdateScreenSize(); }
    //         catch (BindingNotFoundException e) {}
    //     }
    // }
}
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Bindito.Core;
using HarmonyLib;

namespace TobbyTools.CustomTutorialSystem
{
    [UsedImplicitlySystem.UsedImplicitlyHarmonyPatch]
    public class TutorialConfigurationProviderPatch
    {
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            // Plugin.Log.LogError("TargetMethods");
            
            return new[]
            {
                AccessTools.Method(AccessTools.TypeByName("TutorialSystemInitializationConfigurator"), "Configure",
                    new[]
                    {
                        typeof(IContainerDefinition),
                    }),
            };
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Plugin.Log.LogError("RUNNING TRANSPILER");
            
            // IL instructions
            List<CodeInstruction> code = new(instructions);

            int startIndex = -1;
            int endIndex = -1;

            for (var i = 0; i < code.Count - 1; i++)
            {
                // Plugin.Log.LogInfo(code[i + 1] + "");
                
                // Search for the ITutorialConfigurationProvider bind call
                // if (code[i].opcode != OpCodes.Ldarg_1 || code[i + 1].opcode != OpCodes.Callvirt || code[i + 1].operand.ToString() != "Bindito.Core.IBindingBuilder`1[Timberborn.TutorialSystemInitialization.ITutorialConfigurationProvider] Bind[ITutorialConfigurationProvider]()")
                    if (code[i].opcode != OpCodes.Ldarg_1 || code[i + 1].opcode != OpCodes.Callvirt || !code[i + 1].ToString().Contains("Timberborn.TutorialSystem.ITutorialConfigurationProvider"))
                {
                    continue;
                }

                // Loop to end of call to find last index
                for (int j = i; j < code.Count; j++)
                {
                    if (code[j].opcode != OpCodes.Callvirt || code[j].operand.ToString() != "Void AsSingleton()")
                    {
                        continue;
                    }

                    endIndex = j;
                    break;
                }

                startIndex = i;
                break;
            }

            // If method was not found skip
            if (startIndex == -1 && endIndex == -1)
            {
                return code;
            }
            
            // Plugin.Log.LogError("Code found");
            
            // Removes the method out the IL code
            code.RemoveRange(startIndex, endIndex - startIndex + 1);
            return code;
        }
    }
}
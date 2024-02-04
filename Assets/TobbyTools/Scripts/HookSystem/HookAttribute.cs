// https://github.com/PassivePicasso/RainOfStages_4/blob/main/LICENSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using MonoMod.RuntimeDetour;
using TimberApi.ConsoleSystem;

namespace TobbyTools.HookSystem
{
    internal class HookMap
    {
        public MethodInfo SourceMethod;
        public HookAttribute Hook;
    }
    
    [MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
    [UsedImplicitly]
    [AttributeUsage(AttributeTargets.Method)]
    public class HookAttribute : Attribute
    {
        private static readonly ParameterModifier[] EmptyPMs = Array.Empty<ParameterModifier>();
        public static IConsoleWriter Logger;

        private string MethodName { get; }
        private bool IsStatic { get; }
        private int Priority { get; }

        private static readonly List<(Type, Hook)> Hooks = new();

        private readonly Type _type;

        public HookAttribute(Type type, string methodName = null, bool isStatic = false, int priority = 0)
        {
            _type = type;
            MethodName = methodName;
            IsStatic = isStatic;
            Priority = priority;
        }

        private static IEnumerable<HookMap> GetRetargets(Type type, BindingFlags bindingFlags)
            => type.GetMethods(bindingFlags)
                   .Select(mi => new HookMap { SourceMethod = mi, Hook = mi.GetCustomAttributes<HookAttribute>().FirstOrDefault() })
                   .Where(dm => dm.Hook != null);
        
        // public static void ApplyHooks<T>() => ApplyHooks(typeof(T));
        
        public static void ApplyHooks(object instance)
        {
            var type = instance.GetType();
            
            var retargets = GetRetargets(type, BindingFlags.Public | BindingFlags.Static)
                     .Union(GetRetargets(type, BindingFlags.Public | BindingFlags.Instance))
                     .Union(GetRetargets(type, BindingFlags.NonPublic | BindingFlags.Static))
                     .Union(GetRetargets(type, BindingFlags.NonPublic | BindingFlags.Instance))
                     .ToArray();

            if (!retargets.Any()) 
                return;
            Logger?.LogInfo($"Configuring {retargets.Length} Hooks");
            foreach (var map in retargets)
            {
                try
                {
                    var bindingFlags = (map.Hook.IsStatic ? BindingFlags.Static : BindingFlags.Instance) | (map.SourceMethod.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic);

                    Logger?.LogWarning($"Source Method Binding Flags: {(map.SourceMethod.IsStatic ? $"{BindingFlags.Static}" : $"{BindingFlags.Instance}")} " +
                                       $" | {(map.SourceMethod.IsPublic ? $"{BindingFlags.Public}" : $"{BindingFlags.NonPublic}")}");

                    var parameterTypes = map.SourceMethod.GetParameters().Select(pi => pi.ParameterType).Skip(map.Hook.IsStatic ? 1 : 2).ToArray();

                    var parameterNames = map.SourceMethod.GetParameters().Select(pi => pi.Name).Skip(map.Hook.IsStatic ? 1 : 2).ToArray();

                    var paramString = String.Empty;
                    for (var i = 0; i < parameterNames.Length; i++)
                    {
                        if (i > 0) paramString += ",";
                        paramString += $"{parameterTypes[i].Name} {parameterNames[i]}";
                    }
                    Logger?.LogWarning($"Source Method: {map.SourceMethod.DeclaringType}.{map.SourceMethod.Name}({paramString})");

                    var targetMethodName = map.Hook.MethodName ?? map.SourceMethod.Name;
                    Logger?.LogWarning($"Target Method: {map.Hook._type}.{targetMethodName}({paramString})");

                    var targetMethod = map.Hook._type.GetMethod(targetMethodName, bindingFlags, null, parameterTypes, EmptyPMs);
                    Logger?.LogWarning($"Method: ({targetMethod?.Name ?? "Not Found"}) on {map.Hook._type.FullName}");
                        
                        
                    Logger?.LogWarning($"Target Method: {map.Hook._type}.{targetMethodName}({paramString})");
                    Logger?.LogInfo($"Hooking: {map.Hook._type}.{targetMethod?.Name ?? "Not Found"} hooked by ({type.FullName}.{map.SourceMethod.Name})");

                    Hooks.Add((type, new Hook(targetMethod, map.SourceMethod, instance)));
                }
                catch (Exception e)
                {
                    Logger?.LogError(e.ToString());
                }
            }
        }

        public static void DisableHooks(Type type)
        {
            var appliedHooks = Hooks.Where((hookType, _) => hookType.Equals(type));
            foreach (var (_, hook) in appliedHooks)
                hook.Undo();
        }
    }
}
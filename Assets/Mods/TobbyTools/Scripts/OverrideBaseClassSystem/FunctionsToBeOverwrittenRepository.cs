using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TobbyTools.OverrideBaseClassSystem
{
    public abstract class FunctionsToBeOverwrittenRepository
    {
        private static IEnumerable<MethodBase> _functionsToBeOverwritten;

        private static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static IEnumerable<MethodBase> FunctionsToBeOverwritten
        {
            get
            {
                if (_functionsToBeOverwritten == null)
                {
                    Index();
                }
                
                return _functionsToBeOverwritten;
            }
        }

        private static void Index()
        {
            var stopwatch = Stopwatch.StartNew();
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allClasses = assemblies.SelectMany(assembly => assembly.GetTypes());
            
            var list = new List<MethodBase>();

            IndexOverriddenBaseClassInterfaces(list, allClasses);

            _functionsToBeOverwritten = list;
            
            stopwatch.Stop();
            Plugin.Log.LogInfo($"Finished indexing in {stopwatch.ElapsedMilliseconds}ms.");
        }
        
        private static void IndexOverriddenBaseClassInterfaces(List<MethodBase> list, IEnumerable<Type> allClasses)
        {
            var classesWithOverridingBaseClassInterface = FilterClassesWithOverridingBaseClassInterface(allClasses);

            foreach (var classWithOverridingBaseClassInterface in classesWithOverridingBaseClassInterface)
            {
                var classMethods = GetClassMethods(classWithOverridingBaseClassInterface);
                var baseTypeMethods = GetBaseTypeMethods(classWithOverridingBaseClassInterface);

                AddBaseTypeMethodsToList(list, classMethods, baseTypeMethods);
            }
        }

        private static IEnumerable<Type> FilterClassesWithOverridingBaseClassInterface(IEnumerable<Type> allClasses)
        {
            var overridingBaseClassInterfaceType = typeof(IOverridingBaseClass);
            return allClasses.Where(type => overridingBaseClassInterfaceType.IsAssignableFrom(type) && type != overridingBaseClassInterfaceType);
        }

        private static MethodInfo[] GetClassMethods(Type classWithOverridingBaseClassInterface)
        {
            return classWithOverridingBaseClassInterface.GetMethods(BindingFlags | BindingFlags.DeclaredOnly);
        }

        private static MethodBase[] GetBaseTypeMethods(Type classWithOverridingBaseClassInterface)
        {
            return classWithOverridingBaseClassInterface.BaseType.GetMethods(BindingFlags);
        }

        private static void AddBaseTypeMethodsToList(List<MethodBase> list, MethodInfo[] classMethods, MethodBase[] baseTypeMethods)
        {
            foreach (var baseTypeMethod in baseTypeMethods)
            {
                if (classMethods.All(info => info.Name != baseTypeMethod.Name))
                    continue;

                if (list.Any(method => method.DeclaringType == baseTypeMethod.DeclaringType && method.Name == baseTypeMethod.Name))
                    continue;

                Plugin.Log.LogInfo($"{baseTypeMethod.DeclaringType} {baseTypeMethod.Name}");
                list.Add(baseTypeMethod);
            }
        }
    }
}
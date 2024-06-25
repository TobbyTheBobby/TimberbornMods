using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TobbyTools.UsedImplicitlySystem;

namespace TobbyTools.OverrideBaseClassSystem
{
    [UsedImplicitlyHarmonyPatch]
    public class FunctionOverridePatch
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return FunctionsToBeOverwrittenRepository.FunctionsToBeOverwritten;
        }

        public static bool Prefix(object __instance, MethodBase __originalMethod, object[] __args)
        {
            var instanceType = __instance.GetType();

            if (__originalMethod.DeclaringType == instanceType) 
                return true;

            if (!typeof(IOverridingBaseClass).IsAssignableFrom(instanceType))
                return true;
            
            var method = instanceType.GetMethod(__originalMethod.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null && method.GetCustomAttributes(typeof(OverwriteBaseFunction), false).Any())
            {
                method.Invoke(__instance, __args);
                return false;
            }

            return true;
        }
    }
}
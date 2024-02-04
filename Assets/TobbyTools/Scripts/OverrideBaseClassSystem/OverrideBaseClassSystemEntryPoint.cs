using System.Linq;
using HarmonyLib;

namespace TobbyTools.OverrideBaseClassSystem
{
    public class OverrideBaseClassSystemEntryPoint : ISubSystemEntryPoint
    {
        public void Entry()
        {
            if (FunctionsToBeOverwrittenRepository.FunctionsToBeOverwritten.Any())
            {
                Harmony.CreateAndPatchAll(typeof(FunctionOverridePatch), Plugin.PluginGuid);
            }
            
            TestingOverriding();
        }

        private static void TestingOverriding()
        {
            var test = new ClassA();
            test.Start();

            var test2 = new ClassB();
            test2.Start();

            var test3 = (ClassA)new ClassB();
            test3.Start();
            
            var test4 = new ClassC();
            test4.Start();
            
            var test5 = (ClassA)new ClassC();
            test5.Start();
        }

        private class ClassA
        {
            public void Start()
            {
                Test();
            }

            private void Test()
            {
                Plugin.Log.LogWarning("Original ClassA");
            }
        }

        private class ClassB : ClassA, IOverridingBaseClass
        {
            public void Test()
            {
                Plugin.Log.LogError("Overwritten ClassB");
            }
        }

        private class ClassC : ClassA
        {
            public void Test()
            {
                Plugin.Log.LogError("Overwritten ClassC");
            }
        }
    }
}
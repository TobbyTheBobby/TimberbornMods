using System.Collections.Generic;
using System.Reflection;

namespace MultithreadedNavigation
{
    public class RunMethodsOnMainThread
    {
        public readonly List<OriginalMethod> MethodsList = new();

        public class OriginalMethod
        {
            private readonly object _instance;
            private readonly MethodBase _methodBase;

            public OriginalMethod(object instance, MethodBase methodBase)
            {
                _instance = instance;
                _methodBase = methodBase;
            }

            public void RunMethod()
            {
                _methodBase.Invoke(_instance, new object[]{});
            }
        }

        public void UpdateAllVisibilities()
        {
            foreach (var method in MethodsList)
            {
                method.RunMethod();
            }
            MethodsList.Clear();
        }
    }
}
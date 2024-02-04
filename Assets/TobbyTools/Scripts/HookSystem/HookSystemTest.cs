using System;
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace TobbyTools.HookSystem
{
    public class HookSystemTest : ILoadableSingleton
    {
        private readonly EntityService _entityService;

        private HookSystemTest(EntityService entityService)
        {
            _entityService = entityService;
        }
        
        public void Load()
        {
            // TestingHooks();
        }

        private void TestingHooks()
        {
            var gameObject = new GameObject();

            var prefab = gameObject.AddComponent<Prefab>();

            _entityService.Instantiate(prefab);
            
            // var test = gameObject.AddComponent<MyBaseClass>();
            // test.Test();
            //
            // var test2 = gameObject.AddComponent<MySubClass>();
            // test2.Test();
            //
            // var test3 = (MyBaseClass)gameObject.AddComponent<MySubClass>();
            // test3.Test();
        }

        public class MyBaseClass : BaseComponent
        {
            public void Test()
            {
                Plugin.Log.LogWarning("Original ClassA");
            }
        }

        public class MySubClass : MyBaseClass, IHookRegister
        {
            // in this setup it uses the name of the decorated method
            // this needs to be public because we use this visibility to match against the visibility of MyBaseClass.MyOtherTest
            [Hook(typeof(MyBaseClass))] 
            public void Test(Action<MySubClass> original, MySubClass self)
            {
                // original(self);
                
                Plugin.Log.LogError("Overwritten ClassB");
            }
        }
    }
}
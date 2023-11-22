using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using TimberApi.Common.SingletonSystem;
using Timberborn.PathSystem;
using Timberborn.Persistence;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MorePaths
{
    public class MorePathsCore : ITimberApiLoadableSingleton
    {
        private readonly ISpecificationService _specificationService;
        private readonly PathSpecificationObjectDeserializer _pathSpecificationObjectDeserializer;

        private MethodInfo _methodInfo;
        public ImmutableArray<PathSpecification> PathsSpecifications;
        private List<CustomPath> _customPaths;
        public List<CustomPath> CustomPaths
        {
            get
            {
                if (_customPaths != null) 
                    return _customPaths;
                try
                {
                    _customPaths = TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance<CustomPathFactory>().CreatePathsFromSpecification();
                }
                catch (Exception)
                {
                    // ignored
                }
                return _customPaths;
            }
        }

        private static readonly Dictionary<string, FieldInfo> FieldInfos = new();
        private static readonly Dictionary<string, MethodInfo> MethodInfos = new();

        private static readonly BindingFlags PredefinedBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public MorePathsCore(ISpecificationService specificationService, PathSpecificationObjectDeserializer pathSpecificationObjectDeserializer)
        {
            _specificationService = specificationService;
            _pathSpecificationObjectDeserializer = pathSpecificationObjectDeserializer;
        }

        public void Load()
        {
            LoadPathSpecifications();
        }

        private void LoadPathSpecifications()
        {
            if (PathsSpecifications != null)
                return;
            var list = _specificationService.GetSpecifications(_pathSpecificationObjectDeserializer).Where(specification => specification.Enabled).ToList();
            var orderedList = list.OrderBy(specification => specification.ToolOrder);
            PathsSpecifications = orderedList.ToImmutableArray();
        }

        public void AddFakePathsToObjectsPatch(ref IEnumerable<Object> result)
        {
            var pathGameObject = result.First(o => o.name.Split(".")[0] == "Path");
            if (pathGameObject == null) 
                return;

            var resultList = result.ToList();
            resultList.Remove(pathGameObject);

            if (CustomPaths == null)
                return;

            var newObjectsList = resultList.Concat(CustomPaths.Select(path => path.GameObjectFast));

            result = newObjectsList;
        }
        
        public Texture2D TryLoadTexture(string pathName, string fileName, int width = 1024, int height = 1024)
        {
            var texture2D =  new Texture2D(width, height);
            if (pathName == null || fileName == null)
                return texture2D;
            var filePath = Path.Combine(Plugin.path, "Paths", pathName, fileName);
            if (File.Exists(filePath))
                texture2D.LoadImage(File.ReadAllBytes(filePath));
            return texture2D;
        }

        public object InvokeInaccesableMethod(object instance, string methodName, object[] args)
        {
            if (!MethodInfos.ContainsKey(methodName))
            {
                MethodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, PredefinedBindingFlags));
            }
            
            return MethodInfos[methodName].Invoke(instance, args);
        }
        
        public object InvokeInaccesableMethod(object instance, string methodName)
        {
            if (!MethodInfos.ContainsKey(methodName))
            {
                MethodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, PredefinedBindingFlags));
            }
            
            return MethodInfos[methodName].Invoke(instance, new object[]{});
        }
        
        public static object StaticInvokeInaccesableMethod(object instance, string methodName)
        {
            if (!MethodInfos.ContainsKey(methodName))
            {
                MethodInfos.Add(methodName, AccessTools.TypeByName(instance.GetType().Name).GetMethod(methodName, PredefinedBindingFlags));
            }
            
            return MethodInfos[methodName].Invoke(instance, new object[]{});
        }

        public void ChangePrivateField(object instance, string fieldName, object newValue)
        {
            if (!FieldInfos.ContainsKey(fieldName))
            {
                FieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, PredefinedBindingFlags));
            }
            
            FieldInfos[fieldName].SetValue(instance, newValue);
        }

        public object GetPrivateField(object instance, string fieldName)
        {
            if (!FieldInfos.ContainsKey(fieldName))
            {
                FieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, PredefinedBindingFlags));
            }
            
            return FieldInfos[fieldName].GetValue(instance);
        }
        
        public static object StaticGetPrivateField(object instance, string fieldName)
        {
            if (!FieldInfos.ContainsKey(fieldName))
            {
                FieldInfos.Add(fieldName, AccessTools.TypeByName(instance.GetType().Name).GetField(fieldName, PredefinedBindingFlags));
            }
            
            return FieldInfos[fieldName].GetValue(instance);
        }
    }
}
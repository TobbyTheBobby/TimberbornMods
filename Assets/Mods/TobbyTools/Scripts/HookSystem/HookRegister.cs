using System.Collections.Generic;
using Timberborn.BaseComponentSystem;

namespace TobbyTools.HookSystem
{
    public class HookRegister : BaseComponent 
    {
        public void Awake()
        {
            var list = new List<IHookRegister>();
            GetComponentsFast(list);
            foreach (var iHookRegister in list)
            {
                HookAttribute.ApplyHooks(iHookRegister);
            }
        }
    }
}
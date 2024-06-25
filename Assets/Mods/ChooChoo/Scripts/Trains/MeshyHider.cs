using Timberborn.BaseComponentSystem;
using UnityEngine;

namespace ChooChoo.Trains
{
    public class MeshyHider : BaseComponent
    {
        public GameObject _meshyObject;

        private void Start()
        {
            _meshyObject.SetActive(false);
        }
    }
}
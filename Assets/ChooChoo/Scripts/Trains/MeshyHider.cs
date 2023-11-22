using UnityEngine;

namespace ChooChoo
{
    public class MeshyHider : MonoBehaviour
    {
        public GameObject MeshyObject;
        
        private void Start()
        {
            MeshyObject.SetActive(false);
        }
    }
}
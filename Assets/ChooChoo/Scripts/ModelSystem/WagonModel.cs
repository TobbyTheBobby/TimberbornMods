using UnityEngine;

namespace ChooChoo.ModelSystem
{
    public class WagonModel
    {
        public readonly GameObject Model;

        public readonly WagonModelSpecification WagonModelSpecification;

        public WagonModel(GameObject model, WagonModelSpecification wagonModelSpecification)
        {
            Model = model;
            WagonModelSpecification = wagonModelSpecification;
        }
    }
}
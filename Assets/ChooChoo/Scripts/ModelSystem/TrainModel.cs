using UnityEngine;

namespace ChooChoo.ModelSystem
{
    public class TrainModel
    {
        public readonly GameObject Model;

        public readonly TrainModelSpecification TrainModelSpecification;

        public TrainModel(GameObject model, TrainModelSpecification trainModelSpecification)
        {
            Model = model;
            TrainModelSpecification = trainModelSpecification;
        }
    }
}
using Timberborn.BaseComponentSystem;
using UnityEngine;

namespace ChooChoo
{
    public class MachinistCharacterManager : BaseComponent
    {
        private bool _previousState;

        public void Start()
        {
            var trainModels = GetComponentFast<TrainModelManager>().TrainModels;
            foreach (var trainModel in trainModels)
            {
                var modelSpecification = trainModel.TrainModelSpecification;
                var machinist = ChooChooCore.FindBodyPart(trainModel.Model.transform, modelSpecification.MachinistSeatName);

                if (machinist == null)
                    continue;
                
                if (modelSpecification.MachinistScale == null)
                    continue;

                machinist.transform.localScale = new Vector3((float)modelSpecification.MachinistScale, (float)modelSpecification.MachinistScale, (float)modelSpecification.MachinistScale);
            }
        }
    }
}

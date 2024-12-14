using System;
using System.Linq;
using Bindito.Core;
using Timberborn.BaseComponentSystem;
using Timberborn.BlockObjectModelSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.MechanicalSystem;
using Timberborn.MechanicalSystemUI;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace VerticalPowerShaft
{
    public class VerticalPowerShaftComponent : BaseComponent, IModelUpdater
    {
        private EventBus _eventBus;
        private BlockService _blockService;
        
        private TransputPreviewValidator _transputPreviewValidator;
        
        [Inject]
        public void InjectDependencies(EventBus eventBus, BlockService blockService)
        {
            _eventBus = eventBus;
            _blockService = blockService;
        }

        private void Awake()
        {
            _transputPreviewValidator = GetComponentFast<TransputPreviewValidator>();
        }

        private void Start()
        {
            _eventBus.Register(this);

            foreach (Transform direction in GameObjectFast.FindChildTransform("Directions"))
            {
                direction.gameObject.SetActive(false);
            }
        }

        [OnEvent]
        public void UpdateDirections(MechanicalGraphGeneratorAddedEvent mechanicalGraphGeneratorAddedEvent)
        {
            UpdateModel();
        }
        
        public void UpdateModel()
        {
            try
            {
                var directionsParent = GameObjectFast.FindChildTransform("Directions");

                var previewTransputs = _transputPreviewValidator._previewMechanicalNode.PreviewTransputs;
                directionsParent.Find("Down").gameObject.SetActive(IsValid(previewTransputs[0]));
                directionsParent.Find("Left").gameObject.SetActive(IsValid(previewTransputs[1]));
                directionsParent.Find("Up").gameObject.SetActive(IsValid(previewTransputs[2]));
                directionsParent.Find("Right").gameObject.SetActive(IsValid(previewTransputs[3]));
                directionsParent.Find("TopBottom").gameObject.SetActive(IsValid(previewTransputs[4]) || IsValid(previewTransputs[5]));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private bool IsValid(PreviewTransput previewTransput)
        {
            var bottomObjectAt = _blockService.GetBottomObjectAt(previewTransput.Target);
            if (bottomObjectAt == null)
                return false;
            
            var componentFast = bottomObjectAt.GetComponentFast<PreviewMechanicalNode>();
            if (componentFast == null)
                return false;

            return componentFast.PreviewTransputs.Any(previewTransput.CanConnectTo);
        }
    }
}

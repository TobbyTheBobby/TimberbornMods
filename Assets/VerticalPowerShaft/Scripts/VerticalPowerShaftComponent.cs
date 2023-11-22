using System.Linq;
using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.Clusters;
using Timberborn.MechanicalSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace VerticalPowerShaft
{
    public class VerticalPowerShaftComponent : MonoBehaviour
    {
        private EventBus _eventBus;
        private BlockService _blockService;
        
        [Inject]
        public void InjectDependencies(EventBus eventBus, BlockService blockService)
        {
            _eventBus = eventBus;
            _blockService = blockService;
        }

        void Start()
        {
            _eventBus.Register(this);

            foreach (var obj in transform.GetChild(0).GetChild(0).Find("Directions"))
            {
                var transform = obj as Transform;
                transform.gameObject.SetActive(false);
            }

            UpdateDirections(null);
        }

        [OnEvent]
        public void UpdateDirections(MechanicalGraphGeneratorAddedEvent mechanicalGraphGeneratorAddedEvent)
        {
            var directionsParent = transform.GetChild(0).GetChild(0).Find("Directions");
            
            var pos = transform.position;

            directionsParent.Find("Up").gameObject.SetActive(IsMechanicalObject(new Vector3(pos.x, pos.z + 1, pos.y)));
            directionsParent.Find("Down").gameObject.SetActive(IsMechanicalObject(new Vector3(pos.x , pos.z - 1, pos.y)));
            directionsParent.Find("Right").gameObject.SetActive(IsMechanicalObject(new Vector3(pos.x  + 1, pos.z, pos.y)));
            directionsParent.Find("Left").gameObject.SetActive(IsMechanicalObject(new Vector3(pos.x - 1, pos.z, pos.y)));
            directionsParent.Find("TopBottom").gameObject.SetActive(IsOccupied(new Vector3(pos.x, pos.z, pos.y + 1)) || IsMechanicalObject(new Vector3(pos.x, pos.z, pos.y - 1)));
        }

        private bool IsMechanicalObject(Vector3 coords)
        {
            bool flag1 = _blockService.GetObjectsWithComponentAt<TransputSpecificationsProvider>(Vector3Int.FloorToInt(coords)).Any();
            bool flag2 = _blockService.GetObjectsWithComponentAt<MechanicalNodeSpecification>(Vector3Int.FloorToInt(coords)).Any();
            bool flag3 = _blockService.GetObjectsWithComponentAt<ClusterElement>(Vector3Int.FloorToInt(coords)).Any();
            return flag1 || flag2 || flag3;
        }
        
        private bool IsOccupied(Vector3 coords)
        {
            bool flag1 = _blockService.GetFloorObjectAt(Vector3Int.FloorToInt(coords));
            return flag1 ;
        }
    }
}

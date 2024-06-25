using System.Collections.Generic;
using System.Linq;
using ChooChoo.Trains;
using ChooChoo.Wagons;
using HarmonyLib;
using Timberborn.BaseComponentSystem;
using Timberborn.Carrying;
using Timberborn.Characters;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine;

namespace ChooChoo.DestroySystem
{
    public class Destroyable : BaseComponent
    {
        private object _recoveredGoodStackSpawner;
        private GoodReserver _goodReserver;
        private Character _character;
        private Train _train;

        private IEnumerable<GoodCarrier> GoodCarriers =>
            GetComponentFast<WagonManager>().Wagons.Select(wagon => wagon.GetComponentFast<GoodCarrier>());

        public void Awake()
        {
            _recoveredGoodStackSpawner =
                TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance(AccessTools.TypeByName("RecoveredGoodStackSpawner"));
            _goodReserver = GetComponentFast<GoodReserver>();
            _character = GetComponentFast<Character>();
            _train = GetComponentFast<Train>();
        }

        public void Destroy()
        {
            _goodReserver.UnreserveStock();
            InaccessibilityUtilities.SetInaccessibleProperty(_goodReserver, "CapacityReservation", new GoodReservation());
            GameObjectFast.SetActive(false);
            _character.DestroyCharacter();
            var position = TransformFast.position;
            var wrongPosition = new Vector3(position.x, position.z, position.y);
            InaccessibilityUtilities.InvokeInaccessibleMethod(_recoveredGoodStackSpawner, "AddAwaitingGoods",
                new object[] { Vector3Int.RoundToInt(wrongPosition), GetAllGoods() });
        }

        private List<GoodAmount> GetAllGoods()
        {
            var allGoods = new List<GoodAmount>();
            allGoods.AddRange(_train.TrainCost.Select(specification => specification.ToGoodAmount()));
            allGoods.AddRange(GoodCarriers.Where(carrier => carrier.IsCarrying).Select(carrier => carrier.CarriedGoods).ToList());
            return allGoods;
        }
    }
}
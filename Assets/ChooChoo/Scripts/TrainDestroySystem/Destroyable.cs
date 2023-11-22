using System.Collections.Generic;
using System.Linq;
using Bindito.Core;
using HarmonyLib;
using Timberborn.Carrying;
using Timberborn.Characters;
using Timberborn.Goods;
using Timberborn.InventorySystem;
using UnityEngine;

namespace ChooChoo
{
  public class Destroyable : MonoBehaviour
  {
    private object _recoveredGoodStackSpawner;
    private Character _character;
    private Train _train;
    private GoodReserver _goodReserver;

    private List<GoodCarrier> _goodCarriers;

    [Inject]
    public void InjectDependencies(TrainYardService trainYardService)
    {
       _recoveredGoodStackSpawner = TimberApi.DependencyContainerSystem.DependencyContainer.GetInstance(AccessTools.TypeByName("RecoveredGoodStackSpawner"));
    }

    public void Awake()
    {
      _character = GetComponent<Character>();
      _train = GetComponent<Train>();
      _goodReserver = GetComponent<GoodReserver>();
    }

    public void Start()
    {
      _goodCarriers = GetComponent<WagonManager>().Wagons.Select(wagon => wagon.GetComponentFast<GoodCarrier>()).ToList();
    }

    public void Destroy()
    {
      _goodReserver.UnreserveStock();
      ChooChooCore.SetInaccesibleProperty(_goodReserver, "CapacityReservation", new GoodReservation());
      gameObject.SetActive(false);
      _character.DestroyCharacter();
      var position = transform.position;
      var wrongPosition = new Vector3(position.x, position.z, position.y);
      ChooChooCore.InvokePublicMethod(_recoveredGoodStackSpawner, "AddAwaitingGoods", new object[]{ Vector3Int.RoundToInt(wrongPosition), GetAllGoods()});
    }

    private List<GoodAmount> GetAllGoods()
    {
      List<GoodAmount> allGoods = new List<GoodAmount>();
      allGoods.AddRange(_train.TrainCost.Select(specification => specification.ToGoodAmount()));
      allGoods.AddRange(_goodCarriers.Where(carrier => carrier.IsCarrying).Select(carrier => carrier.CarriedGoods).ToList());
      return allGoods;
    }
  }
}

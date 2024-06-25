// using Bindito.Core;
// using GlobalMarket;
// using Timberborn.TickSystem;
// using UnityEngine;
//
// public class PilotCharacterManager : TickableComponent
// {
//     private PilotCharacterFactory _pilotCharacterFactory;
//     private FlyingRootBehavior _flyingRootBehavior;
//
//     private GameObject _pilot;
//
//     private bool _previousState;
//
//     [Inject]
//     public void InjectDependencies(PilotCharacterFactory pilotCharacterFactory)
//     {
//         _pilotCharacterFactory = pilotCharacterFactory;
//     }
//
//     private void Awake()
//     {
//         _pilot = _pilotCharacterFactory.CreatePilot(transform.GetChild(0).GetChild(0));
//         _flyingRootBehavior = GetComponent<FlyingRootBehavior>();
//     }
//     
//     private new void Start()
//     {
//         _pilot.GetComponent<Animator>().SetBool("Sitting", true);
//     }
//
//     public override void Tick()
//     {
//         var newState = _flyingRootBehavior.IsReturned;
//         if (!StateHasChanged(newState)) 
//             return;
//         _previousState = newState;
//         _pilot.SetActive(!newState);
//         _pilot.GetComponent<Animator>().SetBool("Sitting", true);
//     }
//
//     private bool StateHasChanged(bool newState) => _previousState != newState;
// }

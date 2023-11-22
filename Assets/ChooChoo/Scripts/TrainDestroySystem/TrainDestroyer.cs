// using Timberborn.InputSystem;
// using Timberborn.SelectionSystem;
// using Timberborn.SingletonSystem;
//
// namespace ChooChoo
// {
//   public class TrainDestroyer : ILoadableSingleton, IInputProcessor
//   {
//     private readonly EventBus _eventBus;
//     private readonly InputService _inputService;
//     private Destroyable _selectedDestroyable;
//
//     public object GoodStackRecoverer;
//
//     public TrainDestroyer(EventBus eventBus, InputService inputService)
//     {
//       _eventBus = eventBus;
//       _inputService = inputService;
//     }
//
//     public void Load()
//     {
//       _eventBus.Register(this);
//       _inputService.AddInputProcessor(this);
//     }
//
//     [OnEvent]
//     public void OnGameObjectSelectedEvent(GameObjectSelectedEvent gameObjectSelectedEvent)
//     {
//       Destroyable componentInParent = gameObjectSelectedEvent.GameObject.GetComponentInParent<Destroyable>();
//       if (!(bool) (UnityEngine.Object) componentInParent)
//         return;
//       _selectedDestroyable = componentInParent;
//     }
//
//     [OnEvent]
//     public void OnGameObjectUnselectedEvent(GameObjectUnselectedEvent gameObjectUnselectedEvent)
//     {
//       _selectedDestroyable = null;
//     }
//
//     public bool ProcessInput()
//     {
//       if (!_inputService.KillSelectedCharacter || !(bool) (UnityEngine.Object) _selectedDestroyable)
//         return false;
//       _selectedDestroyable.Destroy();
//       return true;
//     }
//   }
// }

using Timberborn.BehaviorSystem;

namespace ChooChoo
{
  public class BringDistributableGoodTrainBehavior : RootBehavior
  {
    private DistributableGoodBringerTrain _distributableGoodBringerTrain;

    private void Awake() => _distributableGoodBringerTrain = GetComponentFast<DistributableGoodBringerTrain>();

    public override Decision Decide(BehaviorAgent agent) => !_distributableGoodBringerTrain.BringDistributableGoods() ? Decision.ReleaseNow() : Decision.ReleaseNextTick();
  }
}

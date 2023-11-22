using Timberborn.BaseComponentSystem;
using Timberborn.ConstructibleSystem;

namespace ChooChoo
{
  public class GoodObtainer : BaseComponent, IFinishedStateListener
  {
    public bool GoodObtainingEnabled { get; private set; }

    public void OnEnterFinishedState()
    {
      EnableGoodObtaining();
    }

    public void OnExitFinishedState()
    {
      DisableGoodObtaining();
    }

    private void EnableGoodObtaining()
    {
      GoodObtainingEnabled = true;
    }

    private void DisableGoodObtaining()
    {
      GoodObtainingEnabled = false;
    }
  }
}

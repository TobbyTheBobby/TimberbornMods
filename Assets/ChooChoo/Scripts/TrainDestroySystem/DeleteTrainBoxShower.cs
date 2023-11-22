using System;
using Timberborn.CoreUI;

namespace ChooChoo
{
  public class DeleteTrainBoxShower
  {
    private readonly DialogBoxShower _dialogBoxShower;

    public DeleteTrainBoxShower(DialogBoxShower dialogBoxShower)
    {
      _dialogBoxShower = dialogBoxShower;
    }

    public void Show(Action confirmAction, string promptLocKey)
    {
      _dialogBoxShower.Create().SetLocalizedMessage(promptLocKey).SetConfirmButton(confirmAction).SetDefaultCancelButton().Show();
    }
  }
}

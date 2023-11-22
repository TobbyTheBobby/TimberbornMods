using System;
using Timberborn.Localization;
using Timberborn.MechanicalSystem;
using UnityEngine;

namespace ChooChoo
{
  public static class MechanicalNodeTextFormatter
  {
    private static readonly string PowerInputMaximumLocKey = "Mechanical.PowerInputMaximum";
    private static readonly string PowerSymbolLocKey = "Mechanical.PowerSymbol";
    private static readonly string PowerOutputLocKey = "Mechanical.PowerOutput";

    public static string FormatGeneratorText(ILoc loc, GameObject train) => loc.T<string>(MechanicalNodeTextFormatter.PowerOutputLocKey, string.Format("{0} {1}", train, loc.T(PowerSymbolLocKey)));

    public static string FormatConsumerText(ILoc loc, MechanicalNode mechanicalNode)
    {
      double num1 = mechanicalNode.Active ? (double) mechanicalNode.PowerEfficiency : 0.0;
      int num2 = Mathf.RoundToInt((float) (num1 * 100.0));
      int val2 = Mathf.RoundToInt((float) num1 * (float) mechanicalNode.PowerInput);
      int num3 = Math.Min(mechanicalNode.PowerInput, val2);
      string str = string.Format("{0} {1}", (object) mechanicalNode.PowerInput, (object) loc.T(MechanicalNodeTextFormatter.PowerSymbolLocKey));
      return loc.T<int, string, int>(MechanicalNodeTextFormatter.PowerInputMaximumLocKey, num3, str, num2);
    }
  }
}

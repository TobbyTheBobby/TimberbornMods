using System;
using UnityEngine.UIElements;

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
  public static class TextFields
  {
    public static void InitializeFloatTextField(
      TextField textField,
      float startingValue,
      float minValue = 0,
      float maxValue = 2147483647,
      Action<float> midEditingCallback = null,
      Action<float> afterEditingCallback = null)
    {
      textField.SetValueWithoutNotify(startingValue.ToString());
      // textField.RegisterCallback((EventCallback<ChangeEvent<string>>)(e =>
      // {
      //   ValidateFloatTextFieldMidEditing(textField, minValue, e);
      //   Action<float> action = midEditingCallback;
      //   if (action == null)
      //     return;
      //   action(float.Parse(textField.value));
      // }));
      textField.RegisterCallback((EventCallback<FocusOutEvent>)(_ =>
      {
        ValidateIntTextFieldAfterEditing(textField, minValue, maxValue);
        Action<float> action = afterEditingCallback;
        if (action == null)
          return;
        action(float.Parse(textField.value));
      }));
    }

    private static void ValidateFloatTextFieldMidEditing(
      TextField textField,
      float minValue,
      ChangeEvent<string> e)
    {
      if (string.IsNullOrWhiteSpace(e.newValue))
      {
        textField.SetValueWithoutNotify(minValue.ToString());
        textField.SelectRange(int.MaxValue, int.MaxValue);
      }
      else
      {
        if (!float.TryParse(e.newValue, out var result) || result < 0)
          textField.SetValueWithoutNotify(e.previousValue);
        else
          textField.SetValueWithoutNotify(e.newValue.Trim());
      }
    }

    private static void ValidateIntTextFieldAfterEditing(
      TextField textField,
      float minValue,
      float maxValue)
    {
      var s = textField.value;
      if (string.IsNullOrWhiteSpace(s))
      {
        textField.SetValueWithoutNotify(minValue.ToString());
      }
      else
      {
        var num = Math.Clamp(float.Parse(s), minValue, maxValue);
        textField.SetValueWithoutNotify(num.ToString());
      }
    }
  }
}
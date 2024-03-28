using UnityEngine.UIElements;

namespace TobbyTools.NewGameModeValueSystem
{
    public interface INewGameModeValue
    {
        string Section { get; }
        VisualElement GetVisualElement();
        void OnEasyModeButtonClicked();
        void OnNormalModeButtonClicked();
        void OnHardModeButtonClicked();
        void SetState(bool newState);
    }
}
using Timberborn.AssetSystem;
using Timberborn.BottomBarSystem;
using Timberborn.CoreUI;
using UnityEngine;
using UnityEngine.UIElements;
using ToolManager = Timberborn.ToolSystem.ToolManager;

namespace PipetteTool.PipetteToolSystem
{
    public class PipetteToolButton : IBottomBarElementProvider
    {
        private readonly VisualElementLoader _visualElementLoader;
        private readonly PipetteTool _pipetteTool;
        private readonly IAssetLoader _assetLoader;
        private readonly ToolManager _toolManager;

        public PipetteToolButton(
            VisualElementLoader visualElementLoader,
            PipetteTool pipetteTool,
            IAssetLoader assetLoader,
            ToolManager toolManager)
        {
            _visualElementLoader = visualElementLoader;
            _pipetteTool = pipetteTool;
            _assetLoader = assetLoader;
            _toolManager = toolManager;
        }

        public BottomBarElement GetElement()
        {
            var visualElement = _visualElementLoader.LoadVisualElement("Common/BottomBar/GrouplessToolButton");
            visualElement.AddToClassList("bottom-bar-button--blue");
            var sprite = _assetLoader.Load<Sprite>("Tobbert/PipetteToolIcon");
            visualElement.Q<VisualElement>("ToolImage").style.backgroundImage = new StyleBackground(sprite);
            visualElement.Q<Button>("ToolButton").RegisterCallback<ClickEvent>(_ => _toolManager.SwitchTool(_pipetteTool));
            return BottomBarElement.CreateSingleLevel(visualElement);
        }
    }
}
using TimberApi.DependencyContainerSystem;
using TimberApi.UiBuilderSystem;
using TimberApi.UiBuilderSystem.PresetSystem;
using Timberborn.AssetSystem;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.Localization;
using Timberborn.Persistence;
using Timberborn.SingletonSystem;
using UnityEngine.UIElements;
using LocalizableLabel = TimberApi.UiBuilderSystem.CustomElements.LocalizableLabel;

namespace DifficultySettingsChanger
{
    public class DifficultySettingsChangerBox : ILoadableSingleton, IPanelController, IInputProcessor
    {
        private static readonly string OpenUILocKey = "Tobbert.GameValueChanger.KeyBinding.OpenUi";
        
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly VisualElementInitializer _visualElementInitializer;
        private readonly IResourceAssetLoader _resourceAssetLoader;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly UIBuilder _uiBuilder;
        private readonly UiPresetFactory _uiPresetFactory;
        private readonly PanelStack _panelStack;
        // private readonly KeyboardController _keyboard;
        private readonly InputService _inputService;
        private readonly ISpecificationService _specificationService;
        private readonly ILoc _loc;
        private readonly GameValueChangerService _gameValueChangerService;
        private readonly EventBus _eventBus;

        private ScrollView _rootScrollView;

        public DifficultySettingsChangerBox(
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory,
            VisualElementInitializer visualElementInitializer,
            IResourceAssetLoader resourceAssetLoader,
            VisualElementLoader visualElementLoader,
            UIBuilder uiBuilder, 
            UiPresetFactory uiPresetFactory,
            PanelStack panelStack, 
            // KeyboardController keyboard, 
            InputService inputService,
            GameValueChangerService gameValueChangerService,
            EventBus eventBus)
        {
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _visualElementInitializer = visualElementInitializer;
            _resourceAssetLoader = resourceAssetLoader;
            _visualElementLoader = visualElementLoader;
            _uiBuilder = uiBuilder;
            _uiPresetFactory = uiPresetFactory;
            _panelStack = panelStack;
            // _keyboard = keyboard;
            _inputService = inputService;
            _gameValueChangerService = gameValueChangerService;
            _eventBus = eventBus;
        }
        
        public void Load()
        {
            _inputService.AddInputProcessor(this);
        }
        
        public bool ProcessInput()
        {
            if (_inputService.IsKeyDown(OpenUILocKey))
            {
                _panelStack.PushOverlay(this);
            }

            return false;
        }
        
        public VisualElement GetPanel()
        {
            // var disclaimer = _uiPresetFactory.Labels().DefaultBold("Tobbert.DifficultySettingsChanger.Disclaimer");
            // disclaimer.style.marginTop = new Length(30, LengthUnit.Pixel);
            // disclaimer.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            // settingChangerContainer.Add(disclaimer);
            //
            // var button = _uiPresetFactory.Buttons().ButtonGame(locKey: "Tobbert.DifficultySettingsChanger.ApplySettings", name: "SettingsApplierButton");
            // button.style.marginTop = new Length(40, LengthUnit.Pixel);
            // settingChangerContainer.Add(button);
            
            // var box = _uiBuilder.CreateBoxBuilder()
            //     .SetBoxInCenter()
            //     .AddCloseButton("CloseButton")
            //
            //     .AddPreset(_ =>
            //     {
            //         var visualElement = new NineSliceVisualElement();
            //         visualElement.AddToClassList("mods-box__navigation");
            //         return visualElement;
            //     })
            //     
            //     .AddComponent(settingChangerContainer)
            //
            //     .BuildAndInitialize();

            var box = new NineSliceVisualElement();
            box.styleSheets.Add(_resourceAssetLoader.Load<StyleSheet>("UI/Views/Core/CoreStyle"));
            box.styleSheets.Add(_resourceAssetLoader.Load<StyleSheet>("UI/Views/Core/CoreStyle"));
            box.styleSheets.Add(_resourceAssetLoader.Load<StyleSheet>($"{Plugin.Mod.UniqueId}/Assets/GameValueChangerStyle"));
            box.AddToClassList("mods-box");
            box.AddToClassList("sliced-border");
            box.AddToClassList("content-centered");

            var headerWrapper = new VisualElement();
            headerWrapper.AddToClassList("capsule-header");
            headerWrapper.AddToClassList("capsule-header--lower");
            headerWrapper.AddToClassList("content-centered");
            var localizableLabel = new LocalizableLabel();
            localizableLabel.AddToClassList("capsule-header__text");
            localizableLabel.TextLocKey = "Tobbert.DifficultySettingsChanger.Header";
            headerWrapper.Add(localizableLabel);
            box.Add(headerWrapper);

            var wrapper = new VisualElement();
            wrapper.AddToClassList("panel-list-view");
            wrapper.AddToClassList("mods-box__wrapper");
            
            var filterContainer = new NineSliceVisualElement();
            filterContainer.AddToClassList("mods-box__navigation");

            var searchWrapper = new VisualElement();
            searchWrapper.AddToClassList("content-row-centered--no-grow");
            
            var searchField = new NineSliceTextField();
            searchField.AddToClassList("mods-box__search");
            searchField.AddToClassList("text-field");
            searchWrapper.Add(searchField);

            // var searchButton = new Button();
            // searchButton.AddToClassList("mods-box__search-button");
            // searchWrapper.Add(searchButton);

            filterContainer.Add(searchWrapper);

            wrapper.Add(filterContainer);

            var valueChangerWrapper = new VisualElement();
            valueChangerWrapper.AddToClassList("mods-box__value-changer-wrapper");

            _rootScrollView = new ScrollView();
            _rootScrollView.AddToClassList("mods-box__mods");
            _rootScrollView.AddToClassList("scroll--green-decorated");
            _rootScrollView.mode = ScrollViewMode.Horizontal;
            
            var settingChangerContainer = _gameValueChangerUiPresetFactory.GetGameValueChangerContainer("Property");
            var settingChangerContainerScrollView = settingChangerContainer.Q<ScrollView>();
            foreach (var child in _gameValueChangerService.GetElements()) 
                settingChangerContainerScrollView.Add(child);
            
            var hierarchicalManager = DependencyContainer.GetInstance<HierarchicalManager>();
            hierarchicalManager.OpenNewLayer(new HierarchicalLayer(1, new VisualElement()));
            
            _rootScrollView.Add(settingChangerContainer);

            valueChangerWrapper.Add(_rootScrollView);
            
            wrapper.Add(valueChangerWrapper);

            box.Add(wrapper);
            // var scrollView = box.Q<ScrollView>();
            // var parent = scrollView.parent;
            // foreach (var child in scrollView.Children().ToList()) 
            //     parent.Add(child);
            // parent.Remove(scrollView);
            //
            // // LogChildren(box);
            //
            // box.Q<Button>("SettingsApplierButton").RegisterCallback<ClickEvent>(_ => OnUIConfirmed());
            // box.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(_ => OnUICancelled());

            _visualElementInitializer.InitializeVisualElement(box);
            
            return box;
        }

        private void LogChildren(VisualElement parent)
        {
            foreach (var child in parent.Children())
            {
                Plugin.Log.LogInfo(child + "");
                LogChildren(child);
            }
        }

        public bool OnUIConfirmed()
        {
            _panelStack.Pop(this);
            _eventBus.Post(new OnGameValueChangerBoxConfirmed());
            DependencyContainer.GetInstance<HierarchicalManager>().Clear();
            return true;
        }

        public void OnUICancelled()
        {
            _panelStack.Pop(this);
            DependencyContainer.GetInstance<HierarchicalManager>().Clear();
        }

        public void AddLayer(VisualElement visualElement)
        {
            // var scrollView = new ScrollView();
            // scrollView.AddToClassList("mods-box__game-value-changer");
            // scrollView.AddToClassList("scroll--green-decorated");
            // scrollView.Add(visualElement);
            _rootScrollView.Add(visualElement);
            
            
            // for (var i = 0; i < 4; i++)
            // {
            //     var scrollView = new ScrollView();
            //     scrollView.AddToClassList("mods-box__game-value-changer");
            //     scrollView.AddToClassList("scroll--green-decorated");
            //     var settingChangerContainer2 = _gameValueChangerUiPresetFactory.GetGameValueChangerContainer();
            //     foreach (var child in _gameValueChangerService.GetElements())
            //     {
            //         settingChangerContainer2.Add(child);
            //     }
            //     scrollView.Add(settingChangerContainer2);
            //     _rootScrollView.Add(scrollView);
            // }
        }
        
        public void RemoveLayer(VisualElement visualElement)
        {
            _rootScrollView.Remove(visualElement);
            
            
            // for (var i = 0; i < 4; i++)
            // {
            //     var scrollView = new ScrollView();
            //     scrollView.AddToClassList("mods-box__game-value-changer");
            //     scrollView.AddToClassList("scroll--green-decorated");
            //     var settingChangerContainer2 = _gameValueChangerUiPresetFactory.GetGameValueChangerContainer();
            //     foreach (var child in _gameValueChangerService.GetElements())
            //     {
            //         settingChangerContainer2.Add(child);
            //     }
            //     scrollView.Add(settingChangerContainer2);
            //     _rootScrollView.Add(scrollView);
            // }
        }
    }
}
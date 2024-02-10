using System.Collections.Generic;
using System.Linq;
using DifficultySettingsChanger.GameValueChangerSystem;
using DifficultySettingsChanger.GameValueChangerSystemUI.Components;
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

namespace DifficultySettingsChanger.GameValueChangerSystemUI
{
    public class DifficultySettingsChangerBox : ILoadableSingleton, IPanelController, IInputProcessor
    {
        private static readonly string OpenUILocKey = "Tobbert.GameValueChanger.KeyBinding.OpenUi";
        
        private readonly GameValueChangerUiPresetFactory _gameValueChangerUiPresetFactory;
        private readonly GameValueChangerRepository _gameValueChangerRepository;
        private readonly VisualElementInitializer _visualElementInitializer;
        private readonly IResourceAssetLoader _resourceAssetLoader;
        private readonly VisualElementLoader _visualElementLoader;
        private readonly PanelStack _panelStack;
        private readonly InputService _inputService;
        private readonly ISpecificationService _specificationService;
        private readonly ILoc _loc;
        private readonly GameValueChangerService _gameValueChangerService;
        private readonly EventBus _eventBus;

        private GameValueChangersFilter _gameValueChangersFilter;
        
        private VisualElement _root;
        private TextField _searchField;
        
        private ScrollView _objectsContainer;
        private ScrollView _propertiesContainer;
        public static ScrollView GameValueChangerContainer;

        private ObjectWrapper[] _objectWrappers;

        public DifficultySettingsChangerBox(
            GameValueChangerUiPresetFactory gameValueChangerUiPresetFactory,
            GameValueChangerRepository gameValueChangerRepository,
            VisualElementInitializer visualElementInitializer,
            IResourceAssetLoader resourceAssetLoader,
            VisualElementLoader visualElementLoader,
            PanelStack panelStack, 
            InputService inputService,
            GameValueChangerService gameValueChangerService,
            EventBus eventBus)
        {
            _gameValueChangerUiPresetFactory = gameValueChangerUiPresetFactory;
            _gameValueChangerRepository = gameValueChangerRepository;
            _visualElementInitializer = visualElementInitializer;
            _resourceAssetLoader = resourceAssetLoader;
            _visualElementLoader = visualElementLoader;
            _panelStack = panelStack;
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
            if (_root != null)
            {
                ApplyFilter();
                return _root;
            }
            
            // TODO SHOW DISCLAIMER THAT USING THE MOD CAN BREAK OTHER MODS VERY EASILY AND THEY SHOULD THEREFORE NOT REPORT BUGS WHEN THIS MOD IS INSTALLED. 

            // var disclaimer = _uiPresetFactory.Labels().DefaultBold("Tobbert.DifficultySettingsChanger.Disclaimer");
            // disclaimer.style.marginTop = new Length(30, LengthUnit.Pixel);
            // disclaimer.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            // settingChangerContainer.Add(disclaimer);
            //
            // var button = _uiPresetFactory.Buttons().ButtonGame(locKey: "Tobbert.DifficultySettingsChanger.ApplySettings", name: "SettingsApplierButton");
            // button.style.marginTop = new Length(40, LengthUnit.Pixel);
            // settingChangerContainer.Add(button);

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
            searchField.RegisterValueChangedCallback(_ => ApplyFilter());
            _searchField = searchField;
            searchWrapper.Add(searchField);

            // var searchButton = new Button();
            // searchButton.AddToClassList("mods-box__search-button");
            // searchWrapper.Add(searchButton);

            filterContainer.Add(searchWrapper);

            wrapper.Add(filterContainer);

            var valueChangerWrapper = new VisualElement();
            valueChangerWrapper.AddToClassList("mods-box__value-changer-wrapper");

            var root = new VisualElement();
            root.AddToClassList("mods-box__mods");
            root.AddToClassList("scroll--green-decorated");
            
            var classesContainer = _gameValueChangerUiPresetFactory.GetObjectsContainer("Objects");
            _objectsContainer = classesContainer.Q<ScrollView>();
            root.Add(classesContainer);
            
            var propertiesWrapper = _gameValueChangerUiPresetFactory.GetObjectsContainer("Property");
            _propertiesContainer = propertiesWrapper.Q<ScrollView>();
            root.Add(propertiesWrapper);
            
            var gameValueChangerContainer = _gameValueChangerUiPresetFactory.GetGameValueChangerContainer("Game Value Changer");
            GameValueChangerContainer = gameValueChangerContainer.Q<ScrollView>();
            root.Add(gameValueChangerContainer);

            valueChangerWrapper.Add(root);
            
            wrapper.Add(valueChangerWrapper);

            box.Add(wrapper);
            
            // LogChildren(box);
            //
            // box.Q<Button>("SettingsApplierButton").RegisterCallback<ClickEvent>(_ => OnUIConfirmed());
            // box.Q<Button>("CloseButton").RegisterCallback<ClickEvent>(_ => OnUICancelled());

            _visualElementInitializer.InitializeVisualElement(box);
            _root = box;
            
            PopulateObjects();
            _gameValueChangersFilter = GameValueChangersFilter.Create(_searchField);
            
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
            Clear();
            _panelStack.Pop(this);
            _eventBus.Post(new OnGameValueChangerBoxConfirmed());
            return true;
        }

        public void OnUICancelled()
        {
            Clear();
            _panelStack.Pop(this);
        }

        private void Clear()
        {
            _objectsContainer.Clear();
            _propertiesContainer.Clear();
            _gameValueChangersFilter.Clear();
        }

        private void PopulateObjects()
        {
            var list = new List<ObjectWrapper>();
            foreach (var groupedValueChangers in _gameValueChangerRepository.GamevalueChangers
                         .GroupBy(changer => changer.ObjectName)
                         .OrderBy(changers => changers.Key))
            {
                var child = _gameValueChangerUiPresetFactory.GetObjectWrapper(groupedValueChangers.Key, groupedValueChangers, _propertiesContainer, GameValueChangerContainer);
                list.Add(child);
                _objectsContainer.Add(child.Root);
            }

            _objectWrappers = list.ToArray();
        }

        private void ApplyFilter()
        {
            foreach (var objectWrapper in _objectWrappers)
            {
                objectWrapper.ApplyFilter(_gameValueChangersFilter);
            }
        }
    }
}
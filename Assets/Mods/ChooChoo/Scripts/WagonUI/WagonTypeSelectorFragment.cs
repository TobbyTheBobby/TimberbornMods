using System;
using System.Collections.Generic;
using System.Linq;
using ChooChoo.UIPresets;
using ChooChoo.Wagons;
using TimberApi.UIBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ChooChoo.WagonUI
{
    internal class WagonTypeSelectorFragment : IEntityPanelFragment
    {
        private static readonly string WagonTypeLocKey = "Tobbert.WagonModel.WagonModel";
        
        private readonly VisualElementLoader _visualElementLoader;
        private readonly DropdownItemsSetter _dropdownItemsSetter;
        private readonly UIBuilder _uiBuilder;
        private readonly ILoc _loc;
        
        private readonly List<VisualElement> _wagonSections = new();
        private readonly List<Dropdown> _dropdowns = new();
        private readonly int _numberOfWagons = 4;
        private VisualElement _root;
        
        private WagonManager _wagonManager;

        public WagonTypeSelectorFragment(
            VisualElementLoader visualElementLoader,
            DropdownItemsSetter dropdownItemsSetter,
            UIBuilder uiBuilder,
            ILoc loc)
        {
            _uiBuilder = uiBuilder;
            _visualElementLoader = visualElementLoader;
            _dropdownItemsSetter = dropdownItemsSetter;
            _loc = loc;
        }

        public VisualElement InitializeFragment()
        {
            _root = _uiBuilder.Create<PanelFragment>().BuildAndInitialize();
            
            for (var i = 0; i < _numberOfWagons; i++)
            {
                var fragment = _visualElementLoader.LoadVisualElement("Game/EntityPanel/PlantablePrioritizerFragment");
                var container = new VisualElement();
                foreach (var element in fragment.Children().ToList())
                {
                    container.Add(element);
                    container.Q<Label>().text = _loc.T(WagonTypeLocKey);
                }

                _root.Add(container);
                _wagonSections.Add(fragment);
                _dropdowns.Add(container.Q<Dropdown>("Priorities"));
            }

            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _wagonManager = entity.GetComponentFast<WagonManager>();
            if (!_wagonManager)
                return;
            for (var i = 0; i < _numberOfWagons; i++)
            {
                _dropdownItemsSetter.SetLocalizableItems(_dropdowns[i], _wagonManager.Wagons[i].GetComponentFast<WagonTypeDropdownProvider>());
                _wagonSections[i].ToggleDisplayStyle(true);
            }

            // _trainWagonManager.WagonTypesChanged += OnWagonTypeChanged;
        }

        public void ClearFragment()
        {
            // if (_trainWagonManager)
            //   _trainWagonManager.WagonTypesChanged -= OnWagonTypeChanged;
            for (var i = 0; i < _numberOfWagons; i++)
            {
                _wagonSections[i].ToggleDisplayStyle(false);
                _dropdowns[i].ClearItems();
            }

            _wagonManager = null;
            UpdateFragment();
        }

        public void UpdateFragment() => _root.ToggleDisplayStyle(_wagonManager);

        private void OnWagonTypeChanged(object sender, EventArgs e)
        {
            _dropdowns.ForEach(dropdown => dropdown.RefreshContent());
        }
    }
}
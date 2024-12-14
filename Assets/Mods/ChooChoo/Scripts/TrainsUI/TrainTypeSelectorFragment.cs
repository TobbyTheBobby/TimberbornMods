using System.Linq;
using ChooChoo.UIPresets;
using TimberApi.UIBuilderSystem;
using Timberborn.BaseComponentSystem;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine.UIElements;

namespace ChooChoo.TrainsUI
{
    internal class TrainTypeSelectorFragment : IEntityPanelFragment
    {
        private readonly VisualElementLoader _visualElementLoader;
        private readonly DropdownItemsSetter _dropdownItemsSetter;
        private readonly UIBuilder _uiBuilder;
        private readonly ILoc _loc;
        private TrainTypeDropdownProvider _trainTypeDropdownProvider;
        private Dropdown _dropdown;
        private VisualElement _root;

        public TrainTypeSelectorFragment(
            VisualElementLoader visualElementLoader,
            DropdownItemsSetter dropdownItemsSetter,
            UIBuilder uiBuilder,
            ILoc loc)
        {
            _visualElementLoader = visualElementLoader;
            _dropdownItemsSetter = dropdownItemsSetter;
            _uiBuilder = uiBuilder;
            _loc = loc;
        }

        public VisualElement InitializeFragment()
        {
            _root = _uiBuilder.Create<PanelFragment>().BuildAndInitialize();

            var fragment = _visualElementLoader.LoadVisualElement("Game/EntityPanel/PlantablePrioritizerFragment");
            var container = new VisualElement();
            foreach (var element in fragment.Children().ToList())
            {
                container.Add(element);
                container.Q<Label>().text = _loc.T("Tobbert.TrainModel.TrainModel");
            }

            _root.Add(container);
            _dropdown = container.Q<Dropdown>("Priorities");

            _root.ToggleDisplayStyle(false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _trainTypeDropdownProvider = entity.GetComponentFast<TrainTypeDropdownProvider>();
            if (!_trainTypeDropdownProvider)
                return;
            _dropdownItemsSetter.SetLocalizableItems(_dropdown, _trainTypeDropdownProvider);
        }

        public void ClearFragment()
        {
            _dropdown.ClearItems();
            _trainTypeDropdownProvider = null;
            UpdateFragment();
        }

        public void UpdateFragment()
        {
            _root.ToggleDisplayStyle(_trainTypeDropdownProvider);
        }
    }
}
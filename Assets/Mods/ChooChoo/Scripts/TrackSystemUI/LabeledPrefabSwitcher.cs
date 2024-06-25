using Timberborn.BaseComponentSystem;
using Timberborn.PrefabSystem;
using TobbyTools.InaccessibilityUtilitySystem;
using UnityEngine;

namespace ChooChoo.TrackSystemUI
{
    public class LabeledPrefabSwitcher : BaseComponent
    {
        [SerializeField]
        private string _displayNameLocKey;
        [SerializeField]
        private string _descriptionLocKey;
        [SerializeField]
        private string _flavorDescriptionLocKey;
        [SerializeField]
        private Sprite _image;

        [SerializeField]
        private string _alternativeDisplayNameLocKey;
        [SerializeField]
        private string _alternativeDescriptionLocKey;
        [SerializeField]
        private string _alternativeFlavorDescriptionLocKey;
        [SerializeField]
        private Sprite _alternativeImage;

        private LabeledPrefab _labeledPrefab;

        private void Awake()
        {
            _labeledPrefab = GetComponentFast<LabeledPrefab>();
        }

        public void SetOriginal()
        {
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_displayNameLocKey", _displayNameLocKey);
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_descriptionLocKey", _descriptionLocKey);
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_flavorDescriptionLocKey", _flavorDescriptionLocKey);
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_image", _image);
        }

        public void SetAlternative()
        {
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_displayNameLocKey", _alternativeDisplayNameLocKey);
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_descriptionLocKey", _alternativeDescriptionLocKey);
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_flavorDescriptionLocKey", _alternativeFlavorDescriptionLocKey);
            InaccessibilityUtilities.SetInaccessibleField(_labeledPrefab, "_image", _alternativeImage);
        }
    }
}
using Timberborn.BaseComponentSystem;
using Timberborn.EntitySystem;
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

        private LabeledEntity _labeledEntity;

        private void Awake()
        {
            _labeledEntity = GetComponentFast<LabeledEntity>();
        }

        public void SetOriginal()
        {
            _labeledEntity._labeledEntitySpec._displayNameLocKey = _displayNameLocKey;
            _labeledEntity._labeledEntitySpec._descriptionLocKey = _descriptionLocKey;
            _labeledEntity._labeledEntitySpec._flavorDescriptionLocKey = _flavorDescriptionLocKey;
            _labeledEntity._image = _image;
        }

        public void SetAlternative()
        {
            _labeledEntity._labeledEntitySpec._displayNameLocKey = _alternativeDisplayNameLocKey;
            _labeledEntity._labeledEntitySpec._descriptionLocKey = _alternativeDescriptionLocKey;
            _labeledEntity._labeledEntitySpec._flavorDescriptionLocKey = _alternativeFlavorDescriptionLocKey;
            _labeledEntity._image = _alternativeImage;
        }
    }
}
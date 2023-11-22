using Timberborn.PrefabSystem;
using UnityEngine;

namespace ChooChoo
{
    public class LabeledPrefabSwitcher : MonoBehaviour
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

        void Awake()
        {
            _labeledPrefab = GetComponent<LabeledPrefab>();
        }

        public void SetOriginal()
        { 
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_displayNameLocKey",  _displayNameLocKey);
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_descriptionLocKey",  _descriptionLocKey);
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_flavorDescriptionLocKey",  _flavorDescriptionLocKey);
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_image",  _image);
        }

        public void SetAlternative()
        {
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_displayNameLocKey",  _alternativeDisplayNameLocKey);
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_descriptionLocKey",  _alternativeDescriptionLocKey);
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_flavorDescriptionLocKey",  _alternativeFlavorDescriptionLocKey);
            ChooChooCore.SetInaccessibleField(_labeledPrefab, "_image",  _alternativeImage);
        }
    }
}
using Bindito.Core;
using Timberborn.AssetSystem;
using Timberborn.Characters;
using Timberborn.EntityPanelSystem;
using Timberborn.Localization;
using UnityEngine;

namespace ChooChoo
{
  public class TrainEntityBadge : MonoBehaviour, IModifiableEntityBadge
  {
    private const string TrainYardDisplayNameLocKey = "Tobbert.TrainYard.DisplayName";
    
    private const string AgeLocKey = "Beaver.Age";
    
    private ILoc _loc;
    
    private IResourceAssetLoader _resourceAssetLoader;
    
    // private SelectionManager _selectionManager;

    private TrainYardService _trainYardService;
    
    private Character _character;

    private Sprite _sprite;

    [Inject]
    public void InjectDependencies(
      ILoc loc,
      IResourceAssetLoader resourceAssetLoader,
      // SelectionManager selectionManager,
      TrainYardService trainYardService)
    {
      _loc = loc;
      _resourceAssetLoader = resourceAssetLoader;
      // _selectionManager = selectionManager;
      _trainYardService = trainYardService;
    }

    public int EntityBadgePriority => 1;

    public void Awake()
    {
      _character = GetComponent<Character>();
      _sprite = _resourceAssetLoader.Load<Sprite>("tobbert.choochoo/tobbert_choochoo/ToolGroupIcon");
    }

    public string GetEntityName() => "<b>" + _character.FirstName + "</b>";

    public void SetEntityName(string entityName) => _character.FirstName = entityName;

    public string GetEntitySubtitle()
    {
      return Age();
    }

    public ClickableSubtitle GetEntityClickableSubtitle()
    {
      // if (_trainYardService.CurrentTrainYard == null)
      //   return ClickableSubtitle.CreateEmpty();
      // GameObject trainYard = _trainYardService.CurrentTrainYard.gameObject;
      // return ClickableSubtitle.Create(() => _selectionManager.SelectAndFocusOn(trainYard), _loc.T(TrainYardDisplayNameLocKey));
      return ClickableSubtitle.CreateEmpty();
    }

    public Sprite GetEntityAvatar() => _sprite;
    
    
    private string Age() => _loc.T(AgeLocKey, _character.Age);
  }
}

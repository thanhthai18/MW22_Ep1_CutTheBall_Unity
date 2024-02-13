using Cysharp.Threading.Tasks;
using Runtime.AssetLoader;
using Runtime.Definition;
using Runtime.Manager.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Foundation.AssetLoaders;

namespace Runtime.UI
{
    public class SkinElement : MonoBehaviour, ISkinElement
    {
        #region Members

        [SerializeField] private SkinType _skinType;
        [SerializeField] private GameObject _outlineObj;
        [SerializeField] private Image _background;
        [SerializeField] private Button _selfButton;

        #endregion Members

        #region Properties

        public SkinType SkinType => _skinType;

        #endregion Properties

        #region Class Methods

        public void Init()
        {
            HighlightCurrentSelect();
            LoadSpriteBackgroundElementAsync().Forget();
            _selfButton.onClick.RemoveAllListeners();
            _selfButton.onClick.AddListener(Select);
        }

        public void Select()
        {
            DataManager.Local.SetPlayerSkinId(SkinType);
        }

        public void HighlightCurrentSelect()
            => _outlineObj.SetActive(IsCurrentSelected());

        public bool IsCurrentSelected()
            => _skinType == (SkinType)DataManager.Local.PlayerSkinId;

        private async UniTask LoadSpriteBackgroundElementAsync()
        {
            var sprite = await SpriteAssetsLoader.LoadAsset($"{SpriteAtlasKey.SPRITE_SKINS_MODAL_ATLAS}[{SkinType}]", this.GetCancellationTokenOnDestroy());
            if (sprite != null)
                _background.sprite = sprite;
        }

        #endregion Class Methods
    }
}
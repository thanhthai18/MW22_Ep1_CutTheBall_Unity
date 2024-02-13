using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Runtime.Definition;
using Runtime.Message;

namespace Runtime.UI
{
    public class SkinsModal : Modal<ModalData>
    {
        #region Members

        [SerializeField] private Button _backButton;
        [SerializeField] private SkinElement[] _skinElements;
        private MessageRegistry<GameStateChangedMessage> _gameStateChangedMessageRegistry;

        #endregion Members

        #region Class Methods

        public override async UniTask Initialize(ModalData modalData)
        {
            await base.Initialize(modalData);
            _gameStateChangedMessageRegistry = Messenger.Subscribe<GameStateChangedMessage>(OnGameStateChanged);
            _backButton.onClick.AddListener(OnClickBack);
            InitSkinElements();
        }

        public override UniTask Cleanup()
        {
            _gameStateChangedMessageRegistry.Dispose();
            return base.Cleanup();
        }

        private void InitSkinElements()
        {
            if (_skinElements != null)
            {
                for (int i = 0; i < _skinElements.Length; i++)
                {
                    _skinElements[i].Init();
                }
            }
        }

        private void HighlightSkinSelected()
        {
            if (_skinElements != null)
            {
                for (int i = 0; i < _skinElements.Length; i++)
                {
                    _skinElements[i].HighlightCurrentSelect();
                }
            }
        }

        private void OnGameStateChanged(GameStateChangedMessage gameStateChangedMessage)
        {
            if (gameStateChangedMessage.GameStateEventType == GameStateEventType.PlayerSkinChanged)
            {
                HighlightSkinSelected();
            }
        }

        private void OnClickBack() => Close(true);

        #endregion Class Methods
    }
}
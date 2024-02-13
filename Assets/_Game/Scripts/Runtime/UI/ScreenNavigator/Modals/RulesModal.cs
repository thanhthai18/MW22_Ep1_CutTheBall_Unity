using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Runtime.UI
{
    public class RulesModal : Modal<ModalData>
    {
        #region Members

        [SerializeField]
        private Button _backButton;

        #endregion Members

        #region Class Methods

        public override async UniTask Initialize(ModalData modalData)
        {
            await base.Initialize(modalData);
            _backButton.onClick.AddListener(OnClickBack);
        }

        private void OnClickBack() => Close(true);

        #endregion Class Methods
    }
}
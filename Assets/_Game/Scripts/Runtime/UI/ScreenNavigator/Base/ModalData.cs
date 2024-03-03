using System;

namespace Runtime.UI
{
    public class ModalData
    {
        #region Properties

        public Action OnClosedCallbackAction { get; private set; }

        #endregion Properties

        #region Class Methods

        public ModalData(Action onClosedCallbackAction)
            => OnClosedCallbackAction = onClosedCallbackAction;

        #endregion Class Methods
    }
}
using System;

namespace Runtime.UI
{
    public class QuitModalData : ModalData
    {
        #region Members

        public string content;
        public Action confirmAction;
        public Action cancelAction;

        #endregion Members

        #region Class Methods

        public QuitModalData(string content,
                             Action confirmAction = null,
                             Action cancelAction = null,
                             Action onClosedCallbackAction = null)
            : base(onClosedCallbackAction)
        {
            this.content = content;
            this.confirmAction = confirmAction;
            this.cancelAction = cancelAction;
        }

        #endregion Class Methods
    }
}
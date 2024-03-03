using System;

namespace Runtime.UI
{
    public class PopupNotificationModalData : ModalData
    {
        #region Members

        public string content;
        public OptionPopupNotification firstOption;
        public OptionPopupNotification secondOption;

        #endregion Members

        #region Class Methods

        public PopupNotificationModalData(string content,
                                          OptionPopupNotification firstOption = null,
                                          OptionPopupNotification secondOption = null,
                                          Action onClosedCallbackAction = null)
            : base(onClosedCallbackAction)
        {
            this.content = content;
            this.firstOption = firstOption;
            this.secondOption = secondOption;
        }

        #endregion Class Methods
    }
}
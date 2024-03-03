using System;

namespace Runtime.UI
{
    public class OptionPopupNotification
    {
        public string content;
        public Action callback;

        public OptionPopupNotification(string content, Action callback = null)
        {
            this.content = content;
            this.callback = callback;
        }
    }
}
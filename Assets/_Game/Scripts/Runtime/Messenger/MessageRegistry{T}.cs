using System;
using System.Collections.Generic;

namespace Runtime.Message
{
    public struct MessageRegistry<T> : IDisposable where T : struct, IMessage
    {
        #region Members

        private List<MessageHandler<T>> _messageHandlers;
        private MessageHandler<T> _messageHandler;

        #endregion Members

        #region Struct Methods

        public MessageRegistry(List<MessageHandler<T>> messageHandlers, MessageHandler<T> messageHandler)
        {
            _messageHandlers = messageHandlers;
            _messageHandler = messageHandler;
        }

        public void Dispose()
        {
            _messageHandlers.Remove(_messageHandler);
            _messageHandlers = null;
            _messageHandler = null;
        }

        #endregion Struct Methods
    }
}
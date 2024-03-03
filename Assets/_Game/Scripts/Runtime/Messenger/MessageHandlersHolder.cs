using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Runtime.Message
{
    public class MessageHandlersHolder<T> where T : struct, IMessage
    {
        #region Members

        private readonly List<MessageHandler<T>> _messageHandlers = new();

        #endregion Members

        #region Properties

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _messageHandlers.Count <= 0;
        }

        #endregion Properties

        #region Class Methods

        public MessageRegistry<T> Subscribe(MessageHandler<T> messageHandler)
        {
            _messageHandlers.Add(messageHandler);
            return new MessageRegistry<T>(_messageHandlers, messageHandler);
        }

        public void Publish(T message)
        {
            var count = _messageHandlers.Count;
            for (var i = 0; i < count; i++)
                _messageHandlers[i].Invoke(message);
        }

        #endregion Class Methods
    }
}
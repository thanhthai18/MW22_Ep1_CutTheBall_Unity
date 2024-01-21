using System.Runtime.CompilerServices;

namespace Runtime.Message
{
    public static class Messenger
    {
        #region Class Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageRegistry<TMessage> Subscribe<TMessage>(MessageHandler<TMessage> messageHandler) where TMessage : struct, IMessage
            => MessengerSingleton.Of<MessageHandlersHolder<TMessage>>().Subscribe(messageHandler);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Publish<TMessage>(TMessage message) where TMessage : struct, IMessage
            => MessengerSingleton.Of<MessageHandlersHolder<TMessage>>().Publish(message);

        #endregion Class Methods
    }
}
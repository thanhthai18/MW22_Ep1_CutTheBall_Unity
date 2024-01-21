using Runtime.Definition;

namespace Runtime.Message
{
    public readonly struct GameStateChangedMessage : IMessage
    {
        #region Members

        public readonly GameStateEventType GameStateEventType;

        #endregion Members

        #region Struct Methods

        public GameStateChangedMessage(GameStateEventType gameStateEventType)
            => GameStateEventType = gameStateEventType;

        #endregion Struct Methods
    }
}
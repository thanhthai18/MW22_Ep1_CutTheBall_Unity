using Runtime.Common.Singleton;
using Runtime.Definition;
using Runtime.Manager.Data;
using Runtime.Message;

namespace Runtime.Gameplay.Manager
{
    public class LifeManager : MonoSingleton<LifeManager>
    {
        #region Members

        protected int maxLife;

        #endregion Members

        #region Properties

        public int MaxLife => maxLife;
        public int CurrentLife { get; private set; }

        #endregion Properties

        #region API Methods

        public virtual void HandleDataLoaded()
        {
            maxLife = Constant.MAX_LIFE;
            CurrentLife = maxLife;
        }

        protected virtual void HandleBallMissed() => DecreaseLife();

        protected virtual void HandleBoomExplored() => DecreaseLife();

        protected virtual void DecreaseLife()
        {
            if (CurrentLife > 0)
            {
                CurrentLife--;
                Messenger.Publish(new GameStateChangedMessage(GameStateEventType.DecreaseLife));
                if (IsAlive())
                    Messenger.Publish(new GameStateChangedMessage(GameStateEventType.GameLost));
            }
        }

        public virtual bool IsAlive() => CurrentLife > 0;

        #endregion API Methods
    }
}
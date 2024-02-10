using UnityEngine;
using Runtime.Common.Singleton;
using Runtime.Message;
using Runtime.Definition;

namespace Runtime.Manager.Input
{
    public class InputManager : MonoSingleton<InputManager>
    {
        #region Members

    

        #endregion Members

        #region Properties


        #endregion Properties

        #region API Methods

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                Messenger.Publish(new GameStateChangedMessage(GameStateEventType.PressBackKey));
        }

        #endregion API Methods
    }
}
using UnityEngine;

namespace Runtime.Tool.FPS
{
    public class FPSImprover : MonoBehaviour
    {
        #region API Methods

        private void Awake()
            => Application.targetFrameRate = 60;

        #endregion API Methods
    }
}
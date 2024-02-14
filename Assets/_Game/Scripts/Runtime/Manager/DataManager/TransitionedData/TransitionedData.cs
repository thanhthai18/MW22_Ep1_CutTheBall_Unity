using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Manager.Data
{
    public class TransitionedData
    {
        #region Properties

        public Vector2 sizeCamera { get; set; } = Vector2.zero;

        #endregion Properties

        #region Class Methods

        public Vector2 GetCameraSize()
        {
            if (sizeCamera.Equals(default))
            {
                Camera mainCamera = Camera.main;
                float xSize = mainCamera.orthographicSize * Screen.width / Screen.height;
                float ySize = mainCamera.orthographicSize;
                sizeCamera = new Vector2(xSize, ySize);
            }

            return sizeCamera;
        }

        #endregion Class Methods
    }
}
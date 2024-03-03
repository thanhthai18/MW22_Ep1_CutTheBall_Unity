using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Config;
using Runtime.Extensions;
using Runtime.Gameplay.EntitySystem;
using Runtime.Manager.Data;
using Runtime.UI;
using Random = UnityEngine.Random;

namespace Runtime.Definition
{
    public class Constant
    {
        #region Members

        public const int TIME_SAVE_DATA = 500;
        public const string HAS_LOAD_SERVER_DATA = "has_load_server_data";
        public const string DATA_SAVED_FOLDER = "/Save/";
        public const string DATA_SAVE_PATH = "/Save/Data.dat";
        public const int TIME_OF_A_DAY_IN_SECONDS = 86400; //30; // // 600; //24 * 60 * 60; // 1 day
        public const int TIME_OF_A_WEEK_IN_SECONDS = 604800; //60; // // // 7200; //24 * 60 * 60 * 7; // 7 day
        public const float COORDINATE_AXES_OFFSET_DEGREES = 90.0f;
        public const float CIRCLE_DEGREES = 360.0f;
        public const string HIT_MATERIAL_COLOR_PROPERTY = "_Fill_Color";
        public const int HASH_NUMBER_BALL_ID = 1000;
        public const int HASH_NUMBER_BOOM_ID = 2000;
        public const int MAX_LIFE = 3;
        public const int MARGIN_VALUE_CAMERA = 3;
        public const int SPREAD_JUMP_POWER = 1;

        public static readonly DateTime JAN1St1970 = new DateTime(1970, 1, 5, 0, 0, 0, DateTimeKind.Utc);

        #endregion Members

        #region Class Methods

        public static Vector3 GetRandomStartPosition()
        {
            Vector2 sizeCamera = DataManager.Transitioned.GetCameraSize();
            float xSize = sizeCamera.x;
            float ySize = sizeCamera.y;
            float xPosition = Random.Range(-xSize, xSize);
            float yPosition = -ySize / 2.0f - MARGIN_VALUE_CAMERA;
            Vector3 randomStartPos = new Vector3(xPosition, yPosition, 0);
            return randomStartPos;
        }

        public static string GetBallId(SkinType skinType) => (HASH_NUMBER_BALL_ID + (int)skinType).ToString();
        public static string GetBallId(int skinTypeId) => (HASH_NUMBER_BALL_ID + skinTypeId).ToString();
        public static string GetBoomId() => HASH_NUMBER_BOOM_ID.ToString();

        #endregion Class Methods
    }
}
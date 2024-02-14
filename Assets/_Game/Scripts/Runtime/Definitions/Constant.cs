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

    public class VFXKey
    {
        #region Members

        public const string EXPLORE_VFX = "{0}_explore_vfx";

        #endregion Members
    }

    public class SceneName
    {
        #region Members

        public const string MAIN_MENU_SCENE_NAME = "MenuScene";
        public const string CUT_THE_BALL_SCENE_NAME = "CutTheBall";

        #endregion Members
    }

    public class ContainerKey
    {
        #region Members

        public const string SCREEN_CONTAINER_LAYER_NAME = "ScreensContainer";
        public const string MODAL_CONTAINER_LAYER_NAME = "ModalsContainer";
        public const string TOAST_CONTAINER_LAYER_NAME = "ToastsContainer";

        #endregion Members
    }

    public class ScreenId
    {
    }

    public class ModalId
    {
        #region Members

        public const string RULES = "prefab_modal_rules";
        public const string QUIT_GAME = "prefab_modal_quit_game";
        public const string SKINS = "prefab_modal_skins";

        #endregion Members
    }

    public class Layer
    {
        #region Members

        public const int BALL_LAYER = 7;
        public const int BOOM_LAYER = 9;
        public const int SPRITE_RENDERER_BUTTON_LAYER = 13;
        public const int SPLIT_LAYER = 16;
        public static int BALL_LAYER_MASK = 1 << BALL_LAYER;
        public static int BOOM_LAYER_MASK = 1 << BOOM_LAYER;
        public static int SPRITE_RENDERER_BUTTON_LAYER_MASK = 1 << SPRITE_RENDERER_BUTTON_LAYER;
        public static int SPLIT_LAYER_LAYER_MASK = 1 << SPLIT_LAYER;

        #endregion Members
    }

    public class TagName
    {
        #region Members

        public const string MAIN_HERO_TAG = "MainHero";

        #endregion Members
    }

    public class SpriteAtlasKey
    {
        #region Members

        public const string SPRITE_ICON_ATLAS = "SpriteIconAtlas";
        public const string SPRITE_SKINS_MODAL_ATLAS = "SpriteSkinsModalAtlas";

        #endregion Members
    }

    public class AddressableKey
    {
        #region Members

        public const string HERO_CONFIG_ASSET_FORMAT = "Runtime.Config.HeroConfig{0}";
        public const string SKILL_DATA_CONFIG_ASSET_FORMAT = "Runtime.Config.{0}SkillDataConfig";
        public const string HERO_IDLE_ANIMATION = "Idle_{0}";
        public const string HERO_MOVE_ANIMATION = "Move_{0}";
        public const string ENTITY_DATA_CONFIG_ASSET_FORMAT = "Runtime.Config.{0}DataConfig";

        #endregion Members
    }

    public class StatusEffectKey
    {
        #region Members

        public const string BERSERKER_EFFECT_PREFAB = "berserker_status_vfx";

        #endregion Members
    }

    public class UIPrefabKey
    {
        #region Members

        public const string HERO_TEAM_PICK = "hero_team_pick";

        #endregion Members
    }

    public class DataKey
    {
        #region Members

        public const string PLAYER_ID = "player_id";
        public const string FIRST_OPEN_FOG = "first_open_fog";

        #endregion Members
    }
}
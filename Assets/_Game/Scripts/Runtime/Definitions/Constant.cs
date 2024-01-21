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
        public const float CIRCLE_DEGREES = 360.0f;
        public const float COORDINATE_AXES_OFFSET_DEGREES = 90.0f;
        public const string HIT_MATERIAL_COLOR_PROPERTY = "_Fill_Color";
        public const int GO_NEXT_AREA_DELAY_IN_MILLISECOND = 2000;
        public const int SHOW_RESULL_DELAY_IN_MILLISECOND = 2000;
        public const int SHOW_RESUILT_DELAY_IN_MILLISECOND_LONGER = 3000;
        public const int SHOW_NEXT_TUT_DIALOG_DELAY_IN_MILLISECOND = 3000;
        public const int SAVE_DELAY_IN_MILLISECOND = 3000;
        public const int SHOW_ENEMY_WARNING_ENEMIES_COUNT = 5;
        public const int WORLD_MAP_DIALOG_DELAY_IN_MILLISECOND = 6000;
        public const float HERO_INTERACT_WITH_GATE_SQR_DISTANCE = 6.0f * 6.0f;
        public const float HERO_MAX_STUCK_IN_OBSTACLE_TIME_TO_IDLE = 0.25f;
        public const float HERO_MAX_STUCK_IN_PLACE_TIME_TO_IDLE = 1.5f;
        public const float HERO_STUCK_IN_PLACE_THRESHOLD = 0.25f;
        public const float HEROES_INTERACT_OBJECTS_DISTANCE = 1.0f;
        public const int HEROES_FORMATION_COLUMN_COUNT = 2;
        public const float HEROES_FORMATION_GROUP_SPEED_DISTANCE_THRESHOLD = 2.5f;
        public const float HEROES_FORMATION_GROUP_SPEED_MAX_MULTIPLY_TIMES = 2.5f;
        public const float HEROES_COLLIDE_DISTANCE_SQR_THRESHOLD = 1.5f * 1.5f;
        public const float HEROES_FORMATION_GROUP_MOVEMENT_BIAS_DOT_VALUE = 0.8f;
        public const float HEROES_FORMATION_MIN_DISTANCE = 1.25f;
        public const float HERO_DIED_RESPAWN_DELAY = 60.0f;
        public const float HERO_TEAM_DETECTED_RANGE = 7.0f;
        public const float ENEMY_WORLD_ACTIVATED_SQR_RANGE = 20.0f * 20.0f;
        public const float ENEMY_WORLD_DETECTED_SQR_RANGE = HERO_TEAM_DETECTED_RANGE * HERO_TEAM_DETECTED_RANGE;
        public const float ENEMY_ACTIVATED_SQR_RANGE = 30.0f * 30.0f;
        public const float ENEMY_DETECTED_SQR_RANGE = (HERO_TEAM_DETECTED_RANGE + 8.0f) * (HERO_TEAM_DETECTED_RANGE + 8.0f);
        public const float ENEMY_IMMORTAL_HP_RATE = 0.5f;
        public const float ENEMY_DIED_RESPAWN_DELAY = 60.0f;
        public const float ENEMY_REFIND_TARGET_BONUS_RANGE = 3.0f;
        public const float BOSS_DIED_RESPAWN_DELAY = 300.0f;
        public const float OBJECT_DESTROYED_RESPAWN_DELAY = 60.0f;
        public const float CHARACTER_IDLE_MIN_DURATION = 6.0f;
        public const float CHARACTER_IDLE_MAX_DURATION = 10.0f;
        public const float CHARACTER_MOVE_RANDOM_MIN_RADIUS = 2.0f;
        public const float CHARACTER_MOVE_RANDOM_MAX_RADIUS = 4.0f;
        public const string HERO_TOMB_ID = "hero_tomb";
        public const string BASE_REST_STATION_ID = "1_2_1";
        public const string DEFAULT_HERO_ID = "1001";
        public const string DEFAULT_HERO_EPIC = "1020";
        public const int MAX_HERO_LEVEL = 70;
        public const float HERO_SELECT_RADIUS_MAX = 250;
        public const float HERO_SELECT_OFFSET = 0;
        public const int VALUE_MAX_REMAINING_TREASURE_CHEST = 25;
        public const int TIME_OF_A_DAY_IN_SECONDS = 86400;//30; // // 600; //24 * 60 * 60; // 1 day
        public const int TIME_OF_A_WEEK_IN_SECONDS = 604800; //60; // // // 7200; //24 * 60 * 60 * 7; // 7 day
        public const long RESOURCE_VALUE_THRESHOLD = 1000;
        public const int BLOCK_LEVEL_DISTANCE_UPGRADE_HERO = 10;
        public const int MAX_LEVEL_HERO_FRAGMENT_COUNT = 9999;
        public const int LIMIT_REWARD_VISUAL_REWARD_DROP_DOWN = 10;
        public const float BASE_DURATION_REQUIRED_VISUAL_UNLOCK_FOG = 0.75f;
        public const float STEP_DURATION_REQUIRED_VISUAL_UNLOCK_FOG = 0.15f;
        public const float SQR_DISTANCE_ANY_MOVE_END = 0.01f;
        public const float TIME_MOVE_RESOURCE = 0.75f;
        public const float SPREAD_VALUE_RESOURCE_VISUAL = 0.2f;
        public const float TIME_MOVE_RESOURCE_VISUAL = 0.5f;
        public const float DURATION_AVERANE_REWARD_VISUAL = 0.9f;
        public const int MAX_RESOURCE_VISUAL = 10;
        public const int LESS_THAN_LEVEL_TO_PENALTY_EXP = 5;
        public const string ARROW_DOWN = "arrow_down";
        public const string SPRITE_OUTLINE_MATERIAL_NAME = "SpriteOutline";
        public const string TELEPORTATION_PORTAL = "teleportation_portal";
        public const string FOG_GATE_ACTIVE = "fog_gate_active";
        public static readonly List<BuildingType> UNLOCKED_BUIDINGS = new List<BuildingType>(){BuildingType.MainBuilding};
        public const string UNLOCKED_TELEPORTATION = "1_2";
        public const int COUNT_STACK_CHILL_TO_FREEZE = 3;
        public const float ORIGINAL_RATIO_SCALE_EXPLODE_VFX = 1;
        public const float ELITE_RATIO_SCALE_EXPLODE_VFX = 1.5f;
        public const long DEFAULT_RESOURCE_CAPACITY = 500;
        public const long BUFF_RESOURCE_CAPACITY = 250;
        public const int RATE_US_DONE = -1;
        public const int RATE_US_SKIP = 1;
        public const long TIME_TO_NEXT_PACK_ON_BASE = 240;
        public const int PVP_MAX_SLOTS_COUNT = 10;
        public const float PVP_ARENA_MATCH_TIME = 60;

        public const string SOFT_TUT_UI_TRAINING_GROUND = "soft_tut_training_ground";

        public static Color ORANGE_COLOR = new Color(235f / 255, 85f / 255, 45f / 255);
        public const int TUTORIAL_ID_8 = 8;
        public const int TUTORIAL_ID_11 = 11;
        public const int ENEMY_FIRST_NUMBER = 2;
        public const int BOSS_FIRST_NUMBER = 3;
        public const int ENEMY_DEVIDE = 1000;

        public static readonly DateTime JAN1St1970 = new DateTime(1970, 1, 5, 0, 0, 0, DateTimeKind.Utc);
        public static readonly Vector2 ELITE_SCALE = new Vector2(1.5f, 1.5f);
        public static readonly Vector2 HERO_BOSS_SCALE = new Vector2(1.4f, 1.4f);

        public const int LEVEL_DIV_DROP_EQUIPMENT = 5;
        public const int MAX_ZONE_LEVEL = 50;
        public const ResourceType RESOURCE_TYPE_REFUND_DISMANTLE_EQUIPMENT = ResourceType.Gold;
        public const int RESOURCE_ID_REFUND_DISMANTLE_EQUIPMENT = 0;
        public static List<EquipmentType> LOCK_SLOT_EQUIPMENT = new List<EquipmentType>() { EquipmentType.Ring, EquipmentType.Symbol, EquipmentType.Relic };
        public const int VALUE_GOLD_DROP_ON_GOLDEN_GET_HIT = 1;
        public const int REQUIRED_PLAYER_LEVEL_FIRST_FLASH_SALE = 5;
        public const int LIMIT_LEVEL_UP_SKILL_TREE_BY_ADS = 3;
        public const int LIMIT_ADS_UNLOCK_DAILY = 5;
        public const int LIMIT_LEVEL_UP_HERO_BY_ADS = 3;
        public const float DELAY_RECHECK_RANDOM_CHEST = 10;
        public const int UP_SCALE_CHEST = 7;
        public const string CAMP_FIRE_PREFAB_NAME = "CampFire";
        public const string KEY_SPRITE_REWARD_MILESTONE_RANKING_PVP = "chest_rank_top_{0}";
        public const int THRESHOLD_TOP_NUMBER_BEST = 3;
        public const int FAKE_MYSELF_TOP_NUMBER = 8;

        public static List<GameActionType> LIST_GAME_ACTION_TYPE_SUGGEST_GO_HOME = new List<GameActionType>()
        {
            GameActionType.ConstructBuilding,
            GameActionType.Recruit,
        };

        public static ResourceType[] LIST_RESOURCE_WITH_THRESHOLD = new ResourceType[]
        {
            ResourceType.Food,
            ResourceType.Wood,
            ResourceType.GreenCrystal,
        };

        public static List<ResourceType> LIST_RESOURCE_CAN_WATCH_ADS = new List<ResourceType>()
        {
            ResourceType.Food,
            ResourceType.Wood,
            ResourceType.GreenCrystal,
        };

        public static List<EquipmentType> EQUIPMENT_TYPE_CURRENT = new List<EquipmentType>() {
            EquipmentType.Weapon,
            EquipmentType.Offhand,
            EquipmentType.Ornament,
            EquipmentType.Helmet,
            EquipmentType.BodyArmour,
            EquipmentType.Boots,
        };


        public static List<ResourceType> LIST_DUNGEON_SCROLL_RESOURCE_TYPE = new List<ResourceType>()
        {
             ResourceType.LavaLakeScrollRare,
             ResourceType.LavaLakeScrollEpic ,
             ResourceType.LavaLakeScrollLegend ,
             ResourceType.GlacierCaveScrollRare ,
             ResourceType.GlacierCaveScrollEpic ,
             ResourceType.GlacierCaveScrollLegend ,
             ResourceType.LostCityScrollRare ,
             ResourceType.LostCityScrollEpic ,
             ResourceType.LostCityScrollLegend ,
             ResourceType.IllusionDesertScrollRare ,
             ResourceType.IllusionDesertScrollEpic ,
             ResourceType.IllusionDesertScrollLegend ,
             ResourceType.DarkCastleScrollRare ,
             ResourceType.DarkCastleScrollEpic,
             ResourceType.DarkCastleScrollLegend
        };

        public static List<UpgradePremiumCampType> LIST_UPGRADE_PRIMIUM_CAMP_TYPE = new List<UpgradePremiumCampType>()
        {
            UpgradePremiumCampType.Exp,
            UpgradePremiumCampType.RespawnTime,
            UpgradePremiumCampType.MaximumResource
        };

        #endregion Members

        #region Class Methods

        public static string GetDungeonFloorPrefabId(string dungeonId, int floorId)
        {
            if (dungeonId.Contains('.'))
            {
                int dotIndex = dungeonId.IndexOf('.');
                return $"Dungeon_Floor_{dungeonId.Substring(0, dotIndex)}.{floorId}";
            }
            else return $"Dungeon_Floor_{dungeonId}.{floorId}";
        }

        public static string GetTowerTrialChallengeFloorPrefabId(TowerTrialChallengeModeType towerTrialChallengeModeType)
            => $"Tower_Trial_Challenge_{(int)towerTrialChallengeModeType}";

        public static string GetPVPArenaPrefabId(PVPArenaEnvironmentType pvpArenaEnvironmentType)
            => $"PVP_Arena_{(int)pvpArenaEnvironmentType}";

        public static string GetSkillTreeBuffStatIcon(SkillTreeBranchType skillTreeBranchType, StatType statType)
            => $"skill_tree_{(int)skillTreeBranchType}_{statType.ToString().ToSnakeCase()}";

        public static string GetSkillTreeSecondBranchIcon(SkillTreeBranchType skillTreeBranchType, SkillTreeSystemType skillTreeSystemType)
             => $"skill_tree_{(int)skillTreeBranchType}_{skillTreeSystemType.ToString().ToSnakeCase()}";

        public static string GetObjectRelicId(string objectId)
            => $"R{objectId}";

        public static string GetHexRarityColor(int rarity)
        {
            switch (rarity)
            {
                case (int)RarityType.Common:
                    return "white";

                case (int)RarityType.Rare:
                    return "#0677E5";

                case (int)RarityType.Epic:
                    return "#D500CD";

                case (int)RarityType.Legend:
                    return "#E79400";
            }
            return "white";
        }

        public static string GetEntityExplosionEffectName(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Boss:
                    return "boss_explode_effect";

                case EntityType.Enemy:
                    return "enemy_explode_effect";

                case EntityType.ObjectCrystal:
                case EntityType.ObjectTree:
                    return "object_explode_effect";
            }
            return "";
        }

        public static Color GetCharacterHealthBarColor(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Hero:
                    return new Color(0.3160377f, 1.0f, 0.3455433f, 1.0f);

                case EntityType.Boss:
                    return new Color(0.9811321f, 0.0f, 0.212886f, 1.0f);

                case EntityType.Enemy:
                    return new Color(1.0f, 0.3607843f, 0.3176471f, 1.0f);
            }
            return Color.white;
        }

        public static bool IsVisualBoss(string characterVisualId)
            => characterVisualId.StartsWith("3");

        public static bool IsResourceTypeThreshold(ResourceType resourceType)
        {
            return resourceType == ResourceType.Food ||
                   resourceType == ResourceType.Wood ||
                   resourceType == ResourceType.GreenCrystal;
        }

        public static bool CanResourceEarnSourceAllowOverThreshold(ResourceEarnSourceType resourceEarnSourceType)
        {
            return resourceEarnSourceType == ResourceEarnSourceType.Shop ||
                   resourceEarnSourceType == ResourceEarnSourceType.TreasureChestAds ||
                   resourceEarnSourceType == ResourceEarnSourceType.Cheat ||
                   resourceEarnSourceType == ResourceEarnSourceType.CampingAds;
        }

        public static bool CanBossReceiveStatusEffect(StatusEffectType statusEffectType)
        {
            return statusEffectType != StatusEffectType.Stun &&
                   statusEffectType != StatusEffectType.KnockUp;
        }

        public static bool CanBossGoldenReceiveStatusEffect(StatusEffectType statusEffectType)
        {
            return statusEffectType != StatusEffectType.Stun &&
                   statusEffectType != StatusEffectType.KnockUp &&
                   statusEffectType != StatusEffectType.Freeze &&
                   statusEffectType != StatusEffectType.KnockUp &&
                   statusEffectType != StatusEffectType.Chill &&
                   statusEffectType != StatusEffectType.Slow;
        }

        public static bool IsAlwaysTakeOneDamageValueCharacter(string entityId)
        {
            return entityId == "3015";
        }
      
        public static DataUpdatedType GetEquipmentDataUpdateTypeByEquipmentType(EquipmentType equipmentType)
        {
            DataUpdatedType dataUpdatedType = DataUpdatedType.None;
            switch (equipmentType)
            {
                case EquipmentType.Weapon:
                    dataUpdatedType = DataUpdatedType.NewEquipmentSlot0;
                    break;
                case EquipmentType.Offhand:
                    dataUpdatedType = DataUpdatedType.NewEquipmentSlot1;
                    break;
                case EquipmentType.Ornament:
                    dataUpdatedType = DataUpdatedType.NewEquipmentSlot2;
                    break;
                case EquipmentType.Helmet:
                    dataUpdatedType = DataUpdatedType.NewEquipmentSlot3;
                    break;
                case EquipmentType.BodyArmour:
                    dataUpdatedType = DataUpdatedType.NewEquipmentSlot4;
                    break;
                case EquipmentType.Boots:
                    dataUpdatedType = DataUpdatedType.NewEquipmentSlot5;
                    break;
                default:
                    break;
            }
            return dataUpdatedType;
        }

        public static EquipmentType GetEquipmentTypeByEquipmentDataUpdateType(DataUpdatedType dataUpdatedType)
        {
            EquipmentType equipmentType = EquipmentType.Question;
            switch (dataUpdatedType)
            {
                case DataUpdatedType.NewEquipmentSlot0:
                    equipmentType = EquipmentType.Weapon;
                    break;
                case DataUpdatedType.NewEquipmentSlot1:
                    equipmentType = EquipmentType.Offhand;
                    break;
                case DataUpdatedType.NewEquipmentSlot2:
                    equipmentType = EquipmentType.Ornament;
                    break;
                case DataUpdatedType.NewEquipmentSlot3:
                    equipmentType = EquipmentType.Helmet;
                    break;
                case DataUpdatedType.NewEquipmentSlot4:
                    equipmentType = EquipmentType.BodyArmour;
                    break;
                case DataUpdatedType.NewEquipmentSlot5:
                    equipmentType = EquipmentType.Boots;
                    break;
                default:
                    break;
            }
            return equipmentType;
        }

        public static bool IsResourceTypeByDungeonScroll(ResourceType resourceType)
        {
            return LIST_DUNGEON_SCROLL_RESOURCE_TYPE.Contains(resourceType);
        }

        public static bool CanDismantleAllEquipmentRarity(RarityType rarityType)
        {
            return rarityType == RarityType.Common ||
                   rarityType == RarityType.Rare ||
                   rarityType == RarityType.Epic;
        }

        public static int GetScoreByStatType(StatType statType, StatModifyType statModify)
        {
            int score = 0;
            switch (statType)
            {
                case StatType.None:
                    break;
                case StatType.AttackDamage:
                    if (!statModify.IsPercentValue())
                    {
                        score = 100;
                    }
                    else
                    {
                        score = 300;
                    }
                    break;
                case StatType.HealthPoint:
                    if (!statModify.IsPercentValue())
                    {
                        score = 25;
                    }
                    else
                    {
                        score = 300;
                    }
                    break;
                case StatType.CritDamage:
                    score = 150;
                    break;
                case StatType.Evasion:
                case StatType.CooldownReduction:
                case StatType.CritChance:
                case StatType.AttackSpeed:
                case StatType.MoveSpeed:
                case StatType.FixedDamageReduction:
                case StatType.DamageReduction:
                case StatType.LifeSteal:
                    score = 300;
                    break;
            }
            return score;
        }

        public static StatType GetStatTypeByEquipmentType(EquipmentType equipmentType)
        {
            if (equipmentType == EquipmentType.Weapon || equipmentType == EquipmentType.Offhand || equipmentType == EquipmentType.Ornament)
            {
                return StatType.AttackDamage;
            }
            else
            {
                return StatType.HealthPoint;
            }
        }

        public static DungeonScrollCombatPowerDifficultyType GetDungeonScrollCombatPowerDifficultyType(double combatPowerRatio)
        {
            if (combatPowerRatio >= 1.1d)
                return DungeonScrollCombatPowerDifficultyType.Easy;
            if (combatPowerRatio >= 0.95d)
                return DungeonScrollCombatPowerDifficultyType.Normal;
            if (combatPowerRatio >= 0.9d)
                return DungeonScrollCombatPowerDifficultyType.Hard;
            return DungeonScrollCombatPowerDifficultyType.VeryHard;
        }

        public static bool IsEntityUseDeathClawsSkillVisualBrown(string entityId)
        {
            return entityId == "3010" || entityId == "3022";
        }

        public static bool IsEntityUseDeathClawsSkillVisualIce(string entityId)
        {
            return entityId == "3011";
        }

        public static (ResourceType, int) GetResourceTypeByMiningResourceType(MiningResourceType miningResourceType)
        {
            if (miningResourceType == MiningResourceType.Gold)
            {
                return (ResourceType.Gold, 0);
            }
            else if (miningResourceType == MiningResourceType.GreenCrystal)
            {
                return (ResourceType.GreenCrystal, 0);
            }
            else if (miningResourceType == MiningResourceType.Wood)
            {
                return (ResourceType.Wood, 0);
            }
            else
            {
                return (ResourceType.Gold, 0);
            }
        }

        public static ResourceSpendSourceType ParseMiningResourceTypeToResourceSpendType(MiningResourceType miningResourceType)
        {
            if (miningResourceType == MiningResourceType.Gold)
            {
                return ResourceSpendSourceType.GoldMine;
            }
            else if (miningResourceType == MiningResourceType.GreenCrystal)
            {
                return ResourceSpendSourceType.EmeraldMine;
            }
            else
            {
                return ResourceSpendSourceType.WoodCamp;
            }
        }

        public static ResourceEarnSourceType ParseMiningResourceTypeToResourceEarnType(MiningResourceType miningResourceType)
        {
            if (miningResourceType == MiningResourceType.Gold)
            {
                return ResourceEarnSourceType.GoldMine;
            }
            else if (miningResourceType == MiningResourceType.GreenCrystal)
            {
                return ResourceEarnSourceType.EmeraldMine;
            }
            else
            {
                return ResourceEarnSourceType.WoodCamp;
            }
        }

        public static MiningResourceType GetMiningResourceTypeByMiningGate(BuildingType buildingType)
        {
            if (buildingType == BuildingType.GoldMine)
            {
                return MiningResourceType.Gold;
            }
            else if (buildingType == BuildingType.EmeraldMine)
            {
                return MiningResourceType.GreenCrystal;
            }
            else if (buildingType == BuildingType.WoodCamp)
            {
                return MiningResourceType.Wood;
            }
            else
            {
                return MiningResourceType.Wood;
            }
        }

        #endregion Class Methods
    }

    public class PortalConstant
    {
        #region Members

        public const string PORTAL_GATE_ID = "portal_gate";
        public const float OUT_BASE_PORTAL_GATE_HEIGHT_OFFSET = 0.25f;
        public const int DELAY_BEFORE_DISAPPEAR_HEROES_IN_MILLISECONDS = 200;
        public const int DELAY_BEFORE_HEROES_TELEPORT_IN_MILLISECONDS = 500;
        public const int DELAY_BEFORE_SHOWING_HERO_APPEAR_EFFECTS_IN_MILLISECONDS = 500;
        public const int DELAY_BEFORE_HEROES_RESPAWNED_IN_BASE_IN_MILLISECONDS = 500;

        #endregion Members
    }

    public class VFXKey
    {
        #region Members

        public const string PORTAL_HERO_APPEAR_EFFECT_ID = "portal_hero_appear_effect";
        public const string PORTAL_HERO_DISAPPEAR_EFFECT_ID = "portal_hero_disappear_effect";
        public const string IMMORTAL_ENEMY_APPEAR_EFFECT_ID = "immortal_enemy_appear_effect";
        public const string SKILL_LINE_INDICATGOR  = "skill_line_indicator";
        public const string HERO_DAMAGE_VISUAL = "hero_damage_visual";
        public const string ENEMY_DAMAGE_VISUAL = "enemy_damage_visual";
        public const string OBJECT_DAMAGE_VISUAL = "object_damage_visual";
        public const string REWARD_VISUAL = "reward_visual";
        public const string REWARD_HOLD_VISUAL = "reward_hold_visual";
        public const string KILL_STEAK_BONUS_VISUAL = "kill_streak_bonus_visual";
        public const string REQUIRED_VISUAL = "required_visual";
        public const string EXP_VISUAL = "exp_visual";
        public const string BUTTON_UNLOCKED_EFFECT = "button_unlocked_effect";

        #endregion Members
    }

    public class ReviveConstant
    {
        #region Members

        public const int VALUE_REQUIRED_GEM_TO_REVIVE_WORLD = 20;
        public const int VALUE_MAX_WATCH_ADS_REVIVE_WORLD = 3;
        public const int VALUE_TIME_KEEP_REVIVE_WORLD = 5;
        public const int VALUE_REQUIRED_GEM_TO_REVIVE_DUNGEON = 99;
        public const int VALUE_TIME_KEEP_REVIVE_DUNGEON = 5;

        #endregion Members
    }

    public class SceneName
    {
        #region Members

        public const string TUTORIAL_MAP_SCENE_NAME = "TutorialMap";
        public const string CUT_THE_BALL_SCENE_NAME = "CutTheBall";
        public const string DUNGEON_MAP_SCENE_NAME = "DungeonMap";
        public const string GOLDEN_CHALLENGE_MAP_SCENE_NAME = "GoldenChallengeMap";
        public const string TOWER_TRIAL_CHALLENGE_MAP_SCENE_NAME = "TowerTrialChallengeMap";
        public const string PVP_MAP_SCENE_NAME = "PVPMap";

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
        #region Members

        public const string HERO_SELECTION = "prefab_screen_hero_selection";
        public const string MINIMAP = "prefab_screen_minimap";

        #endregion Members
    }

    public class ModalId
    {
        #region Members

        public const string CHEAT = "prefab_modal_cheat";
        public const string QUIT_GAME = "prefab_modal_quit_game";
        public const string WORLD_RESULT = "prefab_modal_world_result";
        public const string HERO_INFO = "prefab_modal_hero_info";
        public const string HERO_SKILL = "prefab_modal_hero_skill";
        public const string TREASURE_CHEST = "prefab_modal_treasure_chest";
        public const string MANAGE_HEROES = "prefab_modal_manage_heroes";
        public const string SETTING = "prefab_modal_setting";
        public const string SELECT_LANGUAGE = "prefab_modal_select_language";
        public const string REWARD_DUNGEON = "prefab_modal_reward_dungeon";
        public const string REWARD_DUNGEON_FAIL = "prefab_modal_reward_dungeon_fail";
        public const string REWARD_DUNGEON_REVIVE = "prefab_modal_revive_dungeon";
        public const string INSUFFICIENT_GEMS = "prefab_modal_insufficient_gem";
        public const string QUEST = "prefab_modal_quest";
        public const string WORLD_REVIVE = "prefab_modal_revive_world";
        public const string NOT_ENOUGH_RESOURCE_ALERT = "prefab_modal_not_enough_resource_alert";
        public const string SKILL_TREE = "prefab_modal_skill_tree";
        public const string SHOP = "prefab_modal_shop";
        public const string FLASH_SALE = "prefab_modal_flashsale";
        public const string INSUFFICIENT_RESOURCE = "prefab_modal_insufficient_resource_upgrade";
        public const string GACHA_HERO = "prefab_modal_gacha_hero";
        public const string TELEPORTATION_PORTAL = "prefab_modal_teleportation_portal";
        public const string REFRESH_GACHA = "prefab_modal_refresh_gacha";
        public const string NOTIFICATION = "prefab_modal_popup_notification";
        public const string EQUIPMENT_MANAGER = "prefab_modal_equipment_manager";
        public const string EQUIPMENT_INFO = "prefab_modal_equipment_item_info";
        public const string REWARD = "prefab_modal_reward";
        public const string CHALLENGE = "prefab_modal_challenge";
        public const string GIFT_CODE = "prefab_modal_gift_code";
        public const string GOLDEN_CHALLENGE_SELECT = "prefab_modal_golden_challenge_select";
        public const string GOLDEN_CHALLENGE_RESULT = "prefab_modal_golden_challenge_result";
        public const string TOWER_TRIAL_CHALLENGE_MODE_SELECT = "prefab_modal_tower_trial_challenge_mode_select";
        public const string TOWER_TRIAL_CHALLENGE_FLOOR_SELECT = "prefab_modal_tower_trial_challenge_floor_select";
        public const string TOWER_TRIAL_CHALLENGE_WIN_RESULT = "prefab_modal_tower_trial_challenge_win_result";
        public const string TOWER_TRIAL_CHALLENGE_LOSE_RESULT = "prefab_modal_tower_trial_challenge_lose_result";
        public const string DUNGEON_PORTAL = "prefab_modal_dungeon_portal";
        public const string DUNGEON_PORTAL_SELECT = "prefab_modal_dungeon_portal_select";
        public const string HERO_SELECTION_CHALLENGE = "prefab_modal_hero_selection_challenge";
        public const string BATTLE_PASS_MODAL = "prefab_modal_battle_pass";
        public const string BATTLE_PASS_ACTIVE_PACK_MODAL = "prefab_modal_battle_pass_active";
        public const string MINING_MANAGER = "prefab_modal_mining_manager";
        public const string GROW_PACK = "prefab_modal_grow_pack";
        public const string RATE_US_MODAL = "prefab_modal_rate_us";
        public const string PREMIUM_CAMP_INFO = "prefab_modal_premium_camp_info";
        public const string REWARD_CAMP = "prefab_modal_reward_camp";
        public const string PVP_RANKING = "prefab_modal_pvp_ranking";

        #endregion Members
    }

    public class Layer
    {
        #region Members

        public const int HERO_LAYER = 7;
        public const int OBJECT_LAYER = 9;
        public const int ENEMY_LAYER = 10;
        public const int SPRITE_RENDERER_BUTTON_LAYER = 13;
        public const int WORLD_CHUNK_BOUND_LAYER = 16;
        public static int HERO_LAYER_MASK = 1 << HERO_LAYER;
        public static int OBJECT_LAYER_MASK = 1 << OBJECT_LAYER;
        public static int ENEMY_LAYER_MASK = 1 << ENEMY_LAYER;
        public static int SPRITE_RENDERER_BUTTON_LAYER_MASK = 1 << SPRITE_RENDERER_BUTTON_LAYER;
        public static int WORLD_CHUNK_BOUND_LAYER_LAYER_MASK = 1 << WORLD_CHUNK_BOUND_LAYER;

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
        public const string SPRITE_HERO_ATLAS = "SpriteHeroAtlas";
        public const string SKILL_TREE_ATLAS = "SkillTreeAtlas";
        public const string SPRITE_ENEMY_ATLAS = "SpriteEnemyAtlas";
        public const string SPRITE_BUILDING_ATLAS = "SpriteBuildingAtlas";
        public const string SPRITE_PORTAL_ATLAS = "SpritePortalAtlas";
        public const string SPRITE_ICON_SKILL_HERO_ATLAS = "SpriteIconSkillHeroAtlas";
        public const string SPRITE_EQUIPMENT_ICON_ATLAS = "SpriteEquipmentIconAtlas";
        public const string SPRITE_DUNGEON_SCROLL_ATLAS = "SpriteDungeonScrollAtlas";
        public const string SPRITE_DUNGEON_SCROLL_ATLAS_DIFFICULTY_HINT_POSTFIX = "difficulty_";
        public const string SPRITE_WORLD_MAP_ATLAS = "SpriteWorldMapAtlas";
        public const string SPRITE_CHALLENGE_ATLAS = "SpriteChallengeAtlas";
        public const string SPRITE_BUILDING_ITEM_ATLAS = "SpriteBuildingItemAtlas";
        public const string SPRITE_PVP_ATLAS = "SpritePvpAtlas";

        #endregion Members
    }

    public class AddressableKey
    {
        #region Members

        public const string HERO_CONFIG_ASSET_FORMAT = "Runtime.Config.HeroConfig{0}";
        public const string SKILL_DATA_CONFIG_ASSET_FORMAT = "Runtime.Config.{0}SkillDataConfig";
        public const string HERO_IDLE_ANIMATION = "Idle_{0}";
        public const string HERO_MOVE_ANIMATION = "Move_{0}";

        #endregion Members
    }

    public class StatusEffectKey
    {
        #region Members

        public const string DASH_EFFECT_PREFAB = "dash_vfx";
        public const string STUN_EFFECT_PREFAB = "stun_status_vfx";
        public const string ROOT_EFFECT_PREFAB = "root_status_vfx";
        public const string HEAL_EFFECT_PREFAB = "heal_status_vfx";
        public const string BLEED_EFFECT_PREFAB = "bleed_status_vfx";
        public const string REVIVE_EFFECT_PREFAB = "revive_vfx";
        public const string DISAPPEAR_PREFAB = "disappear_hole";
        public const string APPEAR_PREFAB = "appear_hole";
        public const string ATTACK_FORWARD_PREFAB = "attack_forward";
        public const string DASH_FORWARD_PREFAB = "dash_attack_vfx";
        public const string PERSEVERANCE_EFFECT_PREFAB = "perseverance_status_vfx";
        public const string HASTE_EFFECT_PREFAB = "haste_status_vfx";
        public const string DREAD_EFFECT_PREFAB = "dread_status_vfx";
        public const string HARDENED_EFFECT_PREFAB = "hardened_status_vfx";
        public const string QUICK_EFFECT_PREFAB = "quick_status_vfx";
        public const string FREEZE_EFFECT_PREFAB = "freeze_status_vfx";
        public const string POISON_EFFECT_PREFAB = "poison_status_vfx";
        public const string CRIT_CHANCE_BUFF_EFFECT_PREFAB = "crit_chance_buff_status_vfx";
        public const string EVASION_BUFF_EFFECT_PREFAB = "evasion_buff_status_vfx";
        public const string ATTACK_BUFF_EFFECT_PREFAB = "attack_buff_status_vfx";
        public const string BURN_EFFECT_PREFAB = "burn_status_vfx";
        public const string CHILL_EFFECT_PREFAB = "chill_status_vfx";
        public const string DAMAGE_REDUCTION_EFFECT_PREFAB = "damage_reduction_status_vfx";
        public const string NEGATIVE_REMOVE_EFFECT_PREFAB = "negative_remove_status_vfx";
        public const string SLOW_EFFECT_PREFAB = "slow_status_vfx";
        public const string HEAL_LOOP_EFFECT_PREFAB = "heal_loop_status_vfx";
        public const string TAUNT_EFFECT_PREFAB = "taunt_status_vfx";
        public const string BERSERKER_EFFECT_PREFAB = "berserker_status_vfx";

        #endregion Members

        #region Class Methods

        public static string GetStatusEffectPrefabName(StatusEffectType statusEffectType)
        {
            switch (statusEffectType)
            {
                case StatusEffectType.Stun:
                    return STUN_EFFECT_PREFAB;
                case StatusEffectType.BleedAttack:
                    return BLEED_EFFECT_PREFAB;
                case StatusEffectType.Regen:
                    return PERSEVERANCE_EFFECT_PREFAB;
                case StatusEffectType.Haste:
                    return HASTE_EFFECT_PREFAB;
                case StatusEffectType.Dread:
                    return DREAD_EFFECT_PREFAB;
                case StatusEffectType.Hardened:
                    return HARDENED_EFFECT_PREFAB;
                case StatusEffectType.Quick:
                    return QUICK_EFFECT_PREFAB;
                case StatusEffectType.Freeze:
                    return FREEZE_EFFECT_PREFAB;
                case StatusEffectType.Root:
                    return ROOT_EFFECT_PREFAB;
                case StatusEffectType.HealAttack:
                case StatusEffectType.Heal:
                    return HEAL_EFFECT_PREFAB;
                case StatusEffectType.PoisonAttack:
                    return POISON_EFFECT_PREFAB;
                case StatusEffectType.CritChanceBuff:
                    return CRIT_CHANCE_BUFF_EFFECT_PREFAB;
                case StatusEffectType.EvasionBuff:
                    return EVASION_BUFF_EFFECT_PREFAB;
                case StatusEffectType.AttackBuff:
                    return ATTACK_BUFF_EFFECT_PREFAB;
                case StatusEffectType.BurnAttack:
                    return BURN_EFFECT_PREFAB;
                case StatusEffectType.Chill:
                    return CHILL_EFFECT_PREFAB;
                case StatusEffectType.DamageReductionBuff:
                case StatusEffectType.FixedDamageReductionBuff:
                case StatusEffectType.DamageReductionNonDuration:
                case StatusEffectType.FixedDamageReductionNonDuration:
                    return DAMAGE_REDUCTION_EFFECT_PREFAB;
                case StatusEffectType.NegativeStatusEffectRemove:
                    return NEGATIVE_REMOVE_EFFECT_PREFAB;
                case StatusEffectType.Slow:
                    return SLOW_EFFECT_PREFAB;
                case StatusEffectType.HealingAttackDuration:
                    return HEAL_LOOP_EFFECT_PREFAB;
                case StatusEffectType.Taunt:
                    return TAUNT_EFFECT_PREFAB;
                case StatusEffectType.Berserker:
                    return BERSERKER_EFFECT_PREFAB;
                default:
                    return null;
            }
        }

        #endregion Class Methods
    }

    public class UIPrefabKey
    {
        #region Members

        public const string HERO_TEAM_PICK = "hero_team_pick";
        public const string HERO_UI = "hero_ui";
        public const string LEVEL_UP = "level_up_ui";
        public const string UPDATE_VERSION = "prefab_popup_update_version";

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
using Runtime.Definition;
using Runtime.Extensions;

namespace Runtime.Localization
{
    public class LocalizeKeys
    {
        #region Members

        /*----------------Select hero-----------------*/

        public const string SELECT_HERO_KEY = "select_heroes";
        public const string SELECT_LEADER_KEY = "select_your_leader";

        /*----------------Quest-----------------*/

        public const string STORY_QUEST_COMPLETE = "story_quest_complete";
        public const string STORY_QUEST_COMPLETE_CONTENT = "story_quest_complete_content";
        public const string STORY_QUEST_CLAIM_REWARD = "story_quest_claim_reward";
        public const string SUGGEST_FIND_CHEST = "suggest_find_chest";

        /*----------------Stats-----------------*/

        public const string SKILL_TREE_UNLOCK = "skill_tree_unlock";
        public const string SKILL_TREE_LOCKED = "skill_tree_locked";
        public const string CP = "cp";
        public const string ATTACK_DAMAGE = "attack_damage";
        public const string HEALTH_POINT = "health_point";

        /*----------------Popup-----------------*/
        public const string NOT_ENOUGH_RESOURCE_DESCRIPTION_IN_SPECIFIC = "not_enough_resource_description_in_specific";
        public const string NOT_ENOUGH_RESOURCE_DESCRIPTION = "not_enough_resource_description";
        public const string BEGINNER_VILAGE = "beginner_vilage";

        /*----------------Toast-----------------*/
        public const string PLEASE_UNLOCK_GACHA_BUIDING = "please_unlock_gacha_building";
        public const string RECRUIT_ONE_TO_UNLOCK = "recruit_to_unlock";
        public const string CALL_SKILL_ADS = "call_skip_ads";
        public const string REQUIRE_LOGIN_GOOGLE = "require_login_google";
        public const string LOGIN_GOOGLE_FAILED = "login_google_failed";
        public const string LOGIN_GOOGLE_SUCCESS = "login_google_success";
        public const string LOGIN_APPLE_SUCCESS = "login_apple_success";
        public const string LOGIN_GUEST_SUCCESS = "login_guest_success";
        public const string LOGIN_SUCCESS = "login_success";
        public const string NEED_TO_BUILD_TELEPORTATION = "need_build_teleport";

        public const string SAVE_DATA_SUCCESS = "save_data_success";
        public const string SAVE_DATA_FAILED = "save_data_failed";
        public const string NO_INTERNET = "no_internet";
        public const string CAN_NOT_USE_PORTAL_IN_BASE = "can_not_use_portal_in_base";
        public const string INPUT_GIFT_CODE = "input_gift_code";
        public const string GIFT_CODE_ERROR = "gift_code_error";
        public const string GIFT_CODE_USED = "gift_code_already_used";
        public const string GIFT_CODE_EXPIRE = "gift_code_expire";
        public const string GIFT_CODE_NOT_FOUND = "gift_code_not_found";
        public const string GIFT_CODE_SUCCESS = "gift_code_success";
        public const string GIFT_CODE_REWARD_EMPTY = "gift_code_reward_empty";
        public const string FEATURE_INVALID = "feature_invalid";
        public const string BUY_WITH_GEM = "buy_with_gem";
        public const string NOT_ENOUGH_LEVEL = "not_enough_level";
        public const string CLAIMED_BEFORE = "claimed_before";
        public const string PROCESSING_WARNING = "processing_warning";
        public const string SHOW_CAPACITY = "show_capacity";
        public const string REACH_LEVEL_FORMAT = "reach_level_format";
        public const string CLAIM_MISSING_TREASURE_SUCCESS = "claim_miss_treasure_success";
        public const string UNLOCK_PREVIOUS_STEP = "required_previous_step";
        public const string CANT_BACK_THIS_TIME = "cant_back";

        /*----------------UpgradeHero-----------------*/
        public const string INSUFFICIENT_REQUIRED_LEVEL = "insufficient_required_lv";
        public const string INSUFFICIENT_FRAGMENTS = "insufficient_fragments";
        public const string OWNED_EFFECTS = "owned_effects";
        public const string UNLOCKED_AT_LEVEL = "unlocked_at_level";
        public const string ALL_HERO = "all_hero";

        /*----------------General-----------------*/
        public const string LEVEL = "level";
        public const string UPGRADE = "upgrade";
        public const string LEVEL_SHORT ="level_short";
        public const string CLAIM = "claim";
        public const string VIEW = "view";
        public const string COMPLETED = "completed";
        public const string INSUFFICIENT ="insufficient";
        public const string PURCHASE_MORE ="purchase_more";
        public const string MAX = "max";
        public const string MISS = "miss_damage";
        public const string COMING_SOON = "coming_soon";
        public const string SEARCH_TREE = "search_tree_nearby";
        public const string HUNT_MONSTER = "hunt_monster";
        public const string GOT_IT = "got_it";
        public const string NOTIFICATION = "notification";
        public const string TRY_AGAIN = "try_again";
        public const string CANCEL = "cancel";
        public const string REQUIRED_LEVEL_FORMAT = "required_level_format";
        public const string ENEMY_KEY = "enemy";
        public const string BOSS_KEY = "boss";
        public const string CONFIRM = "confirm";
        public const string WARNING_LOAD_DATA = "warning_load_data";
        public const string ITEM = "item";
        public const string DAILY_LIMITS = "daily_limits";
        public const string INIT_IAP_RECHECK = "init_iap_recheck";
        public const string UPGRADE_SKILL_TREE_SUCCESS = "upgrade_skill_tree_success";
        public const string SUCCESS = "success";
        public const string ASK_QUIT = "ask_quit_game";

        /*----------------Shop-----------------*/
        public const string RESOURCES = "resources";
        public const string FREE = "free";
        public const string FLASH_SALE_CONTENT = "flash_sale_content";
        public const string FLASH_SALE_LEVEL_CONTENT = "flash_sale_level_content";
        public const string FLASH_SALE_NEW_HERO_CONTENT = "flash_sale_new_hero_content";
        public const string INFO_FLASH_SALE_RESOURCE = "info_flash_sale_resource";
        public const string INFO_FLASH_SALE_HERO = "info_flash_sale_hero";


        /*----------------DungeonGamePLay-----------------*/
        public const string KILL_STREAK_BONUS = "kill_streak_bonus";
        public const string KILL = "kill";
        public const string NEXT_FLOOR = "next_floor";
        public const string GIVE_UP = "give_up";
        public const string STAY = "stay";
        public const string DUNGEON_WARNING_QUIT = "warning_quit";
        public const string ENTRY_AVAILABLE = "entry_available";
        public const string NO_ENTRY_MATERIALS = "no_entry_materials";

        /*----------------Gacha-----------------*/
        public const string HERO_SWITCH_NOTI = "hero_swith_noti";
        public const string OBTAIN_ONE_OF_THE_AVAILABLE_HEROES = "obtain_one_of_the_available_heroes";
        public const string YOU_CAN_RECRUIT_AN_ADDITIONAL_HERO = "you_can_recruit_an_additional_hero";
        public const string HIGHER_TIER_HEROES_AFTER_RECRUITS = "higher_tier_heroes_after_recruits";
        public const string LEVEL_GACHA = "level_gacha";

        /*----------------Teleportation-----------------*/
        public const string UNABLE_MOVE_CURRENT_LOCATION = "unable_move_current_location";

        /*----------------Enemy--------------------------*/
        public const string KILL_ENEMY_NEARBY = "kill_enemy_nearby";

        /*----------------Equipment--------------------------*/
        public const string NO_ITEM_OWNED = "no_item_owned";
        public const string ITEM_IS_LOCKED = "item_is_locked";
        public const string DISMANTLE_SUCCES  = "dismantle_success";
        public const string ASK_LOCK_EQUIPMENT_ITEM  = "ask_lock_equipment_item";
        public const string ASK_UNLOCK_EQUIPMENT_ITEM  = "ask_unlock_equipment_item";
        public const string NOT_HAVE_ITEM_CAN_BE_DISMANTLE  = "not_have_item_can_be_dismantle";
        public const string ASK_DISMANTLE_ALL  = "ask_dismantle_all";
        public const string ASK_DISMANTLE_SINGLE  = "ask_dismantle_single";

        /*----------------TUTORIAL--------------------------*/
        public const string TUT_TRAINING_GROUND_GREEN = "tut_training_ground_green";
        public const string TUT_TRAINING_GROUND_RED = "tut_training_ground_red";
        public const string TUT_GACHA_DESCRIPTION = "tut_gacha_des";
        public const string TUT_DUNGEON_PORTAL_DESCRIPTION = "tut_dungeon_portal_des";
        public const string TUT_DUNGEON_PORTAL_SELECT_DESCRIPTION = "tut_dungeon_select_des";
        public const string TUT_CHALLENGE_TOWER_DESC = "tut_tower_desc";
        public const string TUT_CHALLENGE_TOWER_CHOOSE_DESC = "tut_tower_choose_desc";
        public const string TUT_CHALLENGE_TEAM_DESC = "tut_challenge_team_desc";
        
        public const string TUT_CHALLENGE_MIMIC_DESC = "tut_mimic_desc";
        public const string TUT_CHALLENGE_DETAIL_MIMIC_DESC = "tut_detail_mimic_desc";
        public const string TUT_EQUIPMENT_DESC = "tut_equip_desc";
        

        /*----------------CHALLENGE--------------------------*/
        public const string NONE = "none";
        public const string HAVE_NOT_DAILY_LIMITS = "have_not_daily_limits";
        public const string STAGE = "stage";

        /*----------------CAMP--------------------------*/
        public const string PREMIUM_CAMP_DESCRIPTION = "premium_camp_description";

        #endregion Members

        #region Class Methods

       

        /*-----------------Quest------------------*/

        public static string GetQuestKey(GameActionType questType)
        {
            return $"quest_{questType.ToString().ToSnakeCase()}";
        }

       
        #endregion Class Methods

    }
}
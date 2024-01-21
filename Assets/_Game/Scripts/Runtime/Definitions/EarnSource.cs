namespace Runtime.Definition
{
    public enum EarnSource
    {
        Cheat = 0,
        DefaultResource = 1,

        PlayerLevelUp = 20,

        BuildingComplete = 30,

        BuildingResourceClaim = 40,

        ItemExchange = 50,
        ItemBuy = 51,
        ItemPackBuy = 52,

        FoodEat = 60,

        EnergyBuy = 70,

        CraftCancel = 80,
        CraftClaim = 81,

        PlantClaim = 90,

        ObstacleExploit = 100,
        BasketExploit = 101,

        QuestCompleted = 110,

        DailyCheckInClaim = 120,
        DailyCheckInMileStoneClaim = 121,

        IAPProductBuy = 130,

        AnimalFeed = 140,

        CommunityReward = 150,

        GiftCodeClaim = 160,

        MerchantMilestoneClaim = 200,

        DynamiteExplode = 210,

        EventPlay = 500,
        EventReward = 501,

        MiniGameFreeTicket = 600,
        MiniGamePlay = 601,

        // ads
        AdsFlash = 1000,
        AdsDiningTable = 1010,

        AdsInShop = 1020,
        AdsFreeInShop = 1021,

        AdsResourceLack = 1030,

        AdsMiniGameTicket = 1040,
        AdsMiniGameBonus = 1041,

        AdsDungeonVictory = 1050,
        AdsDungeonRevive = 1051,
        AdsDungeonScrollFailHaveReward = 1052,

        AdsTreasureChest = 1060,

        AdsReviveWorld = 1070,

        AdsRefreshGacha = 1080,

        AdsGoldenChallengeVictory = 1090,

        AdsMapFog = 1100,
        
        MailWelcome = 2000,
        MailNewUpdate = 2001,

        SkillTree = 2100,
        
        UpgradeHero = 2200,

        GrowPack = 2300,

        PremiumCamp = 2400,

        AdsCampingReward = 2401,
    }
}
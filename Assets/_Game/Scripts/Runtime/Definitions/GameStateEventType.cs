namespace Runtime.Definition
{
    public enum GameStateEventType
    {
        DataLoaded,
        HeroSpawned,
        GameQuit,
        GameLost,
        NewDayReset,
        NewWeekReset,
        ReviveMapTriggered,
        GiveUpWorldMapTriggered,
        FlashSaleEnd,
        PlayerLevelChanged,
        RefreshGachaCompleted,
        PressBackKey,
        GameFlowStopped,
        HeroTeamPickUpdated,
        MapAreaLoaded,
        MapAreaCompleted,
        DungeonCompleted,
        DungeonPreviousFloorCompleted,
        CampStart,
        CampEnd,
        PremiumCampStart,
        PremiumCampEnd,
    }
}
namespace Runtime.Definition
{
    public enum GameStateEventType
    {
        DataLoaded,
        GameQuit,
        GameLost,
        NewDayReset,
        NewWeekReset,
        PlayerSkinChanged,
        PressBackKey,
        GameFlowStopped,
        BallSpawned,
        BoomSpawned,
        BallExplored,
        BoomExplored,
        DecreaseLife,
    }
}
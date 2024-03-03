namespace UnityScreenNavigator.Runtime.Foundation.Animation
{
    public static class AnimationUpdateDeltaTime
    {
        public static void Set(DeltaTimeType type)
        {
            UpdateDispatcher.Instance.SetDeltaTime(type);
        }
    }
}
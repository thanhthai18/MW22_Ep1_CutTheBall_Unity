namespace Runtime.Utilities
{
    public struct TimeInPart
    {
        #region Members

        public long days;
        public long hours;
        public long minutes;
        public long seconds;

        #endregion Members

        #region Struct Methods

        public TimeInPart(long totalSeconds)
        {
            days = totalSeconds / (24 * 3600);
            totalSeconds = totalSeconds % (24 * 3600);
            hours = totalSeconds / 3600;
            totalSeconds %= 3600;
            minutes = totalSeconds / 60;
            totalSeconds %= 60;
            seconds = totalSeconds;
        }

        #endregion Struct Methods
    }
}
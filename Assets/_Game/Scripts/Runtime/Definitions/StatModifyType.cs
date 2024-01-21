namespace Runtime.Definition
{
    public static class StatModifyTypeExtension
    {
        public static bool IsPercentValue(this StatModifyType statModifyType)
        {
            switch (statModifyType)
            {
                case StatModifyType.BaseBonus:
                    return false;
                case StatModifyType.BaseMultiply:
                    return true;
                default:
                    return false;
            }
        }
    }

    public enum StatModifyType
    {
        BaseBonus = 0,
        BaseMultiply = 1,
    }
}
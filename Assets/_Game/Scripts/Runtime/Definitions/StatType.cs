namespace Runtime.Definition
{
    public static class StatTypeExtensions
    {
        public static bool IsPercentValue(this StatType statType)
        {
            switch (statType)
            {
                case StatType.CritChance:
                case StatType.CritDamage:
                case StatType.DamageReduction:
                case StatType.CooldownReduction:
                case StatType.Evasion:
                case StatType.LifeSteal:
                    return true;
                default:
                    return false;
            }
        }
    }

    public enum StatType
    {
        None = 0,
        MoveSpeed = 1,
        AttackDamage = 2,
        HealthPoint = 3,
        CritChance = 4,
        CritDamage = 5,
        AttackRange = 6,
        Evasion = 8,
        FixedDamageReduction = 9,
        DamageReduction = 10,
        LifeSteal = 11,
        AttackSpeed = 12,
        CooldownReduction = 14,
        ChopDamage = 15,
        ChopSpeed = 16,
        MiningDamage = 17,
        MiningSpeed = 18,
    }
}
using System;

namespace Runtime.Definition
{
    [Flags]
    public enum SkillTargetType
    {
        None = 0,
        Self = 1 << 0,
        Hero = 1 << 1,
        Enemy = 1 << 2,
        Boss = 1 << 3,
        ObjectTree = 1 << 4,
        ObjectCrystal = 1 << 5,
    }
}
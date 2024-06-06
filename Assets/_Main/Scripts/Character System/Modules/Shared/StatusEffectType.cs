using System;

namespace Atomic.Character
{
    [Flags]
    public enum StatusEffectType
    {
        None = 0,
        Normal = 1,
        Burn = 2,
        Freeze = 4,
        Shock = 8,
        Toxic = 0x10,
        ArmorBreak = 0x20,
        Slow = 0x40,
        Stagger = 0x80,
        Invincible = 0x100,
        Rabid = 0x200,
        Bleed = 0x400,
        Fear = 0x800
    }    
}

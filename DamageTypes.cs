using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses
{
    public enum DamageTypes : byte
    {
        Melee,
        Ranged,
        Magic,
        Summon,
        Neutral = 255
    }
}

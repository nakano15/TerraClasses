using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses
{
    public struct SkillProjectile
    {
        public int ProjectilePos;
        public SkillData skillData;
        public PlayerMod Owner;

        public SkillProjectile(int ProjectilePos, SkillData data, PlayerMod Owner)
        {
            this.ProjectilePos = ProjectilePos;
            this.skillData = data;
            this.Owner = Owner;
        }
    }
}

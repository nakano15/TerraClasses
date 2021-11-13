using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Arachnomancer : ClassBase
    {
        public Arachnomancer()
        {
            Name = "Arachnomancer";
            Description = "Summon spiders to aid you in combat.";
            MaxLevel = 50;
            AddSkill(57);
            AddSkill(59);
            AddSkill(83);
            AddSkill(84);
            AddSkill(85);
            SkillColor = new Microsoft.Xna.Framework.Color(90, 13, 110);

            MaxHealthBonus = 2;
            MaxManaBonus = 4;
            PhysicalDamageBonus = 3;
            MagicalDamageBonus = 1;
            DefenseBonus = 0;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase Lore = base.CreateLoreBase();
            Lore.AddHuntObjective(Terraria.ID.NPCID.WallCreeper, 20, "");
            Lore.AddGatherItemObjective(Terraria.ID.ItemID.Cobweb, 500, "");
            Lore.AddGatherObjectObjective("Antlion Meat", Terraria.ID.NPCID.WalkingAntlion, 15, 0.8f);
            return Lore;
        }
    }
}

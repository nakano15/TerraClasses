using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Sharpshooter : ClassBase
    {
        public Sharpshooter()
        {
            Name = "Sharpshooter";
            Description = "Guns are their thing.";
            SkillColor = new Microsoft.Xna.Framework.Color(57, 71, 77);
            AddSkill(2);
            AddSkill(23);
            AddSkill(26);
            AddSkill(27);
            AddSkill(28);
            AddSkill(29);
            AddSkill(68);
            MaxLevel = 50;

            MaxHealthBonus = 2;
            MaxManaBonus = 2;
            PhysicalDamageBonus = 6;
            MagicalDamageBonus = 0;
            DefenseBonus = 0;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.FlintlockPistol, 1, "No matter what size is your target");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Harpy, 30, "\na Sharpshooter should accuratelly");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.JungleBat, 20, " hit them with their guns,");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.QueenBee, 1, "\nwhile keeping a safe distance.");
            return LoreBase;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.ClassList
{
    public class Cerberus : ClassBase
    {
        public Cerberus()
        {
            Name = "Cerberus";
            Description = "No longer guarding the gates of hell.";
            SkillColor = new Microsoft.Xna.Framework.Color(92, 23, 12);
            ClassType = ClassTypes.Aspect;
            MaxLevel = 50;
            AddSkill(22, "", 1); //Cerberus Form
            AddSkill(25); //Elemental Breath
            AddSkill(47); //Fire Breath
            AddSkill(73); //Frost Breath
            AddSkill(74); //Lightning Breath
            AddSkill(77); //Cerberus Bite
            AddSkill(78); //Paw Strike
            AddSkill(79); //Claw Slash

            MaxHealthBonus = 3;
            MaxManaBonus = 2;
            PhysicalDamageBonus = 2;
            MagicalDamageBonus = 2;
            DefenseBonus = 1;
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Hellbat, 100, "This is a long lost");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.AngryBones, 200, " knowledge, about the\n");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.Hellstone, 200, "Cerberus creatures, that used to guard");
            LoreBase.AddGatherItemObjective(Terraria.ID.ItemID.Obsidian, 150, " the underworld.");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.FireImp, 25, "\nUse this knowledge to alter your ");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.Guide, 1, "physical form, and ");
            LoreBase.AddHuntObjective(Terraria.ID.NPCID.WallofFlesh, 1, "\nturn into a hybrid of this fearsome creature.");
            return LoreBase;
        }
    }
}

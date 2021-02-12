﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ID;

namespace TerraClasses.ClassList
{
    public class Berseker : ClassBase
    {
        public Berseker()
        {
            Name = "Berseker";
            Description = "Attack, attack, attack. No tactic.";
            MaxLevel = 50;
            AddSkill(30); //Anger
            AddSkill(31); //Constitution
            AddSkill(32); //Axe Power
            AddSkill(33); //Frenzy
        }

        public override ClassUnlockLoreBase CreateLoreBase()
        {
            ClassUnlockLoreBase LoreBase = new ClassUnlockLoreBase();
            LoreBase.AddHuntObjective(NPCID.Zombie, 30, "What is this unending rage? ");
            LoreBase.AddHuntObjective(NPCID.EaterofSouls, 30, "\nYou have to make use of It");
            LoreBase.AddHuntObjective(NPCID.BloodCrawlerWall, 30, " to make your foes regret");
            LoreBase.AddHuntObjective(NPCID.AngryBones, 150, "\never crossing your path.");
            return LoreBase;
        }
    }
}

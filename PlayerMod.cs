using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
    public class PlayerMod : ModPlayer
    {
        public byte BasicTrainingLevel = 0;
        public List<ClassData> Classes = new List<ClassData>();
        public SkillSlot[] ActiveSkill = new SkillSlot[] { new SkillSlot(), new SkillSlot(), new SkillSlot(), new SkillSlot() };
        public SkillSlot CombatSkill = new SkillSlot(); //Support Skill, Healing Skill?
        private SkillData[] GetSkillsUnderEffect
        {
            get
            {
                List<SkillData> Skills = new List<SkillData>();
                for (int c = 0; c < Classes.Count; c++)
                {
                    Skills.AddRange(Classes[c].Skills.Where(x => (x.IsPassive || ((x.SkillTypes == Enum.SkillTypes.Active || x.SkillTypes == Enum.SkillTypes.Attack) && x.Active)) && x.Level > 0));
                }
                return Skills.ToArray();
            }
        }
        public ClassData GetLastClass
        {
            get
            {
                return Classes[Classes.Count - 1];
            }
        }
        public bool ActiveSkillsUnlocked { get { return BasicTrainingLevel >= 3; } }
        public bool CombatSkillsUnlocked { get { return BasicTrainingLevel >= 6; } }
        public bool ClassChangeUnlocked { get { return BasicTrainingLevel >= 10; } }
        public SkillSlot SkillBeingUsed = new SkillSlot();
        private Dictionary<int, int> SkillBuffLevelLogger = new Dictionary<int, int>();
        public float BuyValue = 1f, SellValue = 1f, ReforgeValue = 1f, HealValue = 1f;

        public override void Initialize()
        {
            AddClass(0, "");
        }

        public bool HasClass(int ClassID, string ClassModID = "")
        {
            if (ClassModID == "")
                ClassModID = mod.Name;
            return Classes.Any(x => x.ClassID == ClassID && x.ClassModID == ClassModID);
        }

        public int GetSkillBuffLevel(int BuffID)
        {
            if (!SkillBuffLevelLogger.ContainsKey(BuffID))
                return 0;
            return SkillBuffLevelLogger[BuffID];
        }

        public void UpdateSkillBuffLevel(int BuffID, int SkillLevel)
        {
            if (!SkillBuffLevelLogger.ContainsKey(BuffID))
                SkillBuffLevelLogger.Add(BuffID, SkillLevel);
            else
                SkillBuffLevelLogger[BuffID] = SkillLevel;
        }

        public SkillData GetSkillInfo(SkillSlot slot)
        {
            if (slot.ClassPosition >= 0 && slot.ClassPosition < Classes.Count)
            {
                ClassData cd = Classes[slot.ClassPosition];
                if (slot.SkillPosition >= 0 && slot.SkillPosition < cd.Skills.Count)
                {
                    return cd.Skills[slot.SkillPosition];
                }
            }
            return null;
        }

        public override void ResetEffects()
        {
            BuyValue = SellValue = ReforgeValue = HealValue = 1f;
        }

        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            price = (int)(price * HealValue);
        }
        
        public override bool Shoot(Item item, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            bool AllowSkillUsage = true;
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                if (!sd.GetBase.BeforeShooting(player, sd, item, ref type, ref damage, ref knockBack, ref position, ref speedX, ref speedY))
                    AllowSkillUsage = false;
            }
            return AllowSkillUsage;
        }

        public void AddClass(int ID, string ModID = "")
        {
            ClassData cd = new ClassData();
            cd.ChangeClass(ID, ModID);
            Classes.Add(cd);
        }

        public void AddClassExp(int Exp)
        {
            if (Exp == 0) return;
            List<ClassData> ClassesToGetExp = new List<ClassData>();
            for (int c = 0; c < Classes.Count; c++)
            {
                if (Classes[c].Level < Classes[c].MaxLevel)
                    ClassesToGetExp.Add(Classes[c]);
            }
            if (ClassesToGetExp.Count > 0)
            {
                float ExpMod = 1f / ClassesToGetExp.Count + (ClassesToGetExp.Count - 1) * 0.1f;
                Exp = (int)(Exp * ExpMod);
                if (Exp == 0)
                {
                    Exp = 1;
                }
                foreach (ClassData cd in ClassesToGetExp)
                {
                    if (cd.Level < cd.MaxLevel)
                    {
                        if (player.whoAmI == Main.myPlayer)
                        {
                            cd.Exp += Exp;
                        }
                    }
                }
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return false;
            }
        }

        public override void PostUpdateMiscEffects()
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.UpdateStatus(player, sd);
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.UpdateAnimation(player, sd);
            }
        }

        public override void SetControls()
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int s = 0; s < 4; s++)
                {
                    if (MainMod.SkillHotKeys[s].JustPressed)
                    {
                        if (ActiveSkill[s].SkillPosition > -1 && ActiveSkill[s].ClassPosition > -1)
                        {
                            Classes[ActiveSkill[s].ClassPosition].Skills[ActiveSkill[s].SkillPosition].UseSkill(player);
                        }
                    }
                }
            }
        }

        public static SkillData GetPlayerSkillData(Player player, int SkillID, string SkillModID = "")
        {
            if (SkillModID == "")
                SkillModID = "TerraClasses";
            List<ClassData> PlayerClasses = player.GetModPlayer<PlayerMod>().Classes;
            for (int c = 0; c < PlayerClasses.Count; c++)
            {
                List<SkillData> Skills = PlayerClasses[c].Skills;
                for (int s = 0; s < Skills.Count; s++)
                {
                    if (Skills[s].ID == SkillID && Skills[s].ModID == SkillModID)
                    {
                        return Skills[s];
                    }
                }
            }
            return null;
        }

        public override void PostItemCheck()
        {
            bool JustUsed = player.itemAnimation > 0 && player.itemAnimation == player.itemAnimationMax - 1;
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.UpdateItemUse(player, sd, JustUsed);
            }
        }

        public override Terraria.ModLoader.IO.TagCompound Save()
        {
            Terraria.ModLoader.IO.TagCompound tag = new Terraria.ModLoader.IO.TagCompound();
            tag.Add("ModVersion", MainMod.ModVersion);
            tag.Add("ClassCount", Classes.Count);
            for (int c = 0; c < Classes.Count; c++)
            {
                Classes[c].Save(tag, c);
            }
            SaveSkillSlot(tag, CombatSkill, "CombatSkillSlot");
            for (int s = 0; s < 4; s++)
            {
                SaveSkillSlot(tag, ActiveSkill[s], "ActiveSkillSlot_" + s);
            }
            return tag;
        }

        public override void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            if (!tag.ContainsKey("ModVersion"))
                return;
            int ModVersion = tag.GetInt("ModVersion");
            if (ModVersion < 1)
                return;
            int ClassCount = tag.GetInt("ClassCount");
            Classes.Clear();
            for (int c = 0; c < ClassCount; c++)
            {
                Classes.Add(new ClassData());
                Classes[c].Load(tag, c, ModVersion);
            }
            CombatSkill = LoadSkillSlot(tag, "CombatSkillSlot");
            for (int s = 0; s < 4; s++)
            {
                ActiveSkill[s] = LoadSkillSlot(tag, "ActiveSkillSlot_" + s);
            }
        }

        public void SaveSkillSlot(Terraria.ModLoader.IO.TagCompound tag, SkillSlot ss, string SkillSlotText)
        {
            if (ss.ClassPosition == -1)
            {
                tag.Add(SkillSlotText + "_Exists", false);
            }
            else
            {
                tag.Add(SkillSlotText + "_Exists", true);
                tag.Add(SkillSlotText + "_ClassID", Classes[ss.ClassPosition].ClassID);
                tag.Add(SkillSlotText + "_ClassModID", Classes[ss.ClassPosition].ClassModID);
                tag.Add(SkillSlotText + "_SkillID", Classes[ss.ClassPosition].Skills[ss.SkillPosition].ID);
                tag.Add(SkillSlotText + "_SkillModID", Classes[ss.ClassPosition].Skills[ss.SkillPosition].ModID);
            }
        }

        public SkillSlot LoadSkillSlot(Terraria.ModLoader.IO.TagCompound tag, string SkillSlotText)
        {
            SkillSlot ss = new SkillSlot();
            if (tag.GetBool(SkillSlotText + "_Exists"))
            {
                int ClassID = tag.GetInt(SkillSlotText + "_ClassID");
                string ClassModID = tag.GetString(SkillSlotText + "_ClassModID");
                int SkillID = tag.GetInt(SkillSlotText + "_SkillID");
                string SkillModID = tag.GetString(SkillSlotText + "_SkillModID");
                for (int c = 0; c < Classes.Count; c++)
                {
                    if (Classes[c].ClassID == ClassID && Classes[c].ClassModID == ClassModID)
                    {
                        for (int s = 0; s < Classes[c].Skills.Count; s++)
                        {
                            if (Classes[c].Skills[s].ID == SkillID && Classes[c].Skills[s].ModID == SkillModID)
                            {
                                ss.ClassPosition = c;
                                ss.SkillPosition = s;
                                break;
                            }
                        }
                    }
                }
            }
            return ss;
        }

        public void OnMobKill(NPC npc)
        {
            for (int i = 0; i < 50; i++)
            {
                if (player.inventory[i].type > 0 && player.inventory[i].modItem is Items.ClassBooks.ClassLoreBookPrefab)
                {
                    Items.ClassBooks.ClassLoreBookPrefab clbp = (Items.ClassBooks.ClassLoreBookPrefab)player.inventory[i].modItem;
                    clbp.OnMobKill(npc);
                }
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.OnHitByNPC(player, sd, npc, damage, crit);
            }
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.OnHitByProjectile(player, sd, proj, damage, crit);
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.OnHitNPC(player, sd, item, target, damage, knockback, crit);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.OnHitNPCWithProj(player, sd, proj, target, damage, knockback, crit);
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.ModifyHitNPC(player, sd, item, target, ref damage, ref knockback, ref crit);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.ModifyHitNPCWithProj(player, sd, proj, target, ref damage, ref knockback, ref crit, ref hitDirection);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.ModifyHitByNPC(player, sd, npc, ref damage, ref crit);
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.ModifyHitByProjectile(player, sd, proj, ref damage, ref crit);
            }
        }

        public override void PostUpdate()
        {
            SkillBeingUsed.ClassPosition = -1;
            SkillBeingUsed.SkillPosition = -1;
            for (int c = 0; c < Classes.Count; c++ )
            {
                ClassData cd = Classes[c];
                cd.Update(player, c);
            }
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
                sd.GetBase.Update(player, sd);
                if (sd.Active)
                    sd.Time++;
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            PlayerLayer layer = new PlayerLayer(mod.Name, "NClasses: Skill Effect Layer", delegate(PlayerDrawInfo pdi)
            {
                foreach (SkillData sd in GetSkillsUnderEffect)
                {
                    sd.GetBase.Draw(player, sd, pdi);
                }
            });
            layers.Add(layer);
        }
    }
}

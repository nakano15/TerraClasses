using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerraClasses
{
    public class PlayerMod : ModPlayer
    {
        public int BasicTrainingLevel { get { return Classes[0].Level; } }
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
        private Dictionary<int, SkillBuffInfo> SkillBuffLevelLogger = new Dictionary<int, SkillBuffInfo>();
        public float BuyValue = 1f, SellValue = 1f, ReforgeValue = 1f, HealValue = 1f;
        public int BackUpWeapon = -1;
        public bool FakeWeapon = false;
        public bool UsingPrivilegedSkill = false;
        public SkillData SkillBeingCasted = null;
        public float DifficultyLevel = 0f;

        public override void Initialize()
        {
            AddClass(0, "");
        }

        public void FakeWeaponUsage(int WeaponPosition, Vector2 Direction, int Duration = 8)
        {
            Vector2 AimDir = Direction - player.Center;
            FakeWeaponUsage(WeaponPosition, (float)Math.Atan2(AimDir.Y, AimDir.X), Duration);
            if((Direction.X < player.Center.X && player.direction > 0) ||
                (Direction.X > player.Center.X && player.direction < 0))
            {
                player.direction *= -1;
            }
        }

        public void FakeWeaponUsage(int WeaponPosition, float ItemRotation, int Duration = 8)
        {
            if (BackUpWeapon == -1)
                BackUpWeapon = player.selectedItem;
            FakeWeapon = true;
            player.selectedItem = WeaponPosition;
            if (ItemRotation < -1.570796f)
            {
                ItemRotation += 3.141593f;
            }
            if (ItemRotation > 1.570796f)
            {
                ItemRotation -= 3.141593f;
            }
            player.itemRotation = ItemRotation;
            player.itemAnimation = player.itemAnimationMax = Duration;
            Main.PlaySound(player.inventory[player.selectedItem].UseSound, player.Center);
        }

        public void SetItemOrientation()
        {
            Item item = player.inventory[player.selectedItem];
            float num = player.mount.PlayerOffsetHitbox;
            if (item.useStyle == 1)
            {
                if (item.type > -1 && Item.claw[item.type])
                {
                    if ((double)player.itemAnimation < (double)player.itemAnimationMax * 0.333)
                    {
                        float num30 = 10f;
                        player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num30) * (float)player.direction;
                        player.itemLocation.Y = player.position.Y + 26f + num;
                    }
                    else if ((double)player.itemAnimation < (double)player.itemAnimationMax * 0.666)
                    {
                        float num31 = 8f;
                        player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num31) * (float)player.direction;
                        num31 = 24f;
                        player.itemLocation.Y = player.position.Y + num31 + num;
                    }
                    else
                    {
                        float num32 = 6f;
                        player.itemLocation.X = player.position.X + (float)player.width * 0.5f - ((float)Main.itemTexture[item.type].Width * 0.5f - num32) * (float)player.direction;
                        num32 = 20f;
                        player.itemLocation.Y = player.position.Y + num32 + num;
                    }
                    player.itemRotation = ((float)player.itemAnimation / (float)player.itemAnimationMax - 0.5f) * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;
                }
                else
                {
                    if ((double)player.itemAnimation < (double)player.itemAnimationMax * 0.333)
                    {
                        float num33 = 10f;
                        if (Main.itemTexture[item.type].Width > 32)
                        {
                            num33 = 14f;
                        }
                        if (Main.itemTexture[item.type].Width >= 52)
                        {
                            num33 = 24f;
                        }
                        if (Main.itemTexture[item.type].Width >= 64)
                        {
                            num33 = 28f;
                        }
                        if (Main.itemTexture[item.type].Width >= 92)
                        {
                            num33 = 38f;
                        }
                        if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                        {
                            num33 += 8f;
                        }
                        player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num33) * (float)player.direction;
                        player.itemLocation.Y = player.position.Y + 24f + num;
                    }
                    else if ((double)player.itemAnimation < (double)player.itemAnimationMax * 0.666)
                    {
                        float num34 = 10f;
                        if (Main.itemTexture[item.type].Width > 32)
                        {
                            num34 = 18f;
                        }
                        if (Main.itemTexture[item.type].Width >= 52)
                        {
                            num34 = 24f;
                        }
                        if (Main.itemTexture[item.type].Width >= 64)
                        {
                            num34 = 28f;
                        }
                        if (Main.itemTexture[item.type].Width >= 92)
                        {
                            num34 = 38f;
                        }
                        if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                        {
                            num34 += 4f;
                        }
                        player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - num34) * (float)player.direction;
                        num34 = 10f;
                        if (Main.itemTexture[item.type].Height > 32)
                        {
                            num34 = 8f;
                        }
                        if (Main.itemTexture[item.type].Height > 52)
                        {
                            num34 = 12f;
                        }
                        if (Main.itemTexture[item.type].Height > 64)
                        {
                            num34 = 14f;
                        }
                        if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                        {
                            num34 += 4f;
                        }
                        player.itemLocation.Y = player.position.Y + num34 + num;
                    }
                    else
                    {
                        float num35 = 6f;
                        if (Main.itemTexture[item.type].Width > 32)
                        {
                            num35 = 14f;
                        }
                        if (Main.itemTexture[item.type].Width >= 48)
                        {
                            num35 = 18f;
                        }
                        if (Main.itemTexture[item.type].Width >= 52)
                        {
                            num35 = 24f;
                        }
                        if (Main.itemTexture[item.type].Width >= 64)
                        {
                            num35 = 28f;
                        }
                        if (Main.itemTexture[item.type].Width >= 92)
                        {
                            num35 = 38f;
                        }
                        if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                        {
                            num35 += 4f;
                        }
                        player.itemLocation.X = player.position.X + (float)player.width * 0.5f - ((float)Main.itemTexture[item.type].Width * 0.5f - num35) * (float)player.direction;
                        num35 = 10f;
                        if (Main.itemTexture[item.type].Height > 32)
                        {
                            num35 = 10f;
                        }
                        if (Main.itemTexture[item.type].Height > 52)
                        {
                            num35 = 12f;
                        }
                        if (Main.itemTexture[item.type].Height > 64)
                        {
                            num35 = 14f;
                        }
                        if (item.type == 2330 || item.type == 2320 || item.type == 2341)
                        {
                            num35 += 4f;
                        }
                        player.itemLocation.Y = player.position.Y + num35 + num;
                    }
                    player.itemRotation = ((float)player.itemAnimation / (float)player.itemAnimationMax - 0.5f) * (float)(-player.direction) * 3.5f - (float)player.direction * 0.3f;
                }
                if (player.gravDir == -1f)
                {
                    player.itemRotation = 0f - player.itemRotation;
                    player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
                }
            }
            else if (item.useStyle == 2)
            {
                player.itemRotation = (float)player.itemAnimation / (float)player.itemAnimationMax * (float)player.direction * 2f + -1.4f * (float)player.direction;
                if ((double)player.itemAnimation < (double)player.itemAnimationMax * 0.5)
                {
                    player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 9f - player.itemRotation * 12f * (float)player.direction) * (float)player.direction;
                    player.itemLocation.Y = player.position.Y + 38f + player.itemRotation * (float)player.direction * 4f + num;
                }
                else
                {
                    player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 9f - player.itemRotation * 16f * (float)player.direction) * (float)player.direction;
                    player.itemLocation.Y = player.position.Y + 38f + player.itemRotation * (float)player.direction + num;
                }
                if (player.gravDir == -1f)
                {
                    player.itemRotation = 0f - player.itemRotation;
                    player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
                }
            }
            else if (item.useStyle == 3)
            {
                if ((double)player.itemAnimation > (double)player.itemAnimationMax * 0.666)
                {
                    player.itemLocation.X = -1000f;
                    player.itemLocation.Y = -1000f;
                    player.itemRotation = -1.3f * (float)player.direction;
                }
                else
                {
                    player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 4f) * (float)player.direction;
                    player.itemLocation.Y = player.position.Y + 24f + num;
                    float num36 = (float)player.itemAnimation / (float)player.itemAnimationMax * (float)Main.itemTexture[item.type].Width * (float)player.direction * item.scale * 1.2f - (float)(10 * player.direction);
                    if (num36 > -4f && player.direction == -1)
                    {
                        num36 = -8f;
                    }
                    if (num36 < 4f && player.direction == 1)
                    {
                        num36 = 8f;
                    }
                    player.itemLocation.X -= num36;
                    player.itemRotation = 0.8f * (float)player.direction;
                }
                if (player.gravDir == -1f)
                {
                    player.itemRotation = 0f - player.itemRotation;
                    player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
                }
            }
            else if (item.useStyle == 4)
            {
                int num37 = 0;
                if (item.type == 3601)
                {
                    num37 = 10;
                }
                player.itemRotation = 0f;
                player.itemLocation.X = player.position.X + (float)player.width * 0.5f + ((float)Main.itemTexture[item.type].Width * 0.5f - 9f - player.itemRotation * 14f * (float)player.direction - 4f - (float)num37) * (float)player.direction;
                player.itemLocation.Y = player.position.Y + (float)Main.itemTexture[item.type].Height * 0.5f + 4f + num;
                if (player.gravDir == -1f)
                {
                    player.itemRotation = 0f - player.itemRotation;
                    player.itemLocation.Y = player.position.Y + (float)player.height + (player.position.Y - player.itemLocation.Y);
                }
            }
            else if (item.useStyle == 5)
            {
                if (item.type == 3779)
                {
                    player.itemRotation = 0f;
                    player.itemLocation.X = player.Center.X + (float)(6 * player.direction);
                    player.itemLocation.Y = player.MountedCenter.Y + 6f;
                }
                else if (Item.staff[item.type])
                {
                    float scaleFactor = 6f;
                    if (item.type == 3476)
                    {
                        scaleFactor = 14f;
                    }
                    player.itemLocation = player.MountedCenter;
                    player.itemLocation += player.itemRotation.ToRotationVector2() * scaleFactor * player.direction;
                }
                else
                {
                    player.itemLocation.X = player.position.X + (float)player.width * 0.5f - (float)Main.itemTexture[item.type].Width * 0.5f - (float)(player.direction * 2);
                    player.itemLocation.Y = player.MountedCenter.Y - (float)Main.itemTexture[item.type].Height * 0.5f;
                }
            }
        }

        public bool HasClass(int ClassID, string ClassModID = "")
        {
            if (ClassModID == "")
                ClassModID = mod.Name;
            return Classes.Any(x => x.ClassID == ClassID && x.ClassModID == ClassModID);
        }

        public static int GetPlayerSkillBuffLevel(Player player, int BuffID)
        {
            return player.GetModPlayer<PlayerMod>().GetSkillBuffLevel(BuffID);
        }

        public int GetSkillBuffLevel(int BuffID)
        {
            if (!SkillBuffLevelLogger.ContainsKey(BuffID))
                return 0;
            return SkillBuffLevelLogger[BuffID].BuffLevel;
        }

        public static SkillBuffInfo GetPlayerSkillBuffInfo(Player player, int BuffID)
        {
            return player.GetModPlayer<PlayerMod>().GetSkillBuffInfo(BuffID);
        }

        public SkillBuffInfo GetSkillBuffInfo(int BuffID)
        {
            if (!SkillBuffLevelLogger.ContainsKey(BuffID))
                return null;
            return SkillBuffLevelLogger[BuffID];
        }

        public void UpdateSkillBuffLevel(int BuffID, int SkillLevel)
        {
            if (!SkillBuffLevelLogger.ContainsKey(BuffID))
                SkillBuffLevelLogger.Add(BuffID, new SkillBuffInfo() { BuffLevel = SkillLevel });
            else
            {
                if(SkillBuffLevelLogger[BuffID].BuffLevel < SkillLevel)
                    SkillBuffLevelLogger[BuffID].BuffLevel = SkillLevel;
            }
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

        public bool ResetSkill(int ClassPosition, int SkillPosition)
        {
            if(ClassPosition >= 0 && ClassPosition < Classes.Count)
            {
                ClassData cd = Classes[ClassPosition];
                if(SkillPosition >= 0 && SkillPosition < cd.Skills.Count)
                {
                    SkillData sd = cd.Skills[SkillPosition];
                    if (cd.ResetSkill(SkillPosition))
                    {
                        if(CombatSkill.ClassPosition == ClassPosition && 
                            CombatSkill.SkillPosition == SkillPosition)
                        {
                            CombatSkill = new SkillSlot();
                        }
                        for(int i = 0; i < 4; i++)
                        {
                            if (ActiveSkill[i].ClassPosition == ClassPosition &&
                                ActiveSkill[i].SkillPosition == SkillPosition)
                                ActiveSkill[i] = new SkillSlot();
                        }
                        Main.NewText("[" + sd.Name + "] resetted successfully.");
                        return true;
                    }
                }
            }
            return false;
        }

        public bool DeleteClass(int ClassPosition)
        {
            if(ClassPosition > 0 && ClassPosition < Classes.Count)
            {
                string ClassName = Classes[ClassPosition].Name + " Lv" + Classes[ClassPosition].Level;
                Classes.RemoveAt(ClassPosition);
                if (CombatSkill.ClassPosition == ClassPosition)
                    CombatSkill = new SkillSlot();
                else if (CombatSkill.ClassPosition > ClassPosition)
                    CombatSkill.ClassPosition--;
                for(int i = 0; i < 4; i++)
                {
                    if (ActiveSkill[i].ClassPosition == ClassPosition)
                        ActiveSkill[i] = new SkillSlot();
                    else if (ActiveSkill[i].ClassPosition > ClassPosition)
                        ActiveSkill[i].ClassPosition--;
                }
                Main.NewText("["+ClassName + "] removed successfully.");
                return true;
            }
            return false;
        }

        public override void PostUpdateEquips()
        {
            int HPBonus = 0, MPBonus = 0, PDamageBonus = 0, MDamageBonus = 0, DefBonus = 0;
            foreach(ClassData cd in Classes)
            {
                ClassBase cb = cd.GetClass;
                HPBonus += cb.MaxHealthBonus;
                MPBonus += cb.MaxManaBonus;
                PDamageBonus += cb.PhysicalDamageBonus;
                MDamageBonus += cb.MagicalDamageBonus;
                DefBonus += cb.DefenseBonus;
            }
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.02f * HPBonus);
            player.statManaMax2 += (int)(player.statManaMax2 * 0.02f * MPBonus);
            player.meleeDamage += 0.02f * PDamageBonus;
            player.rangedDamage += 0.02f * PDamageBonus;
            player.magicDamage += 0.02f * MDamageBonus;
            player.minionDamage += 0.02f * MDamageBonus;
            player.statDefense += (int)(player.statDefense * 0.02f * DefBonus);
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
            float kb = 0;
            foreach (SkillProjectile skproj in SkillData.SkillProjs)
            {
                if (skproj.ProjectilePos == proj.whoAmI)
                {
                    //skproj.skillData.GetBase.SkillProjectileDamageNpc(proj, skproj.skillData, skproj.Owner, )
                    return;
                }
            }
        }

        public override bool PreItemCheck()
        {
            if (FakeWeapon)
            {
                player.itemAnimation--;
                if(player.itemAnimation <= 0)
                {
                    FakeWeapon = false;
                    if(BackUpWeapon > -1)
                    {
                        player.selectedItem = BackUpWeapon;
                        BackUpWeapon = -1;
                    }
                }
                SetItemOrientation();
                return false;
            }
            return base.PreItemCheck();
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
            UsingPrivilegedSkill = false;
            SkillBeingCasted = null;
            foreach (SkillData sd in GetSkillsUnderEffect)
            {
				sd.UpdateSkillEffect(this);
                /*if (sd.CastTime >= sd.GetBase.CastTime)
                {
                    sd.GetBase.Update(player, sd);
                }
                bool Passive = sd.IsPassive;
                if (sd.Active || Passive)
                {
                    if (!Passive && sd.GetBase.UnallowOtherSkillUsage)
                        UsingPrivilegedSkill = true;
                    if (!Passive && sd.CastTime < sd.GetBase.CastTime)
                    {
                        sd.CastTime++;
                        SkillBeingCasted = sd;
                    }
                    else
                    {
                        sd.Time++;
                    }
                }*/
            }
            GetDifficultyLevel();
        }

        public void GetDifficultyLevel()
        {
            Tile tile = Framing.GetTileSafely((int)player.Center.X / 16, (int)player.Center.Y / 16);
            DifficultyLevel = (Classes.Count - 1) * 0.3f;
            if(player.ZoneTowerNebula || player.ZoneTowerSolar || player.ZoneTowerStardust || player.ZoneTowerVortex)
            {
                DifficultyLevel += 3f;
            }
            if (tile.wall == Terraria.ID.WallID.LihzahrdBrickUnsafe)
            {
                DifficultyLevel += 2.25f;
            }
            if (player.ZoneDungeon)
            {
                DifficultyLevel += 1f;
            }
            if (player.ZoneJungle)
            {
                DifficultyLevel += 0.9f;
            }
            if (!Main.dayTime)
            {
                DifficultyLevel += 0.4f;
            }
            if (player.ZoneCorrupt || player.ZoneCrimson || player.ZoneHoly)
            {
                DifficultyLevel += 0.6f;
            }
            if (player.ZoneUnderworldHeight)
            {
                DifficultyLevel += 1.2f;
            }
            if (player.ZoneBeach)
            {
                DifficultyLevel += 0.4f;
            }
            if (Main.hardMode && (!player.ZoneUnderworldHeight || NPC.downedMechBossAny))
            {
                DifficultyLevel += 2;
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            PlayerLayer layer = new PlayerLayer(mod.Name, "NClasses: Skill Effect Layer", delegate(PlayerDrawInfo pdi)
            {
                if (SkillBeingCasted != null)
                {
                    SkillBase Base = SkillBeingCasted.GetBase;
                    float CastPercentage = (SkillBeingCasted.CastTime / Base.CastTime);
                    float Scale = Base.GetEffectRange(SkillBeingCasted);
                    Terraria.DataStructures.DrawData dd;
                    if (Scale > 0)
                    {
                        Scale /= 64;
                        Vector2 DrawPos = SkillBeingCasted.CastPosition - Main.screenPosition;
                        dd =
                        new Terraria.DataStructures.DrawData(MainMod.MagicCircle, DrawPos, null, Color.White * CastPercentage * Main.cursorAlpha, SkillBeingCasted.CastTime * 0.017f,
                        new Vector2(48, 48), Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                        dd.ignorePlayerRotation = true;
                        Main.playerDrawData.Add(dd);
                    }
                    float BarScale = 1 + CastPercentage * 62;
                    Vector2 CastBarPosition = player.Top - Main.screenPosition;
                    CastBarPosition.X = (int)(CastBarPosition.X - 32);
                    CastBarPosition.Y = (int)(CastBarPosition.Y - 12);
                    dd =
                    new Terraria.DataStructures.DrawData(MainMod.CastBar, CastBarPosition, new Rectangle(0, 0, 64, 8), Color.White);
                    dd.ignorePlayerRotation = true;
                    Main.playerDrawData.Add(dd);
                    dd =
                    new Terraria.DataStructures.DrawData(MainMod.CastBar, CastBarPosition, new Rectangle(0, 8, (int)BarScale, 8), Color.White * Main.cursorAlpha);
                    dd.ignorePlayerRotation = true;
                    Main.playerDrawData.Add(dd);
                }
                foreach (SkillData sd in GetSkillsUnderEffect)
                {
                    if(sd.CastTime >= sd.GetBase.CastTime)
                        sd.GetBase.Draw(player, sd, pdi);
                }
            });
            layers.Add(layer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraClasses
{
    public class SkillData
    {
        public static List<SkillProjectile> SkillProjs = 
            new List<SkillProjectile>();
        private SkillBase _SkillBase;
        public SkillBase GetBase
        {
            get
            {
                if (_SkillBase == null)
                    _SkillBase = MainMod.GetSkill(ID, ModID);
                return _SkillBase;
            }
        }
        public int ID = 0;
        public string ModID = "";
        public int Cooldown = 0;
        public float CastTime = 0;
        public int Level { get { if (Active && !IsPassive && SkillTypes != Enum.SkillTypes.Attack) return CastLevel; if (MainMod.DebugMode && MainMod.DebugSkills) return GetBase.MaxLevel; return RealLevel + LevelBonus; } set { RealLevel = value; } }
        private int CastLevel = 0;
        public int RealLevel = 0;
        public int LevelBonus = 0;
        public int Step = 0;
        public int Time = 0;
        public int LastTime { get { return Time > 0 ? Time - 1: 0; } }
        public bool Active = false;
        public string Name { get { return GetBase.Name; } }
        public string Description { get { return GetBase.Description; } }
        public int MaxLevel { get { return GetBase.MaxLevel; } }
        public Enum.SkillTypes SkillTypes { get { return GetBase.skillType; } }
        public bool IsPassive { get { return GetBase.skillType == Enum.SkillTypes.Passive; } }
        private Dictionary<byte, int> NpcDamageCooldown = new Dictionary<byte, int>(), 
            PlayerDamageCooldown = new Dictionary<byte, int>();
        private Dictionary<TargetTranslator.Translator, int> ExtraTargetDamageCooldown = new Dictionary<TargetTranslator.Translator, int>();
        private List<int> PlayerInteraction = new List<int>(), NpcInteraction = new List<int>();
        private List<TargetTranslator.Translator> OtherInteraction = new List<TargetTranslator.Translator>();
        private int Owner = 0;
        private static bool StepChanged = false;
        public Vector2 CastPosition = Vector2.Zero;

        public void ChangeStep(int StepNum = -1, int TimeDiscount = -1)
        {
            if (StepNum == -1)
                Step++;
            else
                Step = StepNum;
			if(TimeDiscount > -1)
				Time -= TimeDiscount;
			else
				Time = 0;
            StepChanged = true;
        }

        public void DrawSkillIcon(Vector2 Position, float OriginX = 0, float OriginY = 0, bool DrawBackground = true, bool DrawForeground = true, bool DrawEffect = true, bool DrawBorder = true, ClassBase cb = null)
        {
            GetBase.DrawSkillIcon(Position, OriginX, OriginY, DrawBackground, DrawForeground, DrawEffect, DrawBorder, this, cb);
        }

        public int GetProjectileFromWeapon(Item i, Player player, bool ConsumeAmmo = true)
        {
            int shoot = 0, damage = 0;
            float speed = 0, kb = 0;
            bool canshoot = true;
            player.PickAmmo(i, ref shoot, ref speed, ref canshoot, ref damage, ref kb, ConsumeAmmo); //tModLoader gives errors when using this...
            return shoot;
        }

        public int GetMeleeDamage(int DamageBonus, float DamagePercentage, Player player)
        {
            int dmg = 0;
            for (int i = 0; i < 10; i++)
            {
                if (player.inventory[i].type > 0 && player.inventory[i].damage > 0)
                {
                    int WDamage = (int)(player.inventory[i].damage * player.meleeDamage);
                    if (!player.inventory[i].melee)
                        WDamage -= WDamage / 4;
                    if (WDamage < 1)
                        WDamage = 1;
                    if (WDamage > dmg)
                        dmg = WDamage;
                }
            }
            return (int)((dmg + DamageBonus) * DamagePercentage);
        }

        public int GetRangedDamage(int DamageBonus, float DamagePercentage, Player player)
        {
            int dmg = 0;
            for (int i = 0; i < 10; i++)
            {
                if (player.inventory[i].type > 0 && player.inventory[i].damage > 0)
                {
                    int WDamage = (int)(player.inventory[i].damage * player.rangedDamage);
                    if (!player.inventory[i].ranged)
                        WDamage -= WDamage / 4;
                    if (WDamage < 1)
                        WDamage = 1;
                    if (WDamage > dmg)
                        dmg = WDamage;
                }
            }
            return (int)((dmg + DamageBonus) * DamagePercentage);
        }

        public int GetMagicDamage(int DamageBonus, float DamagePercentage, Player player)
        {
            int dmg = 0;
            for (int i = 0; i < 10; i++)
            {
                if (player.inventory[i].type > 0 && player.inventory[i].damage > 0)
                {
                    int WDamage = (int)(player.inventory[i].damage * player.magicDamage);
                    if (!player.inventory[i].magic)
                        WDamage -= WDamage / 4;
                    if (WDamage < 1)
                        WDamage = 1;
                    if (WDamage > dmg)
                        dmg = WDamage;
                }
            }
            return (int)((dmg + DamageBonus) * DamagePercentage);
        }

        public int GetSummonDamage(int DamageBonus, float DamagePercentage, Player player)
        {
            int dmg = 0;
            for (int i = 0; i < 10; i++)
            {
                if (player.inventory[i].type > 0 && player.inventory[i].damage > 0)
                {
                    int WDamage = (int)(player.inventory[i].damage * player.minionDamage);
                    if (!player.inventory[i].summon)
                        WDamage -= WDamage / 4;
                    if (WDamage < 1)
                        WDamage = 1;
                    if (WDamage > dmg)
                        dmg = WDamage;
                }
            }
            return (int)((dmg + DamageBonus) * DamagePercentage);
        }

        public void ApplySkillBuff(Player player, int BuffID, int BuffTime, int SkillLevel = -1)
        {
            if (SkillLevel == -1)
                SkillLevel = Level;
            player.AddBuff(BuffID, BuffTime);
            player.GetModPlayer<PlayerMod>().UpdateSkillBuffLevel(BuffID, SkillLevel);
        }

        public void UseSkill(Player player)
        {
            if (Active || Cooldown > 0)
                return;
            if (player.GetModPlayer<PlayerMod>().UsingPrivilegedSkill)
                return;
            if (MainMod.SaySkillNameOnUse && player.whoAmI == Main.myPlayer)
                player.chatOverhead.NewMessage(GetBase.Name + "!!", 300);
            Time = 0;
            Step = 0;
            CastTime = 0;
            CastLevel = Level;
            Active = true;
            Cooldown = GetBase.Cooldown;
            PlayerDamageCooldown.Clear();
            NpcDamageCooldown.Clear();
            ExtraTargetDamageCooldown.Clear();
            PlayerInteraction.Clear();
            NpcInteraction.Clear();
            OtherInteraction.Clear();
            switch (GetBase.PositionToTake)
            {
                case SkillBase.PositionToTakeOnCastEnum.Mouse:
                    CastPosition = SkillBase.GetMousePositionInTheWorld;
                    break;
                case SkillBase.PositionToTakeOnCastEnum.Player:
                    CastPosition = player.Center;
                    break;
            }
        }

        public int GetPlayerDamage(Player Caster, DamageTypes DamageType)
        {
            switch (DamageType)
            {
                case DamageTypes.Melee:
                    return GetMeleeDamage(0, 1, Caster);
                case DamageTypes.Ranged:
                    return GetRangedDamage(0, 1, Caster);
                case DamageTypes.Magic:
                    return GetMagicDamage(0, 1, Caster);
                case DamageTypes.Summon:
                    return GetSummonDamage(0, 1, Caster);
            }
            return 1;
        }

        public DamageTypes GetItemDamage(Item item)
        {
            if (item.melee)
                return DamageTypes.Melee;
            if (item.ranged)
                return DamageTypes.Ranged;
            if (item.magic)
                return DamageTypes.Magic;
            if (item.summon)
                return DamageTypes.Summon;
            return DamageTypes.Neutral;
        }

        public DamageTypes GetProjDamage(Projectile proj)
        {
            if (proj.melee)
                return DamageTypes.Melee;
            if (proj.ranged)
                return DamageTypes.Ranged;
            if (proj.magic)
                return DamageTypes.Magic;
            if (proj.minion)
                return DamageTypes.Summon;
            return DamageTypes.Neutral;
        }
        public int HurtPlayer(Player Caster, Player player, DamageTypes DamageType, float DamageMult, int DamageDirection, int Cooldown = 8, bool Critical = false, bool CountDefense = true)
        {
            if (PlayerDamageCooldown.ContainsKey((byte)player.whoAmI))
                return 0;
            if (Caster.whoAmI != Main.myPlayer)
                return 0;
            int NewDamage = (int)((GetPlayerDamage(Caster, DamageType) * (Critical ? 2 : 1) - (CountDefense ? player.statDefense / 0.5f : 0)) * DamageMult);
            if (NewDamage < 1)
                NewDamage = 1;
            player.statLife -= NewDamage;
            if (player.statLife <= 0)
                player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByPlayer(Owner), NewDamage, DamageDirection, true);
            else
            {
                if (player.stoned)
                {
                    Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                }
                else if (player.frostArmor)
                {
                    Main.PlaySound(Terraria.ID.SoundID.Item27, player.position);
                }
                else if ((player.wereWolf || player.forceWerewolf) && !player.hideWolf)
                {
                    Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 6, 1f, 0f);
                }
                else if (player.boneArmor)
                {
                    Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 2, 1f, 0f);
                }
                else if (!player.Male)
                {
                    Main.PlaySound(20, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                }
                else
                {
                    Main.PlaySound(1, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                }
            }
            CombatText.NewText(player.getRect(), Color.MediumPurple, NewDamage);
            ApplyCooldownToPlayer(player, Cooldown);
            if (Main.netMode == 1)
            {
                Netplay.SendSkillHurtPlayer(Owner, player.whoAmI, NewDamage, DamageDirection);
            }
            return NewDamage;
        }

        public int HurtPlayer(Player player, int Damage, int DamageDirection, int Cooldown = 8, bool Critical = false, bool CountDefense = true)
        {
            if (PlayerDamageCooldown.ContainsKey((byte)player.whoAmI))
                return 0;
            if(Owner != Main.myPlayer)
            {
                return 0;
            }
            int NewDamage = Damage - (CountDefense ? player.statDefense / 2 : 0);
            if (NewDamage < 1)
                NewDamage = 1;
            if (Critical)
                NewDamage *= 2;
            player.statLife -= NewDamage;
            if (player.statLife <= 0)
                player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByPlayer(Owner), NewDamage, DamageDirection, true);
            else
            {
                if (player.stoned)
                {
                    Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                }
                else if (player.frostArmor)
                {
                    Main.PlaySound(Terraria.ID.SoundID.Item27, player.position);
                }
                else if ((player.wereWolf || player.forceWerewolf) && !player.hideWolf)
                {
                    Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 6, 1f, 0f);
                }
                else if (player.boneArmor)
                {
                    Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 2, 1f, 0f);
                }
                else if (!player.Male)
                {
                    Main.PlaySound(20, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                }
                else
                {
                    Main.PlaySound(1, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                }
            }
            CombatText.NewText(player.getRect(), Color.MediumPurple, NewDamage);
            ApplyCooldownToPlayer(player, Cooldown);
            if(Main.netMode == 1)
            {
                Netplay.SendSkillHurtPlayer(Owner, player.whoAmI, NewDamage, DamageDirection);
            }
            return NewDamage;
        }

        public int HurtNpc(Player Caster, NPC npc, DamageTypes DamageType, float DamagePercentage, int DamageDirection, float Knockback, int Cooldown = 8, bool Critical = false, bool CountDefense = true)
        {
            if (NpcDamageCooldown.ContainsKey((byte)npc.whoAmI) || npc.immortal || npc.dontTakeDamage)
                return 0;
            if(Caster.whoAmI != Main.myPlayer)
            {
                return 0;
            }
            int NewDamage = (int)((GetPlayerDamage(Caster, DamageType) * (Critical ? 2 : 1) - (CountDefense ? npc.defense * 0.5f : 0)) * DamagePercentage);
            if (NewDamage < 1)
                NewDamage = 1;
            npc.life -= NewDamage;
            npc.ApplyInteraction(Owner);
            npc.checkDead();
            npc.HitEffect(DamageDirection, NewDamage);
            if (npc.life > 0)
            {
                Main.PlaySound(npc.HitSound, npc.Center);
                if (Knockback > 0 && npc.knockBackResist > 0)
                {
                    float NewKB = Knockback * npc.knockBackResist;
                    npc.velocity.X += DamageDirection * NewKB;
                    npc.velocity.Y -= (npc.noGravity ? 0.75f : 0.5f);
                }
            }
            CombatText.NewText(npc.getRect(), Color.MediumPurple, NewDamage);
            ApplyCooldownToNpc(npc, Cooldown);
            if(Main.netMode == 1)
            {
                Netplay.SendSkillHurtNPC(Caster.whoAmI, npc.whoAmI, NewDamage, Knockback, DamageDirection);
            }
            return NewDamage;
        }

        public int HurtNpc(NPC npc, int Damage, int DamageDirection, float Knockback, int Cooldown = 8, bool Critical = false, bool CountDefense = true)
        {
            if (NpcDamageCooldown.ContainsKey((byte)npc.whoAmI) || npc.immortal || npc.dontTakeDamage)
                return 0;
            if (Owner != Main.myPlayer)
            {
                return 0;
            }
            int NewDamage = Damage - (CountDefense ? npc.defense / 2 : 0);
            if (NewDamage < 1)
                NewDamage = 1;
            if (Critical)
                NewDamage *= 2;
            npc.life -= NewDamage;
            npc.ApplyInteraction(Owner);
            npc.checkDead();
            npc.HitEffect(DamageDirection, NewDamage);
            if (npc.life > 0)
            {
                Main.PlaySound(npc.HitSound, npc.Center);
                if (Knockback > 0 && npc.knockBackResist > 0)
                {
                    float NewKB = Knockback * npc.knockBackResist;
                    npc.velocity.X += DamageDirection * NewKB;
                    npc.velocity.Y -= (npc.noGravity ? 0.75f : 0.5f);
                }
            }
            CombatText.NewText(npc.getRect(), Color.MediumPurple, NewDamage);
            ApplyCooldownToNpc(npc, Cooldown);
            if (Main.netMode == 1)
            {
                Netplay.SendSkillHurtNPC(Owner, npc.whoAmI, NewDamage, Knockback, DamageDirection);
            }
            return NewDamage;
        }

        public int HurtTarget(Player Caster, TargetTranslator.Translator target, DamageTypes DamageType, float DamagePercentage, int DamageDirection, float Knockback, int Cooldown = 8, bool Critical = false, bool CountDefense = true)
        {
            foreach(TargetTranslator.Translator Keys in ExtraTargetDamageCooldown.Keys)
            {
                if (Keys == target)
                    return 0;
            }
            if (target.Immunity)
                return 0;
            int NewDamage = (int)((GetPlayerDamage(Caster, DamageType) * (Critical ? 2 : 1) - (CountDefense ? target.Defense * 0.5f : 0)) * DamagePercentage);
            if (NewDamage < 1)
                NewDamage = 1;
            target.Health -= NewDamage;
            if (target.Health <= 0)
                target.Kill(NewDamage, DamageDirection, true, " was slain...");
            else
            {
                target.PlayHurtSound();
                target.ApplyKnockback(Knockback, DamageDirection);
                target.HitEffect(NewDamage, DamageDirection);
            }
            CombatText.NewText(target.GetRectangle, Microsoft.Xna.Framework.Color.MediumPurple, NewDamage);
            ApplyCooldownToTarget(target, Cooldown);
            return NewDamage;
        }

        public int HurtTarget(TargetTranslator.Translator target, int Damage, int DamageDirection, float Knockback, int Cooldown = 8, bool Critical = false, float DefensePercentage = 1f)
        {
            foreach(TargetTranslator.Translator Keys in ExtraTargetDamageCooldown.Keys)
            {
                if (Keys == target)
                    return 0;
            }
            if (target.Immunity)
                return 0;
            int NewDamage = Damage - (DefensePercentage > 0 ? (int)((target.Defense * DefensePercentage) / 2) : 0);
            if (NewDamage < 1)
                NewDamage = 1;
            if (Critical)
                NewDamage *= 2;
            target.Health -= NewDamage;
            if (target.Health <= 0)
                target.Kill(NewDamage, DamageDirection, true, " was slain...");
            else
            {
                target.PlayHurtSound();
                target.ApplyKnockback(Knockback, DamageDirection);
                target.HitEffect(NewDamage, DamageDirection);
            }
            CombatText.NewText(target.GetRectangle, Microsoft.Xna.Framework.Color.MediumPurple, NewDamage);
            ApplyCooldownToTarget(target, Cooldown);
            return NewDamage;
        }

        /// <summary>
        /// This is actually to get possible targets from Terraria, includding also targets from other mods.
        /// </summary>
        public TargetTranslator.Translator[] GetPossibleTargets(bool Allies, bool SelfIncluded = false, Vector2 Position = default(Vector2), float Distance = 0f, int MaxTargets = 0)
        {
            List<TargetTranslator.Translator> Targets = new List<TargetTranslator.Translator>();
            Player me = Main.player[Owner];
            for(int i = 0; i < 255; i++)
            {
                if(Main.player[i].active && (SelfIncluded || i != Owner))
                {
                    bool IsAlly = !Main.player[i].hostile || (me.team != 0 && me.team == Main.player[i].team);
                    if ((Allies && IsAlly) || (!Allies && !IsAlly))
                    {
                        Targets.Add(new TargetTranslator.PlayerTarget(Main.player[i]));
                    }
                }
                if(i < 200 && Main.npc[i].active)
                {
                    bool IsAlly = (Main.npc[i].townNPC || Main.npc[i].friendly);
                    if ((Allies && IsAlly) || (!Allies && !IsAlly))
                    {
                        Targets.Add(new TargetTranslator.NpcTarget(Main.npc[i]));
                    }
                }
            }
            Targets.AddRange(MainMod.GetOtherModTargets(me, Allies));
            if (Position != default(Vector2) && Distance > 0)
            {
                for (int t = 0; t < Targets.Count; t++)
                {
                    if ((Targets[t].Center - Position).Length() >= Distance) //Doesn't seems to work correctly with infos acquired from other mods.
                    {
                        Targets.RemoveAt(t);
                    }
                }
            }
            if (MaxTargets > 0)
            {
                while (Targets.Count > MaxTargets)
                    Targets.RemoveAt(MaxTargets);
            }
            return Targets.ToArray();
        }

        public TargetTranslator.Translator[] GetPossibleTargets(bool Allies, Rectangle rectangle, bool SelfIncluded = false,int MaxTargets = 0)
        {
            List<TargetTranslator.Translator> Targets = new List<TargetTranslator.Translator>();
            Player me = Main.player[Owner];
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && (SelfIncluded || i != Owner))
                {
                    bool IsAlly = !Main.player[i].hostile || (me.team != 0 && me.team == Main.player[i].team);
                    if ((Allies && IsAlly) || (!Allies && !IsAlly))
                    {
                        Targets.Add(new TargetTranslator.PlayerTarget(Main.player[i]));
                    }
                }
                if (i < 200 && Main.npc[i].active)
                {
                    bool IsAlly = (Main.npc[i].townNPC || Main.npc[i].friendly);
                    if ((Allies && IsAlly) || (!Allies && !IsAlly))
                    {
                        Targets.Add(new TargetTranslator.NpcTarget(Main.npc[i]));
                    }
                }
            }
            Targets.AddRange(MainMod.GetOtherModTargets(me, Allies));
            for (int t = Targets.Count - 1; t >= 0; t--)
            {
                if (!Targets[t].GetRectangle.Intersects(rectangle))
                {
                    Targets.RemoveAt(t);
                }
            }
            if (MaxTargets > 0)
            {
                while (Targets.Count > MaxTargets)
                    Targets.RemoveAt(MaxTargets);
            }
            return Targets.ToArray();
        }

        public Projectile SpawnSkillProjectile(Vector2 Position,
            Vector2 Velocity, int Type, float Knockback,
            PlayerMod Owner, int Damage = 1, float Ai0 = 0, float Ai1 = 0)
        {
            int Pos = Projectile.NewProjectile(Position,
                Velocity, Type, Damage, Knockback, Owner.player.whoAmI,
                Ai0, Ai1);
            AddSkillProjectile(Pos, Owner);
            return Main.projectile[Pos];
        }

        public void AddSkillProjectile(int ProjPos, PlayerMod Owner)
        {
            for(int i = 0; i < SkillProjs.Count; i++)
            {
                if(SkillProjs[i].ProjectilePos == ProjPos)
                {
                    SkillProjs[i] = new SkillProjectile(ProjPos, this, Owner);
                    return;
                }
            }
            SkillProjs.Add(new SkillProjectile(ProjPos, this, Owner));
        }

        public static void CheckSkillProjs()
        {
            for (int i = 0; i < SkillProjs.Count; i++)
            {
                if (!Main.projectile[SkillProjs[i].ProjectilePos].active)
                {
                    SkillProjs.RemoveAt(i);
                }
            }
        }

        public void OnSkillProjectileHitPlayer(Projectile proj, SkillData data, PlayerMod Owner, PlayerMod Target, int Damage, bool Crit)
        {
            SkillBase sb = GetBase;
            int DamageValue = sb.SkillProjectileDamagePlayer(proj, data, Owner, Target, Damage, Crit);
            if (DamageValue < 1)
                DamageValue = 1;
            HurtPlayer(Target.player, DamageValue, proj.direction, 0, Crit, false);
        }

        public void OnSkillProjectileHitNpc(Projectile proj, PlayerMod Owner, NPC Target, int damage, float knockback, bool crit, int hitDirection)
        {
            SkillBase sb = GetBase;
            int DamageValue = sb.SkillProjectileDamageNpc(proj, this, Owner, Target, damage, ref knockback, ref crit, ref hitDirection);
            if (DamageValue < 1)
                DamageValue = 1;
            HurtNpc(Target, DamageValue, hitDirection, knockback, 0, crit, false);
        }

        public void ApplyPlayerInteraction(Player player)
        {
            if (!PlayerInteraction.Contains(player.whoAmI))
                PlayerInteraction.Add(player.whoAmI);
        }

        public void ApplyNpcInteraction(NPC npc)
        {
            if (!NpcInteraction.Contains(npc.whoAmI))
                NpcInteraction.Add(npc.whoAmI);
        }

        public void ApplyTargetInteraction(TargetTranslator.Translator target)
        {
            if (!OtherInteraction.Contains(target))
                OtherInteraction.Add(target);
        }

        public Player[] GetPlayersInteractedWith()
        {
            List<Player> player = new List<Player>();
            foreach (int p in PlayerInteraction)
            {
                player.Add(Main.player[p]);
            }
            return player.ToArray();
        }

        public NPC[] GetNpcsInteractedWith()
        {
            List<NPC> npc = new List<NPC>();
            foreach (int n in NpcInteraction)
            {
                npc.Add(Main.npc[n]);
            }
            return npc.ToArray();
        }

        public TargetTranslator.Translator[] GetTargetsInteractedWith()
        {
            List<TargetTranslator.Translator> targets = new List<TargetTranslator.Translator>();
            foreach(TargetTranslator.Translator t in OtherInteraction)
            {
                targets.Add(t);
            }
            return targets.ToArray();
        }

        public bool ContainsPlayerInteraction(Player player)
        {
            return PlayerInteraction.Contains(player.whoAmI);
        }

        public bool ContainsNpcInteraction(NPC npc)
        {
            return NpcInteraction.Contains(npc.whoAmI);
        }

        public bool ContainsTargetInteraction(TargetTranslator.Translator target)
        {
            foreach(TargetTranslator.Translator Targets in OtherInteraction)
            {
                if (Targets == target)
                    return true;
            }
            return false;
        }

        public void RemovePlayerInteraction(Player player)
        {
            if (PlayerInteraction.Contains(player.whoAmI))
                PlayerInteraction.Remove(player.whoAmI);
        }

        public void RemoveNpcInteraction(NPC npc)
        {
            if (NpcInteraction.Contains(npc.whoAmI))
                NpcInteraction.Remove(npc.whoAmI);
        }

        public void RemoveTargetInteraction(TargetTranslator.Translator target)
        {
            if (OtherInteraction.Contains(target))
                OtherInteraction.Remove(target);
        }

        public void ApplyCooldownToPlayer(Player player, int CooldownTime)
        {
            if (!PlayerDamageCooldown.ContainsKey((byte)player.whoAmI))
            {
                PlayerDamageCooldown.Add((byte)player.whoAmI, CooldownTime);
            }
            else
            {
                PlayerDamageCooldown[(byte)player.whoAmI] = CooldownTime;
            }
        }

        public void ApplyCooldownToNpc(NPC npc, int CooldownTime)
        {
            if (!NpcDamageCooldown.ContainsKey((byte)npc.whoAmI))
            {
                NpcDamageCooldown.Add((byte)npc.whoAmI, CooldownTime);
            }
            else
            {
                NpcDamageCooldown[(byte)npc.whoAmI] = CooldownTime;
            }
        }

        public void ApplyCooldownToTarget(TargetTranslator.Translator target, int CooldownTime)
        {
            foreach (TargetTranslator.Translator key in ExtraTargetDamageCooldown.Keys)
            {
                if (key == target)
                {
                    ExtraTargetDamageCooldown[key] = CooldownTime;
                    return;
                }
            }
            ExtraTargetDamageCooldown.Add(target, CooldownTime);
        }

        public bool ContainsPlayerCooldown(Player player)
        {
            return PlayerDamageCooldown.ContainsKey((byte)player.whoAmI);
        }

        public bool ContainsNpcCooldown(NPC npc)
        {
            return NpcDamageCooldown.ContainsKey((byte)npc.whoAmI);
        }

        public bool ContainsTargetCooldown(TargetTranslator.Translator target)
        {
            return ExtraTargetDamageCooldown.ContainsKey(target);
        }

        public void RemovePlayerCooldown(Player player)
        {
            if (PlayerDamageCooldown.ContainsKey((byte)player.whoAmI))
                PlayerDamageCooldown.Remove((byte)player.whoAmI);
        }

        public void RemoveNpcCooldown(NPC npc)
        {
            if (NpcDamageCooldown.ContainsKey((byte)npc.whoAmI))
                NpcDamageCooldown.Remove((byte)npc.whoAmI);
        }

        public void RemoveTargetCooldown(TargetTranslator.Translator target)
        {
            if (ExtraTargetDamageCooldown.ContainsKey(target))
                ExtraTargetDamageCooldown.Remove(target);
        }

        public void UpdateSkill(Player player)
        {
            Owner = player.whoAmI;
            if (GetBase.skillType != Enum.SkillTypes.Passive)
            {
                if (!Active)
                {
                    if (Cooldown > 0)
                    {
                        if (MainMod.DebugMode)
                            Cooldown = 0;
                        else
                            Cooldown--;
                    }
                    return;
                }
                if (CastTime < GetBase.CastTime)
                {
                    switch (GetBase.PositionToTake)
                    {
                        case SkillBase.PositionToTakeOnCastEnum.Mouse:
                            CastPosition = SkillBase.GetMousePositionInTheWorld;
                            break;
                        case SkillBase.PositionToTakeOnCastEnum.Player:
                            CastPosition = player.Center;
                            break;
                    }
                    return;
                }
            }
            byte[] Keys = PlayerDamageCooldown.Keys.ToArray();
            foreach (byte key in Keys)
            {
                PlayerDamageCooldown[key]--;
                if (PlayerDamageCooldown[key] <= 0)
                    PlayerDamageCooldown.Remove(key);
            }
            Keys = NpcDamageCooldown.Keys.ToArray();
            foreach (byte key in Keys)
            {
                NpcDamageCooldown[key]--;
                if (NpcDamageCooldown[key] <= 0)
                    NpcDamageCooldown.Remove(key);
            }
            TargetTranslator.Translator[] Keys2 = ExtraTargetDamageCooldown.Keys.ToArray();
            foreach (TargetTranslator.Translator target in Keys2)
            {
                ExtraTargetDamageCooldown[target]--;
                if (ExtraTargetDamageCooldown[target] <= 0)
                    ExtraTargetDamageCooldown.Remove(target);
            }
        }
		
		public void UpdateSkillEffect(PlayerMod player)
		{
			if (CastTime >= GetBase.CastTime)
            {
                GetBase.Update(player.player, this);
            }
            bool Passive = IsPassive;
            if (Active || Passive)
            {
                if (!Passive && GetBase.UnallowOtherSkillUsage)
                    player.UsingPrivilegedSkill = true;
                if (!Passive && CastTime < GetBase.CastTime)
                {
                    CastTime++;
                    player.SkillBeingCasted = this;
                }
                else
                {
                    Time++;
                }
            }
		}

        public void EndUse(bool Fail = false)
        {
            Active = false;
            Time = 0;
            if (Fail)
                Cooldown = 60;
            if (MainMod.DebugMode)
            {
                Cooldown = 0;
            }
        }
    }
}

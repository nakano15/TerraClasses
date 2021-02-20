using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses
{
    public class SkillData
    {
        public SkillBase GetBase { get { return MainMod.GetSkill(ID, ModID); } }
        public int ID = 0;
        public string ModID = "";
        public int Cooldown = 0;
        public int Level = 0;
        public int Step = 0;
        public int Time = 0;
        public int LastTime { get { return Time > 0 ? Time - 1: 0; } }
        public bool Active = false;
        public string Name { get { return GetBase.Name; } }
        public string Description { get { return GetBase.Description; } }
        public int MaxLevel { get { return GetBase.MaxLevel; } }
        public Enum.SkillTypes SkillTypes { get { return GetBase.skillType; } }
        public bool IsPassive { get { return GetBase.skillType == Enum.SkillTypes.Passive; } }
        private Dictionary<byte, int> IntegerVariables = new Dictionary<byte, int>();
        private Dictionary<byte, float> FloatVariables = new Dictionary<byte, float>();
        private Dictionary<byte, int> NpcDamageCooldown = new Dictionary<byte, int>(), 
            PlayerDamageCooldown = new Dictionary<byte, int>();
        private Dictionary<TargetTranslator.Translator, int> ExtraTargetDamageCooldown = new Dictionary<TargetTranslator.Translator, int>();
        private List<int> PlayerInteraction = new List<int>(), NpcInteraction = new List<int>();
        private List<TargetTranslator.Translator> OtherInteraction = new List<TargetTranslator.Translator>();
        private int Owner = 0;
        private static bool StepChanged = false;

        public void ChangeStep()
        {
            Step++;
            Time = 0;
            StepChanged = true;
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

        public void SetInteger(byte Variable, int Value)
        {
            if (IntegerVariables.ContainsKey(Variable))
                IntegerVariables[Variable] = Value;
            else
                IntegerVariables.Add(Variable, Value);
        }

        public void ChangeInteger(byte Variable, int Value)
        {
            if (IntegerVariables.ContainsKey(Variable))
                IntegerVariables[Variable] += Value;
            else
                IntegerVariables.Add(Variable, Value);
        }

        public int GetInteger(byte Variable)
        {
            if (IntegerVariables.ContainsKey(Variable))
                return IntegerVariables[Variable];
            return 0;
        }

        public void SetFloat(byte Variable, float Value)
        {
            if (FloatVariables.ContainsKey(Variable))
                FloatVariables[Variable] = Value;
            else
                FloatVariables.Add(Variable, Value);
        }

        public void ChangeFloat(byte Variable, float Value)
        {
            if (FloatVariables.ContainsKey(Variable))
                FloatVariables[Variable] += Value;
            else
                FloatVariables.Add(Variable, Value);
        }

        public float GetFloat(byte Variable)
        {
            if (FloatVariables.ContainsKey(Variable))
                return FloatVariables[Variable];
            return 0;
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
            Time = 0;
            Step = 0;
            Active = true;
            Cooldown = GetBase.Cooldown;
            PlayerDamageCooldown.Clear();
            NpcDamageCooldown.Clear();
            ExtraTargetDamageCooldown.Clear();
            PlayerInteraction.Clear();
            NpcInteraction.Clear();
            OtherInteraction.Clear();
        }

        public int HurtPlayer(Player player, int Damage, int DamageDirection, int Cooldown = 8, bool Critical = false)
        {
            if (PlayerDamageCooldown.ContainsKey((byte)player.whoAmI))
                return 0;
            int NewDamage = Damage - player.statDefense / 2;
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
            CombatText.NewText(player.getRect(), Microsoft.Xna.Framework.Color.MediumPurple, NewDamage);
            ApplyCooldownToPlayer(player, Cooldown);
            return NewDamage;
        }

        public int HurtNpc(NPC npc, int Damage, int DamageDirection, float Knockback, int Cooldown = 8, bool Critical = false)
        {
            if (NpcDamageCooldown.ContainsKey((byte)npc.whoAmI) || npc.immortal || npc.dontTakeDamage)
                return 0;
            int NewDamage = Damage - npc.defense / 2;
            if (NewDamage < 1)
                NewDamage = 1;
            if (Critical)
                NewDamage *= 2;
            npc.life -= NewDamage;
            npc.ApplyInteraction(Owner);
            npc.checkDead();
            npc.HitEffect(DamageDirection, Damage);
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
            CombatText.NewText(npc.getRect(), Microsoft.Xna.Framework.Color.MediumPurple, NewDamage);
            ApplyCooldownToNpc(npc, Cooldown);
            return NewDamage;
        }

        public int HurtTarget(TargetTranslator.Translator target, int Damage, int DamageDirection, float Knockback, int Cooldown = 8, bool Critical = false)
        {
            if (ExtraTargetDamageCooldown.ContainsKey(target) || target.Immunity)
                return 0;
            int NewDamage = Damage - target.Defense / 2;
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
        public TargetTranslator.Translator[] GetPossibleTargets(bool Allies, bool SelfIncluded = false)
        {
            List<TargetTranslator.Translator> Targets = new List<TargetTranslator.Translator>();
            Player me = Main.player[Owner];
            for(int i = 0; i < 255; i++)
            {
                if(Main.player[i].active && (SelfIncluded || i != Owner))
                {
                    bool IsAlly = !Main.player[i].hostile || (me.team != 0 && me.team == Main.player[i].team);
                    if((Allies && IsAlly) || (!Allies && !IsAlly))
                        Targets.Add(new TargetTranslator.PlayerTarget(Main.player[i]));
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
            MainMod.GetOtherModTargets(me, Allies);
            return Targets.ToArray();
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
            return OtherInteraction.Contains(target);
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
            if (!ExtraTargetDamageCooldown.ContainsKey(target))
            {
                ExtraTargetDamageCooldown.Add(target, CooldownTime);
            }
            else
            {
                ExtraTargetDamageCooldown[target] = CooldownTime;
            }
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
            if (!Active)
            {
                if (Cooldown > 0)
                    Cooldown--;
                return;
            }
            //Time++;
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

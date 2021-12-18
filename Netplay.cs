using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace TerraClasses
{
    public class Netplay
    {
        private static ModPacket packet { get { return MainMod.GetPacket; } }

        public static void ReceiveMessage(BinaryReader reader, int Sender)
        {
            MessageIDs mes = (MessageIDs)reader.ReadByte();
            switch (mes)
            {
                case MessageIDs.SendExp:
                    {
                        int Player = reader.ReadByte();
                        int Exp = reader.ReadInt32();
                        if(Player == Main.myPlayer)
                        {
                            Main.player[Player].GetModPlayer<PlayerMod>().AddClassExp(Exp);
                        }
                    }
                    break;

                case MessageIDs.SendClassInfo:
                    {
                        int Player = reader.ReadByte();
                        int ClassID = reader.ReadInt32();
                        string ClassModID = reader.ReadString();
                        int ClassLevel = reader.ReadInt32();
                        PlayerMod pm = Main.player[Player].GetModPlayer<PlayerMod>();
                        if(!pm.HasClass(ClassID, ClassModID))
                        {
                            pm.AddClass(ClassID, ClassModID);
                        }
                        ClassData cd = null;
                        foreach(ClassData cd2 in pm.Classes)
                        {
                            if(cd2.ClassID == ClassID && cd2.ClassModID == ClassModID)
                            {
                                cd = cd2;
                                break;
                            }
                        }
                        cd.Level = ClassLevel;
                    }
                    break;

                case MessageIDs.SendClassSkillInfo:
                    {
                        int Player = reader.ReadByte();
                        int ClassID = reader.ReadInt32();
                        string ClassModID = reader.ReadString();
                        int SkillID = reader.ReadInt32();
                        string SkillModID = reader.ReadString();
                        int Level = reader.ReadInt32();
                        bool Active = reader.ReadBoolean();
                        PlayerMod pm = Main.player[Player].GetModPlayer<PlayerMod>();
                        if (!pm.HasClass(ClassID, ClassModID))
                        {
                            pm.AddClass(ClassID, ClassModID);
                        }
                        ClassData cd = pm.GetClass(ClassID, ClassModID);
                        foreach(SkillData sd in cd.Skills)
                        {
                            if(sd.ID == SkillID && sd.ModID == SkillModID)
                            {
                                sd.Level = Level;
                                if(sd.Active != Active)
                                {
                                    if (Active)
                                    {
                                        sd.UseSkill(pm.player);
                                    }
                                    else
                                    {
                                        sd.EndUse();
                                    }
                                }
                            }
                        }
                    }
                    break;

                case MessageIDs.SendSkillHurtPlayer:
                    {
                        int Caster = reader.ReadByte();
                        int Target = reader.ReadByte();
                        int Damage = reader.ReadInt32();
                        int Direction = reader.ReadSByte();
                        Player Victim = Main.player[Target];
                        Victim.statLife -= Damage;
                        if(Victim.statLife <= 0)
                        {
                            Victim.KillMe(Terraria.DataStructures.PlayerDeathReason.ByPlayer(Caster), Damage, Direction, true);
                        }
                        else
                        {
                            if (Victim.stoned)
                            {
                                Main.PlaySound(0, (int)Victim.position.X, (int)Victim.position.Y, 1, 1f, 0f);
                            }
                            else if (Victim.frostArmor)
                            {
                                Main.PlaySound(Terraria.ID.SoundID.Item27, Victim.position);
                            }
                            else if ((Victim.wereWolf || Victim.forceWerewolf) && !Victim.hideWolf)
                            {
                                Main.PlaySound(3, (int)Victim.position.X, (int)Victim.position.Y, 6, 1f, 0f);
                            }
                            else if (Victim.boneArmor)
                            {
                                Main.PlaySound(3, (int)Victim.position.X, (int)Victim.position.Y, 2, 1f, 0f);
                            }
                            else if (!Victim.Male)
                            {
                                Main.PlaySound(20, (int)Victim.position.X, (int)Victim.position.Y, 1, 1f, 0f);
                            }
                            else
                            {
                                Main.PlaySound(1, (int)Victim.position.X, (int)Victim.position.Y, 1, 1f, 0f);
                            }
                        }
                        CombatText.NewText(Victim.getRect(), Microsoft.Xna.Framework.Color.MediumPurple, Damage);
                    }
                    break;

                case MessageIDs.SendSkillHurtNpc:
                    {
                        int Caster = reader.ReadByte();
                        int Npc = reader.ReadByte();
                        int Damage = reader.ReadInt32();
                        float Knockback = reader.ReadSingle();
                        int Direction = reader.ReadSByte();
                        NPC npc = Main.npc[Npc];
                        npc.life -= Damage;
                        npc.ApplyInteraction(Caster);
                        npc.checkDead();
                        npc.HitEffect(Direction, Damage);
                        if (npc.life > 0)
                        {
                            Main.PlaySound(npc.HitSound, npc.Center);
                            if (Knockback > 0 && npc.knockBackResist > 0)
                            {
                                float NewKB = Knockback * npc.knockBackResist;
                                npc.velocity.X += Direction * NewKB;
                                npc.velocity.Y -= (npc.noGravity ? 0.75f : 0.5f);
                            }
                        }
                        CombatText.NewText(npc.getRect(), Microsoft.Xna.Framework.Color.MediumPurple, Damage);
                    }
                    break;
            }
        }

        public static void SendExp(int Player, int Exp)
        {
			if(Main.netMode == 0)
				return;
            /*using (Message mes = new Message(MessageIDs.SendExp, Player))
            {
                mes.Write((byte)Player);
                mes.Write(Exp);
            }*/
            MainMod.ResetPacket();
            packet.Write((byte)MessageIDs.SendExp);
            packet.Write((byte)Player);
            packet.Write(Exp);
            packet.Send(Player, -1);
        }

        public static void SendClassInfo(int Player, int ClassPosition)
        {
            if (Main.netMode == 0)
                return;
            MainMod.ResetPacket();
            PlayerMod pm = Main.player[Player].GetModPlayer<PlayerMod>();
            if (ClassPosition < 0 || ClassPosition >= pm.Classes.Count)
                return;
            packet.Write((byte)MessageIDs.SendClassInfo);
            packet.Write((byte)Player);
            ClassData cd = pm.Classes[ClassPosition];
            packet.Write(cd.ClassID);
            packet.Write(cd.ClassModID);
            packet.Write(cd.Level);
            packet.Send(-1, Player);
        }

        public static void SendClassSkillInfo(int Player, int ClassPosition, int SkillPosition)
        {
            if (Main.netMode == 0)
                return;
            MainMod.ResetPacket();
            PlayerMod pm = Main.player[Player].GetModPlayer<PlayerMod>();
            if (ClassPosition < 0 || ClassPosition >= pm.Classes.Count)
                return;
            ClassData cd = pm.Classes[ClassPosition];
            if (SkillPosition < 0 || SkillPosition >= cd.Skills.Count)
                return;
            SkillData sd = cd.Skills[SkillPosition];
            packet.Write((byte)MessageIDs.SendClassSkillInfo);
            packet.Write((byte)Player);
            packet.Write(cd.ClassID);
            packet.Write(cd.ClassModID);
            packet.Write(sd.ID);
            packet.Write(sd.ModID);
            packet.Write(sd.RealLevel);
            packet.Write(sd.Active);
            packet.Send(-1, Player);
        }

        public static void SendSkillHurtPlayer(int Caster, int Target, int Damage, int DamageDirection)
        {
            if (Main.netMode == 0)
                return;
            MainMod.ResetPacket();
            packet.Write((byte)MessageIDs.SendSkillHurtPlayer);
            packet.Write((byte)Caster);
            packet.Write((byte)Target);
            packet.Write(Damage);
            packet.Write((sbyte)DamageDirection);
            packet.Send(-1, Caster);
        }

        public static void SendSkillHurtNPC(int Caster, int Npc, int Damage, float Knockback, int Direction)
        {
            if (Main.netMode == 0)
                return;
            MainMod.ResetPacket();
            packet.Write((byte)MessageIDs.SendSkillHurtNpc);
            packet.Write((byte)Caster);
            packet.Write((byte)Npc);
            packet.Write(Damage);
            packet.Write(Knockback);
            packet.Write((sbyte)Direction);
            packet.Send(-1, Caster);
        }

        public class Message : BinaryWriter, IDisposable
        {
            int ToWho = -1, FromWho = -1;

            public Message(MessageIDs id, int ToWho = -1, int FromWho = -1)
            {
                this.ToWho = ToWho;
                this.FromWho = FromWho;
                Write((byte)id);
            }

            public new void Dispose()
            {
                MainMod.ResetPacket();
                ModPacket packet = MainMod.GetPacket;
                BaseStream.Position = 0;
                int b;
                while((b = BaseStream.ReadByte()) != -1) //Doesn't seems to work, it isn't placing the bytes on the packet. It seems to literally be sending an empty message
                {
                    packet.Write((byte)b);
                }
                packet.Send(ToWho, FromWho);
            }
        }

        public static void SendMessageToOtherPlayers(string Message, Microsoft.Xna.Framework.Color color)
        {
            NetMessage.BroadcastChatMessage(Terraria.Localization.NetworkText.FromLiteral(Message), color, Main.myPlayer);
        }

        public enum MessageIDs : byte
        {
            SendExp,
            SendClassInfo,
            SendClassSkillInfo,
            //SkillDataInfos
            SendSkillHurtPlayer,
            SendSkillHurtNpc
        }
    }
}

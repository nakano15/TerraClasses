using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace TerraClasses
{
    public class ClassUnlockLoreBase
    {
        public List<LoreObjective> Objectives = new List<LoreObjective>();

        public void AddHuntObjective(int MobID, int Stack, string LorePiece)
        {
            HuntMonsterObjective hmo = new HuntMonsterObjective(MobID, Stack, LorePiece);
            Objectives.Add(hmo);
        }

        public void AddGatherItemObjective(int ItemID, int Stack, string LorePiece)
        {
            GatherItemObjective gio = new GatherItemObjective(ItemID, Stack, LorePiece);
            Objectives.Add(gio);
        }

        public void AddTalkToNpcObjective(int NpcID, string Message, string LorePiece, string CustomNpcName = "")
        {
            TalkToNpcObjective tno = new TalkToNpcObjective(NpcID, Message, LorePiece, CustomNpcName);
            Objectives.Add(tno);
        }

        public void AddGatherObjectObjective(string ObjectName, int MobID, int Stack, string LorePiece, float DropRate = 0.5f)
        {
            GatherObjectObjective goo = new GatherObjectObjective(ObjectName, MobID, Stack, LorePiece, DropRate);
            Objectives.Add(goo);
        }

        public class LoreObjective
        {
            public string LorePiece = "";
            public ObjectiveType objectivetype = ObjectiveType.None;
        }

        public class HuntMonsterObjective : LoreObjective
        {
            public int MobID = 0, Stack = 0;
            private string MobName = "";
            public string GetMobName { get { return MobName; } }

            public HuntMonsterObjective(int MobID, int Stack, string LorePiece)
            {
                this.MobID = MobID;
                this.Stack = Stack;
                this.LorePiece = LorePiece;
                NPC n = new NPC();
                n.SetDefaults(MobID);
                MobName = n.TypeName;
                objectivetype = ObjectiveType.Hunt;
            }
        }

        public class GatherItemObjective : LoreObjective
        {
            public int ItemID = 0, Stack = 0;
            private string ItemName = "";
            public string GetItemName { get { return ItemName; } }

            public GatherItemObjective(int ItemID, int Stack, string LorePiece)
            {
                this.ItemID = ItemID;
                this.Stack = Stack;
                this.LorePiece = LorePiece;
                Item i = new Item();
                i.SetDefaults(ItemID);
                ItemName = i.Name;
                objectivetype = ObjectiveType.GatherItem;
            }
        }

        public class TalkToNpcObjective : LoreObjective
        {
            public int NpcID = 0;
            public string MessageText = "", NpcName = "";

            public TalkToNpcObjective(int NpcID, string Message, string LorePiece, string CustomNpcName = "")
            {
                this.NpcID = NpcID;
                this.MessageText = Message;
                if (CustomNpcName == "")
                {
                    NPC npc = new NPC();
                    npc.SetDefaults(NpcID);
                    NpcName = npc.GivenOrTypeName;
                }
                else
                {
                    NpcName = CustomNpcName;
                }
                objectivetype = ObjectiveType.TalkToNpc;
                this.LorePiece = LorePiece;
            }
        }

        public class GatherObjectObjective : LoreObjective
        {
            public int MobID, Stack;
            private string MobName, ObjectName;
            public float DropRate;
            public string GetMobName { get { return MobName; } }
            public string GetObjectName { get { return ObjectName; } }

            public GatherObjectObjective(string ObjectName, int MobID, int Stack, string LorePiece, float DropRate = 0.5f)
            {
                this.LorePiece = LorePiece;
                this.MobID = MobID;
                this.Stack = Stack;
                this.DropRate = DropRate;
                this.ObjectName = ObjectName;
                NPC n = new NPC();
                n.SetDefaults(MobID);
                MobName = n.TypeName;
                objectivetype = ObjectiveType.GatherObject;
            }
        }

        public enum ObjectiveType
        {
            None,
            Hunt,
            GatherItem,
            TalkToNpc,
            GatherObject
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraClasses.Items.ClassBooks
{
    public class ClassLoreBookPrefab : ModItem
    {
        private int ClassID;
        private string ClassModID;
        private ClassUnlockLoreData LoreData { get { return MainMod.GetClassLoreData(ClassID, ClassModID); } }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override void SetStaticDefaults()
        {

        }
        
        public ClassLoreBookPrefab(int ClassID, string ClassModID = "")
        {
            this.ClassID = ClassID;
            this.ClassModID = ClassModID;
        }

        public override void SetDefaults()
        {
            item.useTurn = true;
            item.width = 20;
            item.height = 20;
            item.useStyle = 4;
            item.useTime = 90;
            item.UseSound = SoundID.Item6;
            item.useAnimation = 90;
            item.rare = 1;
            item.value = 50000;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int PlacementPosition = -1;
            for (int i = 0; i < tooltips.Count; i++ )
            {
                if (tooltips[i].Name == "Price")
                {
                    PlacementPosition = i;
                    break;
                }
            }
            string[] TextToConcatenate = LoreData.GetText().Split('\n');
            byte Line = 0;
            foreach (string Text in TextToConcatenate)
            {
                TooltipLine tl = new TooltipLine(mod, "ClassInfoLine" + Line++, Text);
                if (PlacementPosition == -1)
                    tooltips.Add(tl);
                else
                {
                    tooltips.Insert(PlacementPosition++, tl);
                }
            }
        }

        /*public override Terraria.ModLoader.IO.TagCompound Save()
        {
            Terraria.ModLoader.IO.TagCompound tag = new Terraria.ModLoader.IO.TagCompound();
            LoreData.Save(tag);
            return tag;
        }

        public override void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            LoreData.Load(tag);
        }*/

        public void OnMobKill(NPC npc)
        {
            LoreData.OnMobKill(npc.type, npc.netID);
        }
        
        public override void UpdateInventory(Player player)
        {
            if(LoreData != null)
                LoreData.Update(player);
        }

        public override bool UseItem(Player player)
        {
            if (LoreData != null)
            {
                if (LoreData.IsComplete)
                {
                    MainMod.AddClassUnlocked(ClassID, ClassModID);
                    MainMod.DeleteClassLoreData(ClassID, ClassModID);
                }
                else
                {
                    LoreData.OnBookUsed(player);
                }
                return player.whoAmI == Main.myPlayer;
            }
            return false;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.Items.ClassBooks
{
    public class FighterBook : ClassLoreBookPrefab
    {
        public FighterBook()
            : base(1)
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 500;
        }
    }
}

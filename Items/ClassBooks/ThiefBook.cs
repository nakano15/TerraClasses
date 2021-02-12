using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.Items.ClassBooks
{
    public class ThiefBook : ClassLoreBookPrefab
    {
        public ThiefBook()
            : base(6)
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 500;
        }
    }
}

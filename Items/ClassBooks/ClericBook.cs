using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.Items.ClassBooks
{
    public class ClericBook : ClassLoreBookPrefab
    {
        public ClericBook()
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

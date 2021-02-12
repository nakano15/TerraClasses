using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.Items.ClassBooks
{
    public class BersekerBook : ClassLoreBookPrefab
    {
        public BersekerBook()
            : base(9)
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 500;
        }
    }
}

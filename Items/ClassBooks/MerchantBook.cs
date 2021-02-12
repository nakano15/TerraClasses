using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerraClasses.Items.ClassBooks
{
    public class MerchantBook : ClassLoreBookPrefab
    {
        public MerchantBook()
            : base(3)
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 500;
        }
    }
}

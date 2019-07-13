using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory
{
    class Item
    {
        public string pname {get; set;}
        public double sellingPrice { get; set; }

        public double costPrice { get; set; }

        public double quantity { get; set; }
        //public Item(string pname, double sellingPrice, double costPrice )
        //{
        //    this.pname = pname;
        //    this.sellingPrice = sellingPrice;
        //    this.costPrice = costPrice;
        //  //  this.quantity = quantity;
        //}
    }

}


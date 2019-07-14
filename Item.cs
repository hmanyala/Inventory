using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory
{
    public class Item
    {
        public string pname {get; set;}
        public double sellingPrice { get; set; }
        public double OldSellingPrice { get; set; }

       // public double QtySoldSoFar { get; set; }
        public double CostPrice { get; set; }

        public double TotalQuantity { get; set; }
        public double quantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Transaction> transactions { get; set; }

        //public Item(string pname, double sellingPrice, double costPrice )
        //{
        //    this.pname = pname;
        //    this.sellingPrice = sellingPrice;
        //    this.costPrice = costPrice;
        //  //  this.quantity = quantity;
        //}

 
    }

    public class Transaction
    {
        public double SoldQty { get; set; }
        public double SellingPrice { get; set; }
    }

}


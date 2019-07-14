using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Inventory
{


    //Commands    
    public class AddItem : ICommand
    {
        Item _invitem = new Item();
        Mobile rcver = new Mobile();
        public bool IsCompleted { get; set; }
        public double QtyUpdated { get; set; }
        public AddItem(string pname, double sellingPrice, double costPrice)
        {
            _invitem.pname = pname;
            _invitem.sellingPrice = Math.Round(sellingPrice,2);
            _invitem.CostPrice = Math.Round(costPrice, 2);
        }
        public void Process()
        {
            rcver.AddItems(_invitem);
            IsCompleted = rcver.IsCompleted;

        }

        private void AddItems(Item item)
        {
            var jsonData = System.IO.File.ReadAllText(Helper.FilePath);
            var pList = JsonConvert.DeserializeObject<List<Item>>(jsonData)
                                  ?? new List<Item>();
            pList.Add(item);

            jsonData = JsonConvert.SerializeObject(pList);
            System.IO.File.WriteAllText(Helper.FilePath, jsonData);
            IsCompleted = true;
            Console.WriteLine(" Item Added Successfully");
        }
    }

    public class InventoryReport : ICommand
    {
        public bool IsCompleted { get; set; }

        Mobile rcver = new Mobile();
        public void Process()
        {
            rcver.getReport();
            IsCompleted = rcver.IsCompleted;
            Console.WriteLine("Report Generated Successfully");

        }

        
    }

    public class ProfitReport : ICommand
    {
        public bool IsCompleted { get; set; }

        Mobile rcver = new Mobile();
        public void Process()
        {
            rcver.getProfitReport();
            IsCompleted = rcver.IsCompleted;
            Console.WriteLine("\n\nReport Generated Successfully");

        }
    }
        public class UpdateSoldQty : ICommand
    {
        public bool IsCompleted { get; set; }
        public double QtyUpdated { get; set; }
        string _pname = string.Empty;
        double _qty = 0.00;

        Mobile rcver = null;
        public UpdateSoldQty(string pro, double qty)
        {
            _pname = pro;
            _qty = Math.Round(qty, 2) ;
            rcver = new Mobile();
        }
        public void Process()
        {
            //  UpdateProduct(_pname, _qty);
            rcver.UpdateProduct(_pname, _qty, false);
            IsCompleted = rcver.IsCompleted;
            QtyUpdated = rcver.QtyUpdated;

        }


    }

    public class UpdatePurchasedQty : ICommand
    {
        public bool IsCompleted { get; set; }
        public double QtyUpdated { get; set; }
        Mobile rcver = null;
        string _pname = string.Empty;
        double _qty = 0.00;
        public UpdatePurchasedQty(string pro, double qty)
        {
            _pname = pro;
            _qty = Math.Round(qty, 2);
            rcver = new Mobile();
        }
        public void Process()
        {
            //  UpdateProduct(_pname, _qty);
            rcver.UpdateProduct(_pname, _qty, true);
            IsCompleted = rcver.IsCompleted;
            QtyUpdated = rcver.QtyUpdated;

        }

    }

    public class UpdateSellingPrice : ICommand
    {
        public bool IsCompleted { get; set; }
        public double PriceUpdated { get; set; }
        Mobile rcver = null;
        string _pname = string.Empty;
        double _sp = 0.00;
        public UpdateSellingPrice(string pro, double sp)
        {
            _pname = pro;
            _sp = sp;
           
            rcver = new Mobile();
        }
        public void Process()
        {

            rcver.UpdateSellingPrice(_pname, _sp);
            IsCompleted = rcver.IsCompleted;
            PriceUpdated = rcver.PriceUpdated;
        }

    }
    public class Delete : ICommand
    {
        public bool IsCompleted { get; set; }
        string _pname = string.Empty;

        Mobile rcver = null;
        public Delete(string pname)
        {
            _pname = pname;
            rcver = new Mobile();
        }

        public void Process()
        {
            rcver.DeleteItem(_pname);
            IsCompleted = rcver.IsCompleted;

        }

    }



}

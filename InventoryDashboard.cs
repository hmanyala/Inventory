using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Inventory
{
    
    
    //Commands    
public class Create : ICommand
    {
        Item _invitem = new Item() ;
        public bool IsCompleted { get; set; }
        //  public Create(Item item)
        public Create(string pname , double sellingPrice , double costPrice)
        {
           
            _invitem.pname = pname;
            _invitem.sellingPrice = sellingPrice;
            _invitem.costPrice = costPrice;
        }
        public void Execute()
        {
            AddItems(_invitem);
           
        }

      

        private void AddItems(Item item)
        {
            // Read existing json data
            var jsonData = System.IO.File.ReadAllText(Helper.FilePath);
            var pList = JsonConvert.DeserializeObject<List<Item>>(jsonData)
                                  ?? new List<Item>();
            pList.Add(item);

            // Update json data string
            jsonData = JsonConvert.SerializeObject(pList);
            System.IO.File.WriteAllText(Helper.FilePath, jsonData);
            IsCompleted = true;
            Console.WriteLine(" Item Added Successfully");
        }
    }

    public class Report : ICommand
    {
        public bool IsCompleted { get; set; }
        public void Execute()
        {
            getReport();
            IsCompleted = true;
            Console.WriteLine("Report Generated Successfully");

        }


        private void getReport()
        {
            // Read existing json data
            var jsonData = System.IO.File.ReadAllText(Helper.FilePath);
            var pList = JsonConvert.DeserializeObject<List<Item>>(jsonData)
                                  ?? new List<Item>();
            
           DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("SellingPrice", typeof(double));
            dt.Columns.Add("CostPrice", typeof(double));
            dt.Columns.Add("Qty", typeof(double));
            dt.Columns.Add("Value",typeof(double));

            foreach (var item in pList)
            {
                dr = dt.NewRow();
                dr["Item"] = item.pname; 
                dr["CostPrice"] = item.costPrice; 
                dr["SellingPrice"] = item.sellingPrice;
                dr["Qty"] = item.quantity;
                dr["Value"] = item.costPrice * item.quantity;

                dt.Rows.Add(dr);
            }

            Console.WriteLine("         -----------Inventory Report-----------");
            Console.WriteLine("Item Name\tBought at\t Sold at\t Available Qty\t Value ");
            foreach (DataRow dro in dt.Rows)
            {

                Console.WriteLine("{0}  \t {1} \t {2} \t {3} \t\bnmjm,  {4}",
                    dro[0], dro[1], dro[2] , dro[3],dro[4]);
            }



        }
    }
    public class UpdateBuy : ICommand
    {
        public bool IsCompleted { get; set; }
        public double QtyUpdated { get; set; }
        string _pname = string.Empty;
        double _qty = 0.00;
        public UpdateBuy(string pro, double qty)
        {
            _pname = pro;
            _qty = qty;
        }
        public void Execute()
        {
            UpdateProduct(_pname, _qty);
         
        }

        public  void UpdateProduct(string pname, double quantity)
        {
            // Read existing json data
            var json = System.IO.File.ReadAllText(Helper.FilePath);
            pname = pname.ToUpper();
            var curritems = JsonConvert.DeserializeObject<List<Item>>(json.ToUpper());
            var curritem = JsonConvert.SerializeObject(curritems.Where(i => i.pname == pname));
            if (curritem.Count() > 0)
            {

                JArray jArray = JArray.Parse(json);
                var jObjects = jArray.ToObject<List<JObject>>();

                foreach (var obj in jObjects)
                {
                    if (obj["pname"].ToString().ToUpper() == pname)
                    {
                        var i = jObjects.IndexOf(obj);
                        var q = obj["quantity"].ToString();
                        var curQty = Convert.ToDouble(q);
                        if (quantity > 0)
                        {
                            QtyUpdated = curQty + quantity;
                            obj["quantity"] = QtyUpdated;
                            Console.WriteLine("Item Quantity Updated(Aded) to " + QtyUpdated);
                            JArray outputArray = JArray.FromObject(jObjects);
                            string output = outputArray.ToString();
                            File.WriteAllText(Helper.FilePath, output);
                            IsCompleted = true;
                        }
                        else
                        {
                            Console.WriteLine("Entered item Quantity should  be greater than zero " + quantity);
                        }
                    }


                }
             
            }
            else
            {
                Console.WriteLine("Item not found");
            }

        }


    }

        public class UpdateSell : ICommand
        {
            public bool IsCompleted { get; set; }
        public double QtyUpdated { get; set; }

        string _pname = string.Empty;
            double _qty = 0.00;
            public UpdateSell(string pro, double qty)
            {
                _pname = pro;
                _qty = qty;
            }
            public void Execute()
            {
                UpdateProduct(_pname, _qty);
             
            }

            public  void UpdateProduct(string pname, double quantity)
            {
                // Read existing json data
                var json = System.IO.File.ReadAllText(Helper.FilePath);
            pname = pname.ToUpper();
                var curritems = JsonConvert.DeserializeObject<List<Item>>(json.ToUpper());
                var curritem = JsonConvert.SerializeObject(curritems.Where(i => i.pname == pname));
            if (curritem.Count() > 0)
            {
                JArray jArray = JArray.Parse(json);
                var jObjects = jArray.ToObject<List<JObject>>();

                foreach (var obj in jObjects)
                {
                    if (obj["pname"].ToString().ToUpper() == pname)
                    {
                        var i = jObjects.IndexOf(obj);
                        var q = obj["quantity"].ToString();
                        var curQty = Convert.ToDouble(q);
                        if (quantity > 0)
                        {
                            if (curQty >= quantity)
                            {
                                QtyUpdated = curQty - quantity;
                                obj["quantity"] = QtyUpdated;
                                Console.WriteLine("Item Quantity Updated(Removed) to " + QtyUpdated);
                                JArray outputArray = JArray.FromObject(jObjects);
                                string output = outputArray.ToString();
                                File.WriteAllText(Helper.FilePath, output);
                                IsCompleted = true;
                            }
                            else
                            {
                                Console.WriteLine("Item Quantity should not be greater current quantity "+ curQty);
                            }
                        }
                    }
                }
                
               
            }
            else
            {
                Console.WriteLine("Item not found");
            }


            }
        }

    public class Delete : ICommand
    {
        public bool IsCompleted { get; set; }
        string _pname = string.Empty;
        public Delete(string pname)
        {
            _pname = pname;
        }

        public void Execute()
        {
            DeleteItem(_pname);
          
        }

        private void DeleteItem(string pname)
        {
           // Read existing json data
            var json = System.IO.File.ReadAllText(Helper.FilePath);

            pname = pname.ToUpper();
            var curritems = JsonConvert.DeserializeObject<List<Item>>(json.ToUpper());
            var curritem = JsonConvert.SerializeObject(curritems.Where(i => i.pname == pname));
            if (curritem.Count() > 0)
            {
                var newJsonString = JsonConvert.SerializeObject(curritems.Where(i => i.pname != pname));

                var jsonData = JsonConvert.SerializeObject(newJsonString).ToString();
                System.IO.File.WriteAllText(Helper.FilePath, jsonData.Replace("[]",""));
                IsCompleted = true;
                Console.WriteLine("Item deleted");
            }
            else
            {
                Console.WriteLine("Item not found");
            }
                                                         
        }
    
    }

    //Helper

    public static class Helper
    {
        public static string FilePath
                {
                    get
                    {

                        var filePath = System.Configuration.ConfigurationManager.AppSettings["Source"];
                        //  var filePath = @"C:\Users\hmanyala\source\repos\Inventory\data.json";
                        
                        return filePath;
                    }
                }    


    }
}

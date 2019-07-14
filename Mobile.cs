using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Inventory
{
    public class Mobile
    {
        public bool IsCompleted { get; set; }
        public double QtyUpdated { get; set; }

        public double PriceUpdated { get; set; }
        public Mobile()
        {

        }


        /// <summary>
        /// Add Items
        /// </summary>
        /// <param name="item"></param>
        public void AddItems(Item item)
        {
            List<Transaction> t = new List<Transaction>();         
            var pList = JsonConvert.DeserializeObject<List<Item>>(Helper.JSONdata.ToUpper())
                                  ?? new List<Item>();
            var curritem = JsonConvert.SerializeObject(pList.Where(i => i.pname == item.pname.ToUpper()));
            int count = curritem.Count();
            if (!curritem.Equals("[]"))
            {
                Console.WriteLine("Item already exists");
            }
            else
            {
                pList.Add(item);
                var jArray = JArray.FromObject(pList);
                var jObjects = jArray.ToObject<List<JObject>>();
                JArray outputArray = JArray.FromObject(jObjects);
               // string output = outputArray.ToString();
                string output = jArray.ToString();
                System.IO.File.WriteAllText(Helper.FilePath, output);
                IsCompleted = true;
                Console.WriteLine(" Item Added Successfully");
            }
        }


        /// <summary>
        /// Generate Inventory Report
        /// </summary>
        public void getReport()
        {
            var pList = JsonConvert.DeserializeObject<List<Item>>(Helper.JSONdata)
                                  ?? new List<Item>();
            if (pList.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("Item", typeof(string));
                dt.Columns.Add("SellingPrice", typeof(double));
                dt.Columns.Add("CostPrice", typeof(double));
                dt.Columns.Add("Qty", typeof(double));
                dt.Columns.Add("Value", typeof(double));

                foreach (var item in pList)
                {
                    dr = dt.NewRow();
                    dr["Item"] = item.pname;
                    dr["CostPrice"] = item.CostPrice;
                    dr["SellingPrice"] = item.sellingPrice;
                    dr["Qty"] = item.quantity;
                    dr["Value"] = item.CostPrice * item.quantity;

                    dt.Rows.Add(dr);
                }

                Console.WriteLine("         -----------Inventory Report-----------");
                Console.WriteLine("Item Name\t\tBoughtAt\t\tSoldAt\tAvailableQty\tValue");
                foreach (DataRow dro in dt.Rows)
                {

                    Console.WriteLine("{0}\t\t\t{1}\t\t{2}\t\t{3}\t{4}",
                        dro[0], dro[1], dro[2], dro[3], dro[4]);
                }
                IsCompleted = true;
            }
            else
            {
                Console.WriteLine("No items found");
            }

        }

        /// <summary>
        /// Generate Profit Report
        /// </summary>
        public void getProfitReport()
        { 
            var pList = JsonConvert.DeserializeObject<List<Item>>(Helper.JSONdata)
                                  ?? new List<Item>();
            if (pList.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("Item", typeof(string));
                dt.Columns.Add("TotalQty", typeof(double));
                dt.Columns.Add("CostPrice", typeof(double));
                dt.Columns.Add("OldSellingPrice", typeof(double));
                dt.Columns.Add("NewSellingPrice", typeof(double));
                dt.Columns.Add("Amount(CostPrice)", typeof(double));
                dt.Columns.Add("Amount(OldSellingPrice)", typeof(double));
                dt.Columns.Add("Profit(OldSellingPrice)", typeof(double));
                dt.Columns.Add("Amount(NewSellingPrice)", typeof(double));
                dt.Columns.Add("Profit(NewSellingPrice)", typeof(double));

                foreach (var item in pList)
                {
                    double ntranAmt = 0.00, otranAmt = 0.0;
                    dr = dt.NewRow();

                    dr["Item"] = item.pname;
                    dr["TotalQty"] = item.TotalQuantity;
                    dr["CostPrice"] = item.CostPrice;
                    dr["OldSellingPrice"] = item.OldSellingPrice;
                    dr["NewSellingPrice"] = item.sellingPrice;
                    dr["Amount(CostPrice)"] = item.CostPrice * item.TotalQuantity;
                    if (item.transactions != null)
                    {
                        if (item.transactions.Count > 0)
                        {
                            foreach (var tran in item.transactions.Where(x => x.SellingPrice == item.sellingPrice))
                            {
                                ntranAmt += (tran.SoldQty * tran.SellingPrice);
                            }
                            foreach (var tran in item.transactions.Where(x => x.SellingPrice == item.OldSellingPrice))
                            {
                                otranAmt += (tran.SoldQty * tran.SellingPrice);
                            }
                        }
                    }
                    //  var j = (item.OldSellingPrice * item.QtySoldSoFar);

                    dr["Amount(OldSellingPrice)"] = otranAmt;
                    dr["Profit(OldSellingPrice)"] =  otranAmt - (item.CostPrice * item.TotalQuantity);

                    dr["Amount(NewSellingPrice)"] = ntranAmt;
                    dr["Profit(NewSellingPrice)"] = ntranAmt - (item.CostPrice * item.TotalQuantity);

                    dt.Rows.Add(dr);
                }

                Console.WriteLine("         -----------Profit Report-----------");
                Console.WriteLine("Name\t\t\tTotalQuantity\t\tSellingPrice(Old)\tSellingPrice(New)\tAmount(CostPrice)\tAmount(OP)\t Profit(OP)\t\tAmount(NP)\t Profit(NP)");
                foreach (DataRow dro in dt.Rows)
                {

                    Console.WriteLine("{0}\t\t\t{1}\t\t{2}\t\t\t{3}\t\t\t{4}\t\t\t{5}\t\t{6}\t\t\t{7}\t\t\t{8}",
                        dro[0], dro[1], dro[3], dro[4], dro[5],dro[6], dro[7], dro[8], dro[9]);
                }

                IsCompleted = true;
            }
            else
            {
                Console.WriteLine("No items found");
            }

        }

        /// <summary>
        /// Manage  Inventory (Purhases/Sales)
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="quantity"></param>
        /// <param name="isSoldQty"></param>
        public void UpdateProduct(string pname, double quantity, bool isSoldQty)
        {

            // Read existing json data
            List<Transaction> t = new List<Transaction>();
            pname = pname.ToUpper();
            var curritems = JsonConvert.DeserializeObject<List<Item>>(Helper.JSONdata.ToUpper());
            var curritem = JsonConvert.SerializeObject(curritems.Where(i => i.pname == pname));
             
            if (!curritem.Equals("[]"))
            {

                var jobjList = JArray.Parse(Helper.JSONdata);
                JArray jArray = JArray.Parse(curritem);
                var currjObject = jobjList.ToObject<List<JObject>>();

                foreach (var obj in currjObject)
                {
                    if (obj["pname"].ToString().ToUpper() == pname)
                    {
                        var i = currjObject.IndexOf(obj);
                        var q = obj["quantity"].ToString();
                        var curQty = Convert.ToDouble(q);
                        if (isSoldQty)
                        { 
                            #region Manage Sold Qty
                            if (curQty >= quantity)
                            {
                                QtyUpdated = curQty - quantity;
                                obj["quantity"] = QtyUpdated;
                                Console.WriteLine("Item Quantity(Deducted) from Inventory" + QtyUpdated);

                                var h = obj.Property("transactions");
                                if (h == null)
                                {

                                    obj.Add("transactions", new JArray(
                                                                new JObject(
                                                        new JProperty("SoldQty", quantity),
                                                        new JProperty("SellingPrice", obj["sellingPrice"])
                                                        )
                                                    ));
                                }
                                else
                                {

                                    JArray item = (JArray)obj["transactions"]; 
                                               item.Add(new JObject(
                                                        new JProperty("SoldQty", quantity),
                                                        new JProperty("SellingPrice", obj["sellingPrice"])
                                                        )
                                                    );
                                }
                        

                                JArray outputArray = JArray.FromObject(currjObject);
                                
                                string output = outputArray.ToString();

                                File.WriteAllText(Helper.FilePath, output);
                                IsCompleted = true;
                                #endregion
                            }
                            else
                            {
                                Console.WriteLine("Entered quantity should not be greater than current quantity " + curQty+
                                    "\n Check item inventory!!");
                                
                            }
                        }
                        else
                        {
                            #region Manage Purchased Qty
                            if (quantity > 0)
                            {
                                QtyUpdated = curQty + quantity;
                                obj["quantity"] = QtyUpdated;
                                obj["TotalQuantity"] = Convert.ToDouble(obj["TotalQuantity"]) + quantity;
                                Console.WriteLine("Item Quantity Added to Inventory. Available Stock is " + QtyUpdated);
                                JArray outputArray = JArray.FromObject(currjObject);
                                string output = outputArray.ToString();
                                File.WriteAllText(Helper.FilePath, output);
                                IsCompleted = true;
                            }
                            else
                            {
                                Console.WriteLine("Entered item Quantity should  be greater than zero " + quantity);
                            }
                            #endregion
                        }
                    }
                }


            }
            else
            {
                Console.WriteLine("Item not found");
            }


        }

        /// <summary>
        /// Manage Item Pricing
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="newSP"></param>
        public void UpdateSellingPrice(string pname, double newSP)
        {
            var curritems = JsonConvert.DeserializeObject<List<Item>>(Helper.JSONdata.ToUpper());
         
            var curritem = JsonConvert.SerializeObject(curritems.Where(i => i.pname == pname.ToUpper()));
            if (!curritem.Equals("[]"))
            {
                JArray jArray = JArray.Parse(Helper.JSONdata);
                var jObjects = jArray.ToObject<List<JObject>>();

                foreach (var obj in jObjects)
                {
                    //obj.Add("OldSellingPrice");
                    if (obj["pname"].ToString().ToUpper() == pname.ToUpper())
                    {
                        var i = jObjects.IndexOf(obj);
                        var q = obj["sellingPrice"].ToString();
                        var curSP = Convert.ToDouble(q);
                        if (newSP > 0)
                        {
                            obj["OldSellingPrice"] = curSP;
                            obj["sellingPrice"] = newSP;
                            Console.WriteLine("\n Item Selling Price is changed to " + newSP);
                            JArray outputArray = JArray.FromObject(jObjects);
                            string output = outputArray.ToString();
                            File.WriteAllText(Helper.FilePath, output);
                            IsCompleted = true;
                            PriceUpdated = newSP;
                        }
                        else
                        {
                            Console.WriteLine("\nSelling price should be greater than zero");
                        }

                    }
                }


            }
            else
            {
                Console.WriteLine("Item not found");
            }


        }

        /// <summary>
        /// Delete Item
        /// </summary>
        /// <param name="pname"></param>
        public void DeleteItem(string pname)
        {
            var json = File.ReadAllText(Helper.FilePath);
            pname = pname.ToUpper();
            var curritems = JsonConvert.DeserializeObject<List<Item>>(json.ToUpper());
            var curritem = JsonConvert.SerializeObject(curritems.Where(i => i.pname == pname));
            if (!curritem.Equals("[]"))
            {
                var newJsonString = JsonConvert.SerializeObject(curritems.Where(i => i.pname != pname));
                JArray jArray = JArray.Parse(newJsonString);
                var jObjects = jArray.ToObject<List<JObject>>();
                JArray outputArray = JArray.FromObject(jObjects);
                string output = outputArray.ToString();
                System.IO.File.WriteAllText(Helper.FilePath, output.Replace("[]", string.Empty));
                IsCompleted = true;
                Console.WriteLine("\n Item deleted successfulyy");
            }
            else
            {
                Console.WriteLine("\n Item not found");
            }

        }

    }

}


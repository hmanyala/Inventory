using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Inventory
{
    
    public interface ICommand
    {
        void Execute();
    }

    //Invoker
    public class Mobile
    {
        public Mobile()
        {

        }
        public void Invoke(ICommand cmd)
        {
            Console.WriteLine("Invoking.......");
            cmd.Execute();
        }
        
    }

    //Concrete    
public class Create : ICommand
    {
    
        Item invitem = new Item();
        public Create(string item, double cPrice , double sPrice)
        {
           
            invitem.pname = item;
            invitem.sellingPrice = sPrice;
            invitem.costPrice = cPrice;
        }
        public void Execute()
        {
            AddItems(invitem);
            
            Console.WriteLine(" Item created");
        }

        private void AddItems(Item item)
        {
            var filePath = @"C:\Users\hmanyala\source\repos\Inventory\data.json";
            // Read existing json data
            var jsonData = System.IO.File.ReadAllText(filePath);
            var pList = JsonConvert.DeserializeObject<List<Item>>(jsonData)
                                  ?? new List<Item>();

            pList.Add(item);
               
            //item.Add(new Item()
            //{
            //    Name =  "Person"
            //});

            // Update json data string
            jsonData = JsonConvert.SerializeObject(pList);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
    }

    public class Report : ICommand
    {
        Item invitem = new Item();
        public void Execute()
        { }
    }

    public class UpdateBuy : ICommand
    {
        Item invitem = new Item();
        public UpdateBuy(string pro, double qty)
        {
            invitem.pname = pro;
            invitem.quantity = qty;
        }
        public void Execute()
        {
             UpdateProduct(invitem);
            //var query = (from stud in _arrStudents
            //             where stud.Name == "BBB"
            //             select stud)
            //          .Update(st => { st.Standard = "0"; st.Status = "X"; });
            Console.WriteLine("Item Updated.");
        }

        private void UpdateProduct(Item item)
        {
            var filePath = @"C:\Users\hmanyala\source\repos\Inventory\data.json";
            // Read existing json data
            var json = System.IO.File.ReadAllText(filePath);
          
            //dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            var js = JsonConvert.DeserializeObject<List<Item>>(json);
            JArray jArray = JArray.Parse(json);
            var jObjects = jArray.ToObject<List<JObject>>();

            //var query = (from stud in  
            //             where stud.pname == "BBB"
            //             select stud)
            //         .Update(st => {  st.Status = "X"; });

            foreach (var obj in jObjects)                             
            {
                if (obj["pname"].ToString() == item.pname)
                {
                    var i = jObjects.IndexOf(obj);
                    obj["quantity"] = item.quantity.ToString();
                }
                //if (jObjects.IndexOf(obj) == 1)                       
                //{
                //    foreach (var prop in obj.Properties())            
                //    {
                //        if (prop.Name == "selected_status")           
                //            obj["selected_status"] = 0;               
                //    }
                //}
            }
            JArray outputArray = JArray.FromObject(jObjects);
            string output = outputArray.ToString();
           // string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);

        }

}

    public class Delete : ICommand
    {
        Item invitem = new Item();
        public void Execute()
        { }
    }

    //Request Obj
}

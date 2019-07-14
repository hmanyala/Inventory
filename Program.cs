using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;



namespace Inventory
{
    class Program
    {
        static void Main(string[] args)

        {
            while (true)
            {
                StockController sc = new StockController();
                Console.WriteLine("\n\n   -----------Inventory Management------------");
                Console.WriteLine("Choose Operation ");
                Console.WriteLine("\n 1. Add Item \n 2. Add Inventory Stock \n 3. Update Sold Stock \n" +
                    " 4. Update New Price \n 5. Delete Item\n 6. Inventory Report \n 7. Profit Report \n 8.Exit");
                string input = Console.ReadKey().KeyChar.ToString();
                string iparam = string.Empty;
                Item item = new Item();
                switch (input)
                {

                    case "1":
                        double cprice = 0.0, sprice = 0.0;
                        Console.WriteLine("\n\n Enter item");
                        string pname = Console.ReadLine();
                        Console.WriteLine("\n Enter Cost price");
                        iparam = Console.ReadLine();
        
                         if (ValidInput(iparam))
                        {
                            cprice = Convert.ToDouble(iparam);
                        }
                        else
                        {
                            Console.WriteLine("Input cannot be empty, Please enter valid input");
                        }
                        Console.WriteLine("\n Enter Selling price");
                        iparam = Console.ReadLine();                      
                        if(ValidInput(iparam))
                        {
                            sprice = Convert.ToDouble(iparam);
                            AddItem c = new AddItem(pname, sprice, cprice);
                            sc.Invoke(c);
                        }
                        else
                        {
                            Console.WriteLine("Input cannot be empty or Please enter valid input");                         
                        }

                        break;
                    case "2":
                        double qty = 0.0;
                        Console.WriteLine("\n Enter item");
                        string uname = Console.ReadLine();
                        Console.WriteLine("Enter Quantity");
                         iparam = Console.ReadLine();
                        if (ValidInput(iparam))
                        {
                             qty = Convert.ToDouble(iparam);
                            UpdateSoldQty ub = new UpdateSoldQty(uname, qty);
                            sc.Invoke(ub);
                        }
                        else
                        {
                            Console.WriteLine("Input cannot be empty or Please enter valid input");
                        }

                        break;
                  
                    case "3":
                        Console.WriteLine("\nEnter item");
                        string sname = Console.ReadLine();
                        Console.WriteLine("Enter Quantity");
                        iparam = Console.ReadLine();
                        if (ValidInput(iparam))
                        {
                            qty = Convert.ToDouble(iparam);
                            UpdatePurchasedQty ub = new UpdatePurchasedQty(sname, qty);
                            sc.Invoke(ub);
                        }
                        else
                        {
                            Console.WriteLine("Input cannot be empty or Please enter valid input");
                        }
                        break;
                    case "4":
                        Console.WriteLine("\nEnter item");
                        string nparam = Console.ReadLine();
                        Console.WriteLine("Enter New Price");
                        iparam = Console.ReadLine();
                        if (ValidInput(iparam))
                        {
                            var nprice = Convert.ToDouble(iparam);
                            UpdateSellingPrice usp = new UpdateSellingPrice(nparam, nprice);
                            sc.Invoke(usp);
                        }
                        else
                        {
                            Console.WriteLine("Input cannot be empty or Please enter valid input");
                        }
                        break;
                    case "5":
                        Console.WriteLine("\nEnter item");
                        string ubname = Console.ReadLine();

                        Delete d = new Delete(ubname);
                        sc.Invoke(d);
                        break;
                    case "6":
                        InventoryReport r = new InventoryReport();
                        sc.Invoke(r);
                        break;
                    case "7":
                        ProfitReport pr = new ProfitReport();
                        sc.Invoke(pr);
                        break;
                    case "8":
                        Environment.Exit(0);
                        break;
                    default:
                        {
                            Console.WriteLine("Operation not found");
                        }
                        break;
                }

            }
        }

        private static bool ValidInput(string iparam)
        {
      
            bool chkValue = Double.TryParse(iparam, out double cp);
            Math.Round(cp,2);
            return chkValue;
        }


    }


}

using System;

namespace Inventory
{
    class Program
    {
        static void Main(string[] args)

        {


           

            while (true)
            {
                StockController sc = new StockController();
                Console.WriteLine("\n\nInventory Management");
                Console.WriteLine("Choose Operation ");
                Console.WriteLine("\n 1. Add Item \n 2. Update Buy \n 3. Update Sell \n 4. Delete Item\n 5.Inventory Report \n 6.Exit");
                string input = Console.ReadKey().KeyChar.ToString();
                //Console.WriteLine("\n 1. Add Item \n 2. Update Buy \n 3. Update Sell \n 4. Delete \n 5.Inventory Report\n Exit \n\n");
                switch (input)
                {

                    case "1":
                        Console.WriteLine("\n\n Enter item");
                        string pname = Console.ReadLine();
                        Console.WriteLine("\n Enter Cost price");
                        double cprice = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("\n Enter Selling price");
                        double sprice = Convert.ToDouble(Console.ReadLine());
                        Create c = new Create(pname, cprice, sprice);
                        // Create c = new Create(item);
                        sc.Invoke(c);
                        break;
                    case "2":
                        Console.WriteLine("\n Enter item");
                        string uname = Console.ReadLine();
                        Console.WriteLine("Enter Quantity");
                        var qty = Convert.ToDouble(Console.ReadLine());

                        UpdateBuy ub = new UpdateBuy(uname, qty);
                        sc.Invoke(ub);
                        break;
                    case "4":
                        Console.WriteLine("\nEnter item");
                        string ubname = Console.ReadLine();

                        Delete d = new Delete(ubname);
                        sc.Invoke(d);
                        break;
                    case "3":
                        Console.WriteLine("\nEnter item");
                        string usName = Console.ReadLine();
                        Console.WriteLine("Enter Quantity");
                        double usQty = Convert.ToDouble(Console.ReadLine());

                        UpdateSell us = new UpdateSell(usName, usQty);
                        sc.Invoke(us);
                        break;
                    case "5":
                        Report r = new Report();
                        sc.Invoke(r);
                        break;
                    case "6":
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


        //create invoker object

        
    }


}

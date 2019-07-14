using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory
{
   

   public class StockController
    {
        //Invoker

        public void Invoke(ICommand cmd)
        {
            Console.WriteLine("\nInvoking.......");
            cmd.Process();
        }


    }
}

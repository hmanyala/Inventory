using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory
{
    public interface ICommand
    {
        void Execute();
        bool IsCompleted { get; set; }
    }

   public class StockController
    {
        //Invoker

        public void Invoke(ICommand cmd)
        {
            Console.WriteLine("Invoking.......");
            cmd.Execute();
        }


    }
}

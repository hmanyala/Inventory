using System;

namespace Inventory
{
    class Program
    {
        static void Main(string[] args)
        {
            Mobile m = new Mobile();
            Create c = new Create("Book1", 10.50, 13.79);
            m.Invoke(c);
            c = new Create("Food01", 1.47, 3.98);
            m.Invoke(c);
            UpdateBuy ub = new UpdateBuy("Book", 600.00);
            m.Invoke(ub);
   
           



        }


        //create invoker object

        
    }


}

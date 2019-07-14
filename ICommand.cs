using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory
{
    public interface ICommand
    {
        void Process();
       // bool IsCompleted { get; set; }
    }
}

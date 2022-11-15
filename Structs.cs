using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumePDF
{
    internal struct FibonacciHEaderState
    {
        public int Previous { get; set; }
        public int Current { get; set; }
    }

    internal struct OrdersTableState
    {
        public int ShownItemsCount { get; set; }
    }
}

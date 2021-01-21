using System;
using System.Collections.Generic;
using System.Text;

namespace Wriststone_Administration.DB.ApplicationTables
{
    public class OrderDetailsCase
    {
        public long Id { get; set; }
        public long Order { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
}

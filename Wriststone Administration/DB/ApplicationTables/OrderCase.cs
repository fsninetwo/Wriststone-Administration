using System;
using System.Collections.Generic;
using System.Text;

namespace Wriststone_Administration.DB.ApplicationTables
{
    public class OrderCase
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Account { get; set; }
        public string Payment { get; set; }
        public decimal Price { get; set; }
    }
}

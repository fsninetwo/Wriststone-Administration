using System;
using System.Collections.Generic;
using System.Text;

namespace Wriststone_Administration.DB.ApplicationTables
{
    public class RatingCase
    {
        public long? Id { get; set; }
        public int? Rate { get; set; }
        public string Message { get; set; }
        public string Product { get; set; }
        public string Account { get; set; }
    }
}

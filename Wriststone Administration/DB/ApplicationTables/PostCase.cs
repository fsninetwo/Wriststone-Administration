using System;
using System.Collections.Generic;
using System.Text;

namespace Wriststone_Administration.DB.ApplicationTables
{
    public class PostCase
    {
        public long Id { get; set; }
        public string Context { get; set; }
        public DateTime Created { get; set; }
        public string Thread { get; set; }
        public string Account { get; set; }
        public long? Status { get; set; }
    }
}

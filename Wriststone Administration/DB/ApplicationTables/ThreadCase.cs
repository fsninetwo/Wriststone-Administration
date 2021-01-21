using System;
using System.Collections.Generic;
using System.Text;

namespace Wriststone_Administration.DB.ApplicationTables
{
    public class ThreadCase
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public DateTime Created { get; set; }
        public string Account { get; set; }
        public string Category { get; set; }
        public long? Status { get; set; }
    }
}

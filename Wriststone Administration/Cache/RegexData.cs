using System;
using System.Collections.Generic;
using System.Text;

namespace Wriststone_Administration.Cache
{
    static class RegexData
    {
        public const string Password = @"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%%^&*()<>{}])[a-zA-Z0-9!@#$%^&*()<>]{8,20}$";
        public const string Email = @"^[\w]+@[\w]+\.[\w]+$";
    }
}

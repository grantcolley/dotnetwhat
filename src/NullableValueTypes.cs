using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetwhat.library
{
    public class NullableValueTypes
    {
        public void Test123()
        {
            int i = 5;
            int? j = null;
            j = 7;
            _ = j.HasValue;
            _ = j.Value;
        }
    }
}

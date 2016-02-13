using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriticalChainAddIn.Utils
{
    class Validation
    {

        public static int ValidateForNumericValue(string value)
        {
            if (value.IsEmpty()) return 0;
            if (value.IsNumeric()) return int.Parse(value);
            throw new BusinessException($"Value: '{value}' is not numeric");
        }
    }
}

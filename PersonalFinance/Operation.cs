using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public class Operation
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public OperationType OperationType { get; set; }
    }
}

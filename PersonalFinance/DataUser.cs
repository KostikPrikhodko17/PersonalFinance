using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public class DataUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<ModelTovar> HistoryTovars { get; set; }
        public List<Income> Incomes { get; set; } 
    }
}

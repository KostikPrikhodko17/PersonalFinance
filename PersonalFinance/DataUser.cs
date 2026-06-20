using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public class DataUser
    {
        public int Id;
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<ModelGood> HistoryGoods { get; set; }
    }
}

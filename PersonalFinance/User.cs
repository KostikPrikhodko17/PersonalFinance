using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public struct User
    {
        public string Name;
        public decimal Balance;
        public List<ModelGood> HistoryGoods;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance
{
    public struct DataUser
    {
        public string Name;
        public decimal Balance;
        public List<ModelGood> HistoryGoods;

        public DataUser(string Name, decimal Balance, List<ModelGood> HistoryGoods)
        {
            this.Name = Name;
            this.Balance = Balance;
            this.HistoryGoods = HistoryGoods;
        }
    }
}

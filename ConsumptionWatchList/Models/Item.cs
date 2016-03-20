using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SQLite.Net.Attributes;
//using SQLite.Net;

namespace ConsumptionWatchList.Models
{
    public class Item
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public float Amount { set; get; }
        public string Symbol { set; get; }
    }
}

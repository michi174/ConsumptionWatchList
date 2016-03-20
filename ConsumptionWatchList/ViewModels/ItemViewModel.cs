using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumptionWatchList.Models;

namespace ConsumptionWatchList.ViewModels
{
    public class ItemViewModel : BaseViewModel<Item>
    {
        public ItemViewModel(Item item = null) : base(item) { }

        public int ID
        {
            get { return This.ID; }
            set { SetProperty(This.ID, value, () => This.ID = value); }
        }

        public string Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }

        public float Amount
        {
            get { return This.Amount; }
            set { SetProperty(This.Amount, value, () => This.Amount = value); }
        }

        public string Symbol
        {
            get { return This.Symbol; }
            set { SetProperty(This.Symbol, value, () => This.Symbol = value); }
        }

    }
}

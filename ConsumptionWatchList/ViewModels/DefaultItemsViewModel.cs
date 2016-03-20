using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumptionWatchList.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ConsumptionWatchList.ViewModels
{
    public class DefaultItemsViewModel : BaseViewModel
    {
        DefaultItems defaultItems = new DefaultItems();
        public NotifyTaskCompletion<ObservableCollection<ItemViewModel>> TDefaultItems;
        int _SelectedIndex = -1;

        public DefaultItemsViewModel()
        {
            this.TDefaultItems = new NotifyTaskCompletion<ObservableCollection<ItemViewModel>>(this.LoadAllItemsAsync());
            this.Initial().ConfigureAwait(false);
        }

        private async Task<ObservableCollection<ItemViewModel>> LoadAllItemsAsync()
        {         
            var allItems = await defaultItems.LoadItemsAsync();
            Debug.WriteLine("...got the DB Items.");
            ObservableCollection<ItemViewModel> TAllItems = new ObservableCollection<ItemViewModel>();

            Debug.WriteLine("Loading Items to UI...");
            foreach (var item in allItems)
            {
                var newItem = new ItemViewModel(item);
                newItem.PropertyChanged += Item_OnNotifyPropertyChanged;
                TAllItems.Add(newItem);
                Debug.WriteLine("- Item "+ TAllItems.IndexOf(newItem) + " loaded on UI -");
            }
            Debug.WriteLine("...UI was Refreshed. Got all Items asyncronly!");

            return TAllItems;
        }

        private async Task Initial()
        {
            await this.TDefaultItems.Task;
            this.Items = this.TDefaultItems.Result;
        }


        ObservableCollection<ItemViewModel> _Items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { if(SetProperty(ref _SelectedIndex, value)) { RaisePropertyChanged(nameof(SelectedItem)); } }
        }

        public ItemViewModel SelectedItem
        {
            get { return (_SelectedIndex >= 0) ? _Items[_SelectedIndex] : null; }
        }

        public async Task Save()
        {
            
            var item = new ItemViewModel();
            item.Name = "Name";
            item.Amount = 1;
            item.Symbol = "☺";
            item.PropertyChanged += Item_OnNotifyPropertyChanged;
            await defaultItems.SaveAsync(item);
            Items.Add(item);
            SelectedIndex = Items.IndexOf(item);
            
        }

        public async Task UpdateAsync(ItemViewModel item)
        {
            ItemViewModel oldItem = this.Items.Where(c => c.ID == item.ID).Single();
            int oldIndex = this.Items.IndexOf(oldItem);

            await defaultItems.SaveAsync(item);
            this.Items[oldIndex] = item;
        }

        public async Task Delete()
        {
            if(SelectedIndex != -1)
            {
                var item = Items[SelectedIndex];
                await defaultItems.Delete(item);
                Items.RemoveAt(SelectedIndex);
            }
        }

        private async void Item_OnNotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           await defaultItems.SaveAsync((ItemViewModel)sender);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsumptionWatchList.Data;
using Microsoft.Data.Entity;
using System.Diagnostics;

namespace ConsumptionWatchList.Models
{
    public class DefaultItems
    {
        public List<Item> Items { set; get; }
        private ConsumptionWLContext DbEF = new ConsumptionWLContext();

        public DefaultItems()
        {

        }

        /// <summary>
        /// Simuliert eine langsame Datenbankabfrage. Im Produktivmodus deaktivieren!
        /// </summary>
        public async Task setDelay()
        {
            Debug.WriteLine("Starting download of items...");
            await Task.Delay(600);
            Debug.WriteLine("...loading complete.");
        }

        /// <summary>
        /// Lädt die alle Items asynchron aus der Datenbank und gib sie zurück.
        /// </summary>
        public async Task<List<Item>> LoadItemsAsync(int start = 0, int numItems = 10)
        {
            //Hier muss eine <List> zurückgegeben werden, damit sie nicht im Konstruktor initialisiert werden muss.
            //Das ViewModel muss die Liste per await abfragen und PropertyChanged() auslösen, damit das UI mitbekommt,
            //dass die Items jetzt geladen sind und zu Verfügung stehen.

            Task del = setDelay();
            List<Item> allItems = new List<Item>();

            while (!del.IsCompleted)
            {
                Debug.WriteLine("...still loading...");
                await Task.Delay(100);
            }

            allItems = await (from i in this.DbEF.DBItems
                              orderby i.Name
                              select i).Skip(start).Take(numItems).ToListAsync();

            Debug.WriteLine("...we need to refresh the UI - we got the items now, but the UI doesn't know it.");

            this.Items = allItems;

            return allItems;
        }

        public async Task<bool> SaveAsync(Item item)
        {
            bool result = false;
            var existingItem = this.DbEF.DBItems.Where(c => c.ID == item.ID).SingleOrDefault();

            try
            {
                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.Amount = item.Amount;
                    existingItem.Symbol = item.Symbol;

                    this.DbEF.Update(existingItem);
                    await this.DbEF.SaveChangesAsync();

                }
                else
                {
                    this.DbEF.Add(item);
                    await this.DbEF.SaveChangesAsync();
                    this.Items.Add(item);
                    System.Diagnostics.Debug.WriteLine("Item saved.");

                }
            }
            catch
            {
                result = false;
                System.Diagnostics.Debug.WriteLine("Item can't be saved.");
            }


            return result;
        }

        public async Task Delete(Item item)
        {
            try
            {
                if (this.Items.Contains(item))
                {
                    this.DbEF.Remove(item);
                    await this.DbEF.SaveChangesAsync();
                    this.Items.Remove(item);
                    System.Diagnostics.Debug.WriteLine("Item removed.");

                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Item can't be removed.");
            }

        }

        public async Task Update(Item item)
        {
            var existingItem = this.DbEF.DBItems.Where(c => c.ID == item.ID).SingleOrDefault();

            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Amount = item.Amount;
                existingItem.Symbol = item.Symbol;

                this.DbEF.Update(existingItem);
                await this.DbEF.SaveChangesAsync();

            }
        }
    }
}

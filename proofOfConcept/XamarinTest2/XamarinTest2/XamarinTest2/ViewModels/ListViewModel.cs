using XamarinTest2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTest2.ViewModels;
using XamarinTest2.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace XamarinTest2.ViewModels
{
    internal class ListViewModel
    {
        private readonly IDataStore<Item> store;
        private readonly ObservableCollection<Item> items;
        public ObservableCollection<Item> Items { get { return items; }}

        public ListViewModel()
        {
            store = new MockDataStore();
            items = new ObservableCollection<Item>();
            List<Item> list = (List<Item>)store.GetItemsAsync().Result;
            foreach (Item item in list)
            {
                Items.Add(item);
            }

            /*            getter = new ToDoListGetter("https://localhost:44381", ref items);
                        getter.StartConnectionAsync();*/
        }

        
    }
}

using XamarinTest2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XamarinTest2.ViewModels;
using XamarinTest2.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;

namespace XamarinTest2.ViewModels
{
    internal class GetListViewModel
    {


        public GetListViewModel()
        {
          //  store = new WebDataStore("http://coresqltester.azurewebsites.net/JsonTodos/");
            //items = new ObservableCollection<ToDo>();
/*            List<ToDo> list = (List<ToDo>)store.GetItemsAsync().Result;
            foreach(ToDo item in list)
            {
                Items.Add(item);
            }*/
        }


    }
}

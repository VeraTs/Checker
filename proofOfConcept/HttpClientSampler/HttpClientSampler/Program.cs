using HttpClientSampler.TDOs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientSampler
{
    public class Program
    {
        public static void Main()
        {
            // exmple of how a to do list will appear, property wise, and how it will be printed
            /*ToDoList toDos = new ToDoList();
            toDos.Add(new ToDo() { id = 0, createdDate = "1534T", description = "Tester1" });
            toDos.Add(new ToDo() { id = 1, createdDate = "1534T", description = "Tester2" });
            toDos.Add(new ToDo() { id = 2, createdDate = "1534T", description = "Tester3" });
            Console.WriteLine(toDos);
            String jsonStr = JsonSerializer.Serialize(toDos);
            Console.WriteLine(jsonStr);

            Console.WriteLine("End of demo, get actual list from server:\n\n");*/


            // using System.Text.Json
            // create client and send request
            ToDoHttpClient client = new ToDoHttpClient("http://coresqltester.azurewebsites.net/Todos/");
            String str = client.SendRequest().Result;
            
            // print response json
            Console.WriteLine(str);

            // generate the response into a ToDoList object (list of the DTO with prettier printing)
            ToDoList? list2 = JsonSerializer.Deserialize<ToDoList>(str);
            
            // print list
            Console.WriteLine(list2);

        }
    }
}



using HttpClientSampler.TDOs;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpClientSampler
{
    public class Program
    {
        public static void Main()
        {
/*            ToDoList toDos = new ToDoList();
            toDos.AddtoDo(new ToDo() { id = 0, createdDate = "1534T", description = "Tester1" });
            toDos.AddtoDo(new ToDo() { id = 1, createdDate = "1534T", description = "Tester2" });
            toDos.AddtoDo(new ToDo() { id = 2, createdDate = "1534T", description = "Tester3" });
            Console.WriteLine(toDos);
            String jsonStr = JsonSerializer.Serialize(toDos);
            Console.WriteLine(jsonStr);

*/

            // using System.Text.Json
            // create client and send request
            ToDoHttpClient client = new ToDoHttpClient("http://coresqltester.azurewebsites.net/Todos/");
            String str = client.SendRequest();
            
            // print response json
            Console.WriteLine(str);

            // generate the response into a ToDoList object (list of the DTO with prettier printing)
            ToDoList? list2 = JsonSerializer.Deserialize<ToDoList>(str);
            
            // print list
            Console.WriteLine(list2);

        }
    }
}



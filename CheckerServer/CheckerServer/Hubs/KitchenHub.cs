using System.Linq;
using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CheckerServer.Hubs
{
    public class KitchenHub : Hub
    {
        private readonly KitchenManager? r_Manager = null;
        private readonly CheckerDBContext _context;
        public IServiceProvider? Services { get; }

        // apparently, the ctor is called for each invocation. FFS WTF.
        // so everything here needs to be a reference to a static thing.
        public KitchenHub(IServiceProvider services, CheckerDBContext context)
        {
            // issues here - MAJOR. The context is refreshed at every call for the hub, 
            // and the previous context assigned to KitchenManager is disposed of
            // updating the context is needed

            //r_Manager = kitchenManager;
            Services = services;
            _context = context;

         //   useKitchenManager();
        }

        private object locker = new object();

        private async Task useKitchenManager()
        {

            //Dictionary<int, List<LineDTO>> lines = r_Manager.ManageKitchen();
            r_Manager.ManageKitchenAsync();
/*            foreach (var line in lines)
            {
                Restaurant? rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == line.Key);
                // not awaited since this pretains to different restaurants
                if (rest != null)
                    await Clients.Group(rest.Name).SendAsync("UpdatedLines", lines[rest.ID]);
            }*/

            /*System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 30000; // 50 sec for denugging purposes
            timer.Elapsed += async (o, e) => {
                
                r_Manager.UpdateContext(_context);


            };

            timer.Start();*/
        }

        // checks if orderITem is legitimate and moves it if it is
        // checks if orderITem is legitimate and moves it if it is
        public async Task MoveOrderItemToDoing(int id)
        {
            OrderItem? item = await _context.OrderItems.Include("Dish").FirstOrDefaultAsync(item => item.ID == id);
            if (item != null)
            {
                await moveFromListToList(item, eLineItemStatus.ToDo, eLineItemStatus.Doing, "ToDo", "Doing");
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "No such orderItem");
            }
        }

        // checks if orderITem is legitimate and moves it if it is
        public async Task MoveOrderItemToDone(int id)
        {
            OrderItem? item = await _context.OrderItems.Include("Dish").FirstOrDefaultAsync(item => item.ID == id);
            if (item != null)
            {
                await moveFromListToList(item, eLineItemStatus.Doing, eLineItemStatus.Done, "Doing", "Done");
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "No such orderItem");
            }
        }

        private async Task moveFromListToList(OrderItem item, eLineItemStatus prevStatus, eLineItemStatus nextStatus, string prevListName, string nextListName)
        {
            if (item.LineStatus == prevStatus)
            {
                // move to doing state and list
                item.LineStatus = nextStatus;
                if (Services != null && await _context.SaveChangesAsync() > 0)
                {
                    // yay - success
                    // now just update KitchenManager
                    KitchenManager manager = Services.GetService<KitchenManager>();
                    Order? order = await _context.Orders.FirstOrDefaultAsync(o => o.ID == item.OrderId);
                    manager.ItemWasMoved(order, item);
                    await Clients.Caller.SendAsync("ItemMoved", item);
                }
                else
                {
                    await Clients.Caller.SendAsync("DBError", "Error in updating DB, try again later");
                }
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "Can't move to " + nextListName + " List - it is not in " + prevListName + "list");
            }
        }

        // temporary group registration with rest name
        // in future will be with actual protection and privacy thing
        /******************* IMPORTANT - THIS IS NOT RECONNECT SAFE
         * 
         * meaning that if a client disconnects from the hub, on reconnecting they MUST REREGISTER WITH THE GROUP
         * 
         * ***************************/
        public async Task RegisterForGroup(int restId)
        {
            Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
            if (rest == null)
            {
                await Clients.Caller.SendAsync("DBError", "No such restaurant is registered");
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, rest.Name);
                var manager = Services.GetService<KitchenManager>();
                manager.AddGroupMember(restId);
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has joined the group {rest.Name}.");
            }

        }

        public async Task UnRegisterForGroup(int restId)
        {
            Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
            if (rest == null)
            {
                await Clients.Caller.SendAsync("DBError", "No such restaurant is registered");
            }
            else
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, rest.Name);
                
                //s_Manager.RemoveGroupMember(restId);
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has left the group {rest.Name}.");
            }
        }
    }
}

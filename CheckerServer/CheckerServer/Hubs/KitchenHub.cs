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
        private readonly CheckerDBContext _context;
        private readonly KitchenUtils r_KitchenUtils;

        public KitchenHub(CheckerDBContext context)
        {
            _context = context;
            r_KitchenUtils = new KitchenUtils(context);
            manageKitchen();
        }


        // amnage the kitchen utils
        private async Task manageKitchen()
        {

            // we're currently ignoring multiple restaurants, and assuming singal restaurant
            // for multiple restaurants, we will have multiple users, and in the SignalR case,
            // multiple Clients associated with the user, in a Group


            /**
             * How To manage a kitchen?
             *  1) Load everything (done in ctor!)
             *  2) while server operating:
             *      2.1) Check all open orders, and update the OrderITem lists for each Line
             *              \----- 'GetUpdatedLines' - will return a mapping restId -> {line | line in rest, with updated orderItem lists inside}
             *      2.2) For each line, send the Line and lists to each relevant restaurant
             *              \----- foreach(int key in keys) { await Clients.Group(...).SendAsync(event thing) }
             *      2.3) if there are new orders, sift them into the KitchenUtils
             *              \----- 'LoadNewOrders' with input of active orders from DB (actually, is input needed? KitchenUtils has context access)
             *      2.4) if there are closed orders, remove them from the KitchenUtils
             *              \----- 'ClearDeadOrders' with no input needed
             * 
             */

            // step 2.1
            // restId to Lines
            Dictionary<int, List<LineDTO>> updatedLines = r_KitchenUtils.GetUpdatedLines();

            // step 2.2
            foreach (int restId in updatedLines.Keys)
            {
                Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == restId);
                if (rest != null)
                {
                    // not awaited since this pretains to different restaurants
                    Clients.Group(rest.Name).SendAsync("UpdatedLines", updatedLines[rest.ID]);
                }
            }

            // step 2.3
            int success = r_KitchenUtils.LoadNewOrders();
            if (success > 0)
            {
                // yay, things loaded and hoarded - continue with my life, this will happen again, obvs.
            }
            else
            {
                // should I report a signalR error? And if so - to whom?
                await Clients.All.SendAsync("SignalRError", "Error in uploading new orders to the management system");
            }

            // step 2.4
            r_KitchenUtils.ClearOrders();
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
                await Clients.Group(rest.Name).SendAsync("NewGroupMember", $"{Context.ConnectionId} has left the group {rest.Name}.");
            }
        }
    }
}

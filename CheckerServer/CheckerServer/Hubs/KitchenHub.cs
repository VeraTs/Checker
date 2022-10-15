﻿using System.Linq;
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
        private readonly IHubContext<OrdersHub> ordersHubContext;
        public IServiceProvider? Services { get; }

        public KitchenHub(IServiceProvider services, CheckerDBContext context, IHubContext<OrdersHub> ordersHubContext)
        {
            Services = services;
            _context = context;
            this.ordersHubContext = ordersHubContext;
        }

        private object locker = new object();

        // checks if orderITem is legitimate and moves it if it is
        public async Task MoveOrderItemToDoing(int id)
        {
            OrderItem? item = await _context.OrderItems.Include("Dish").FirstOrDefaultAsync(item => item.ID == id);
            if (item != null)
            {
                item.Start = DateTime.Now;

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
                item.Finish = DateTime.Now;
                Boolean statSuccess = await StatisticUtils.updateDishStat(Services, item);

                Line line = await _context.Lines.FirstOrDefaultAsync(l => l.ID == item.Dish.LineId);
                Restaurant rest = await _context.Restaurants.FirstOrDefaultAsync(r => r.ID == line.RestaurantId);
                int spot = await itemToBeServed(item, rest);
                if(spot < 0)
                {
                    await Clients.Caller.SendAsync("CantPlaceItem", "No free spot found");
                }
                else
                {
                    await Clients.Caller.SendAsync("PlaceItem", item, spot);
                }
            }
            else
            {
                await Clients.Caller.SendAsync("DBError", "No such orderItem");
            }
        }

        internal async Task<int> itemToBeServed(OrderItem orderItem, Restaurant rest)
        {
            int spot = -1;
            try
            {
                if (orderItem != null)
                {
                    bool success = await OrdersUtils.prepareItemForServing(_context, orderItem);
                    if (success)
                    {
                        spot = orderItem.ServingAreaZone;
                        Line? line = await _context.Lines.FirstAsync(l => l.ID == orderItem.Dish.LineId);
                        if(line!= null)
                        {
                            await ordersHubContext.Clients.Group(rest.Name).SendAsync("ItemToBeServed", line.ServingAreaId, orderItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ordersHubContext.Clients.Group(rest.Name).SendAsync("SignalRError", "an unexpected error occured: " + ex.Message);
            }

            return spot;
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
                    RestaurantManager? manager = Services.GetService<RestaurantManager>();
                    Order? order = await _context.Orders.FirstOrDefaultAsync(o => o.ID == item.OrderId);
                    if (manager != null && order!= null)
                    {
                        KitchenManager kitchen = manager.getKitchenForRestaurant(order.RestaurantId);
                        kitchen.ItemWasMoved(order, item);
                        await Clients.Caller.SendAsync("ItemMoved", item);
                    }
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
                RestaurantManager manager = Services.GetService<RestaurantManager>();
                manager.getKitchenForRestaurant(restId).AddGroupMember(restId);
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

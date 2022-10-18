using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CheckerServer.utils
{
    public class OrdersUtils
    {
        private static Dictionary<int, Dictionary<int, Dictionary<int, OrderItem?>>> servingAreasPerRestaurant = new Dictionary<int, Dictionary<int, Dictionary<int,OrderItem?>>>();

        /*** orderitem state changes as needed and serving area state changes as needed ***/
        public static async Task<Boolean> prepareItemForServing(CheckerDBContext context, OrderItem item)
        {
            Boolean success = false;
            try
            {
                OrderItem orderitem = await context.OrderItems.FirstAsync(oi => oi.ID == item.ID);
                orderitem.Status = eItemStatus.WaitingToBeServed;
                Line line = await context.Lines.FirstOrDefaultAsync(l => l.ID == orderitem.Dish.LineId);
                ServingArea area = await context.ServingAreas.FirstOrDefaultAsync(sa => sa.ID == line.ServingAreaId);
                int spot = findSpotInServingArea(area);
                if (spot > -1)
                {
                    if (OrdersUtils.fillSpotInServingArea(area, item, spot))
                    {
                        item.ServingAreaZone = spot;
                        success = await context.SaveChangesAsync() > 0;
                    }
                }

                return success;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /*** manages the serving areas in the service ***/
        public static Boolean addServingArea(ServingArea area)
        {
            Boolean res = true;
            lock (servingAreasPerRestaurant)
            {
                if (!servingAreasPerRestaurant.ContainsKey(area.RestaurantId))
                {
                    servingAreasPerRestaurant.Add(area.RestaurantId, new Dictionary<int, Dictionary<int, OrderItem?>>());
                }
            }

            lock (servingAreasPerRestaurant[area.RestaurantId])
            {
                Dictionary<int, Dictionary<int, OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    Dictionary<int, OrderItem?> newArea = new Dictionary<int, OrderItem?>(area.ZoneNum);
                    for (int i = 0; i < area.ZoneNum; i++)
                    {
                        newArea.Add(i, null);
                    }

                    areas.Add(area.ID, newArea);
                }
            }

            return res;
        }

        public static Boolean removeServingArea(ServingArea area)
        {
            Boolean res = true;
            if (!servingAreasPerRestaurant.ContainsKey(area.RestaurantId))
            {
                res = false;
            }
            else
            {
                Dictionary<int, Dictionary<int, OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (!areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    areas.Remove(area.ID);
                }
            }

            return res;
        }

        /*** report a spot in a serving area is filled by an order item ***/
        public static Boolean fillSpotInServingArea(ServingArea area, OrderItem item, int spot)
        {
            Boolean res = true;
            if (!servingAreasPerRestaurant.ContainsKey(area.RestaurantId)) { res = false; }
            else
            {
                Dictionary<int, Dictionary<int, OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (!areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    lock (areas[area.ID])
                    {
                        if (areas[area.ID][spot] != null) { res = false; }
                        else
                        {
                            areas[area.ID][spot] = item;
                        }
                    }
                }
            }

            return res;
        }

        /*** report a spot in the serving area no longer contains an order item ***/
        public static Boolean freeSpot(ServingArea area, OrderItem item)
        {
            Boolean res = true;
            if (!servingAreasPerRestaurant.ContainsKey(area.RestaurantId)) { res = false; }
            else
            {
                Dictionary<int, Dictionary<int, OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (!areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    lock (areas[area.ID])
                    {
                        if (areas[area.ID][item.ServingAreaZone] == null) { res = false; }
                        else
                        {
                            areas[area.ID][item.ServingAreaZone] = null;
                        }
                    }
                }
            }

            return res;
        }

        /*** finds spot in serving area to place finished order item ***/
        public static int findSpotInServingArea(ServingArea area)
        {
            int spot = -1;
            if (servingAreasPerRestaurant.ContainsKey(area.RestaurantId))
            {
                Dictionary<int, Dictionary<int, OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (areas.ContainsKey(area.ID))
                {
                    lock (areas[area.ID])
                    {
                        if(areas[area.ID].Values.Any(val => object.Equals(val, null)))
                        {
                            // only assign spot if there is space in the serving area
                            KeyValuePair<int, OrderItem?> pair = areas[area.ID].First(zone => object.Equals(zone.Value, null));
                            spot = pair.Key;
                        }
                    }
                }
            }

            return spot;
        }

        private class ServingAreaSpots
        {
            List<OrderItem> itemsInSpots;
            public ServingAreaSpots(int capacity)
            {
                itemsInSpots = new List<OrderItem>(capacity);
            }
        }
    }
}
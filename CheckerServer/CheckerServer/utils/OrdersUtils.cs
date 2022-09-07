using CheckerServer.Models;
using System.Collections;

namespace CheckerServer.utils
{
    public class OrdersUtils
    {
        private static Dictionary<int, Dictionary<int, List<OrderItem?>>> servingAreasPerRestaurant = new Dictionary<int, Dictionary<int, List<OrderItem?>>>();

        public static Boolean addServingArea(ServingArea area)
        {
            Boolean res = true;
            if (!servingAreasPerRestaurant.ContainsKey(area.RestaurantId))
            {
                servingAreasPerRestaurant.Add(area.RestaurantId, new Dictionary<int, List<OrderItem?>>());
            }

            Dictionary<int, List<OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
            if (areas.ContainsKey(area.ID)) { res = false; }
            else
            {
                areas.Add(area.ID, new List<OrderItem?>(area.ZoneNum));
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
                Dictionary<int, List<OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (!areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    areas.Remove(area.ID);
                }
            }

            return res;
        }

        public static Boolean fillSpotInServingArea(ServingArea area, OrderItem item, int spot)
        {
            Boolean res = true;
            if(!servingAreasPerRestaurant.ContainsKey(area.RestaurantId)) { res = false; }
            else
            {
                Dictionary<int, List<OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (!areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    if(areas[area.ID][spot] != null) { res = false; }
                    else
                    {
                        areas[area.ID][spot] = item;
                    }
                }
            }

            return res;
        }

        public static Boolean freeSpot(ServingArea area, OrderItem item)
        {
            Boolean res = true;
            if (!servingAreasPerRestaurant.ContainsKey(area.RestaurantId)) { res = false; }
            else
            {
                Dictionary<int, List<OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (!areas.ContainsKey(area.ID)) { res = false; }
                else
                {
                    if (areas[area.ID][item.ServingAreaZone] == null) { res = false; }
                    else
                    {
                        areas[area.ID][item.ServingAreaZone] = null;
                    }
                }
            }

            return res;
        }

        public static int findSpotInServingArea(ServingArea area)
        {
            int spot = -1;
            if (servingAreasPerRestaurant.ContainsKey(area.RestaurantId))
            {
                Dictionary<int, List<OrderItem?>> areas = servingAreasPerRestaurant[area.RestaurantId];
                if (areas.ContainsKey(area.ID))
                {
                    spot = areas[area.ID].IndexOf(null);
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

        internal static bool freeSpot(int servingAreaId, OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}

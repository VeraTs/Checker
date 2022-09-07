using CheckerServer.Models;

namespace CheckerServer.utils
{
    public class ManagementUtils
    {
        private static Dictionary<String, Restaurant> userRestaurants = new Dictionary<string, Restaurant>();

        public static Boolean addUserRest(String userName, Restaurant rest)
        {
            Boolean res = true;
            if(string.IsNullOrEmpty(userName) || rest == null)
            {
                res = false;
            }
            else
            {
                if (userRestaurants.ContainsKey(userName))
                {
                    if(userRestaurants[userName].ID != rest.ID)
                    {
                        res = false;
                    }
                } else
                {
                    userRestaurants.Add(userName, rest);
                }
            }

            return res;
        }

        public static Restaurant? getUserRest(String userName)
        {
            Restaurant? rest = null;
            if (userRestaurants.ContainsKey(userName))
            {
                rest = userRestaurants[userName];
            }

            return rest;
        }

        public static Boolean hasUserRest(String userName)
        {
            return userRestaurants.ContainsKey(userName);
        }

        public static Boolean removeUserRest(String userName)
        {
            Boolean res = false;
            if(userRestaurants.ContainsKey(userName))
            {
                userRestaurants.Remove(userName);
                res = true;
            }

            return res;
        }
    }
}

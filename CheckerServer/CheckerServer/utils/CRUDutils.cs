using CheckerServer.Models;

namespace CheckerServer.utils
{
    public class CRUDutils
    {
        public static string onRejectFromDB(BaseDBItem item, string exceptionMsg="")
        {
            String msg = "";
            if (item.ID != 0)
            {
                msg = "You cannot decide an item id on your own";
            }
/*            else if(!string.IsNullOrEmpty(exceptionMsg))
            {
                msg = exceptionMsg;
            }*/
            else
            {
                msg = "Database Error has Occured. Make sure all input fields are valid.";
            }

            return msg;
        }
    }
}

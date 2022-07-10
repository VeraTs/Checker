namespace CheckerUI.Enums
{
    public enum eItemStatus      // the status of the order item overall
    {
        Ordered,                // initial state
        AtLine,                 // during entire time it is at perpetration line
        WaitingToBeServed,      // at serving zone
        Served,                 // served to table
        Returned                // customer returned item for some reason
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerServer.Models
{
    public class OrderItem : BaseDBItem
    {
        public virtual Dish? Dish { get; set; }

        [ForeignKey("Dish")]
        public int DishId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]
        public int ServingAreaZone { get; set; }
        
        public string Changes { get; set; }
        public eItemStatus Status { get; set; } = eItemStatus.Ordered;
        public eLineItemStatus LineStatus { get; set; } = eLineItemStatus.Locked;

        //statistics 
        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }

        public int dishCount(int i_Month)
        {
            if (DateTime.Now.Month != i_Month)
            {
                this.Dish.LastMonthSales = this.Dish.LastMonthSales;
                this.Dish.ThisMonthSales = 0;
                this.Dish.AvrageMonthSales = 0;
            }
            long elapsedTicks = Finish.Ticks - Start.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            double doneTime = elapsedSpan.TotalMinutes;
            this.Dish.AvrageMonthSales = (this.Dish.AvrageMonthSales * this.Dish.ThisMonthSales + (int)doneTime) / (this.Dish.ThisMonthSales + 1);
            return DateTime.Now.Month;
        }
    }

    public enum eItemStatus      // the status of the order item overall
    {
        Ordered,                // initial state
        AtLine,                 // during entire time it is at preperation line
        WaitingToBeServed,      // at serving zone
        Served,                 // served to table
        Returned                // customer returned item for some reason
    }

    public enum eLineItemStatus  // status of the order item in the line of preperation 
    {
        Locked,
        ToDo,
        Doing,
        Done,
        Rejected
    }
}

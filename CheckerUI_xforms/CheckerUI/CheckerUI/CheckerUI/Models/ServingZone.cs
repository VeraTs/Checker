using CheckerUI.ViewModels;

namespace CheckerUI.Models
{
    public class ServingZone
    {
        public int id { get; set; }
        public bool isAvailable { get; set; } = true;
        public OrderItemViewModel item { get; set; }
    }
}

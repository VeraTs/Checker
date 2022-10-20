namespace CheckerUI.Models
{
    public class Maker : BaseDBItem
    {
        public string name { get; set; }
        public int lineId { get; set; } // every maker lives in a line
    }
}

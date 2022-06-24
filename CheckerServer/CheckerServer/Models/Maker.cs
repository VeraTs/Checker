namespace CheckerServer.Models
{
    public class Maker : BaseDBItem
    {
        public string Name { get; set; }
        public int LineId { get; set; } // every maker lives in a line
    }
}

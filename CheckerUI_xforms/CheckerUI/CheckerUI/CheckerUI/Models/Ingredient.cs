namespace CheckerUI.Models
{
    public class Ingredient : BaseDBItem
    {
        public string name { get; set; }
        public int inStock { get; set; }
        public Measurement measurement { get; set; }
    }
}

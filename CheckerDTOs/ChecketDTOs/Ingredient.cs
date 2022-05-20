namespace CheckerDTOs
{
    public class Ingredient : BaseDBItem
    {
        public string Name { get; set; }
        public int InStock { get; set; }
        public Measurement Measurement { get; set; }
    }
}

namespace CheckerDTOs
{
    public class Dish : BaseDBItem
    {
        public string Name { get; set; }
        public int LineId { get; set; }
        public int RestMenuId { get; set; }

        public string Description { get; set; }
        public DishType Type { get; set; }
    }
}

namespace CheckerDTOs
{
    public class DishIngredient
    {
        public Ingredient Ingredient { get; set; }
        public Dish Dish { get; set; }
        public Measurement Measurement { get; set; }
        public int Amount { get; set; } = 0;
    }
}

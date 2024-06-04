namespace PortionWise.Models.Ingredient.DTOs
{
    public class CreateIngredientDTO
    {
        public required string Name { get; set; }
        public required Decimal Amount { get; set; }
        public required string Unit { get; set; }
    }
}

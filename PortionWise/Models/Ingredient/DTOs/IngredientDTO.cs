namespace PortionWise.Models.Ingredient.DTOs
{
    public class IngredientDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required string Unit { get; set; }
    }
}

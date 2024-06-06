using PortionWise.Models.Recipe.BO;


namespace PortionWise.Models.Ingredient.BOs
{
    public class IngredientBO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Amount { get; set; }
        public required string Unit { get; set; }

        public required Guid RecipeId { get; set; }
    }
}

using PortionWise.Models.Ingredient.BOs;


namespace PortionWise.Models.Recipe.BO
{
    public class RecipeBO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int portionSize { get; set; }
        public required string Instruction { get; set; }
        public List<IngredientBO>? Ingredients { get; set; }
    }
}

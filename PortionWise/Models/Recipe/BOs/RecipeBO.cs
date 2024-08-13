using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Nutrition.BOs;

namespace PortionWise.Models.Recipe.BO
{
    public class RecipeBO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PortionSize { get; set; }
        public required string Instruction { get; set; }
        public List<IngredientBO>? Ingredients { get; set; }
        public List<TotalNutritionBO>? NutritionInfo { get; set; }
    }
}

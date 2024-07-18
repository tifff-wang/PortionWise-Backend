using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Nutrition.DTOs;

namespace PortionWise.Models.Recipe.DTOs
{
    public class RecipeDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int portionSize { get; set; }
        public required string Instruction { get; set; }
        public required List<IngredientDTO> Ingredients { get; set; }
        public required List<TotalNutritionDTO> NutritionInfo { get; set; }
    }
}

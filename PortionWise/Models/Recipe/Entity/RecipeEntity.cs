using System.ComponentModel.DataAnnotations;
using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Nutrition.Entity;

namespace PortionWise.Models.Recipe.Entities
{
    public class RecipeEntity
    {
        [Key]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PortionSize { get; set; }
        public required string Instruction { get; set; }
        public List<IngredientEntity>? Ingredients { get; set; }
        public List<NutritionEntity>? NutritionInfo { get; set; }
    }
}

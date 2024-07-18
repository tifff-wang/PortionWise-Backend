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
        public int portionSize { get; set; }
        public required string Instruction { get; set; }
        public ICollection<IngredientEntity>? Ingredients { get; set; }
        public ICollection<NutritionEntity>? NutritionInfo { get; set; }
    }
}

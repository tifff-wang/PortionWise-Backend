using System.ComponentModel.DataAnnotations;
using PortionWise.Models.Recipe.Entities;


namespace PortionWise.Models.Ingredient.Entities
{
    public class IngredientEntity
    {
        [Key]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required Decimal Amount { get; set; }
        public required string Unit { get; set; }

        public required Guid RecipeId { get; set; }
        public required RecipeEntity Recipe { get; set; }
    }
}

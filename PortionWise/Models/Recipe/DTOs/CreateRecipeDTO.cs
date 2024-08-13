using PortionWise.Models.Ingredient.DTOs;


namespace PortionWise.Models.Recipe.DTOs
{
    public class CreateRecipeDTO
    {
        public required string Name { get; set; }
        public int PortionSize { get; set; }
        public required string Instruction { get; set; }
        public List<CreateIngredientDTO>? Ingredients { get; set; }
    }
}

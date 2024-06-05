using PortionWise.Models.Ingredient.DTOs;


namespace PortionWise.Models.Recipe.DTOs
{
    public class RecipeSummaryDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

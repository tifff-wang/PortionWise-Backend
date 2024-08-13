namespace PortionWise.Models.Recipe.DTOs
{
  public class UpdateRecipeDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public int PortionSize { get; set; }
        public required string Instruction { get; set; }
    }
}

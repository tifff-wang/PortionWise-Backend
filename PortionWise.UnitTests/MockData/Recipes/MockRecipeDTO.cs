using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Recipe.DTOs;

namespace PortionWise.UnitTests.MockData.Recipes
{
  public class MockRecipeDTO
    {
        private static Guid _id1 = Guid.NewGuid();
        private static Guid _id2 = Guid.NewGuid();

        public static List<RecipeDTO> CreateMockRecipeDTO()
        {
            return new List<RecipeDTO>
            {
                new RecipeDTO
                {
                    Id = _id1,
                    Name = "Banana Bread",
                    CreatedAt = DateTime.UtcNow,
                    PortionSize = 12,
                    Instruction = "Bake the banana cake",
                    Ingredients = new List<IngredientDTO>
                    {
                        new IngredientDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Banana",
                            Amount = 300,
                            Unit = "g",
                        },
                        new IngredientDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Butter",
                            Amount = 75,
                            Unit = "g",
                        }
                    }
                },
                new RecipeDTO
                {
                    Id = _id2,
                    Name = "Chocolate Cake",
                    CreatedAt = DateTime.UtcNow,
                    PortionSize = 8,
                    Instruction = "Bake chocolate cake",
                    Ingredients = new List<IngredientDTO>
                    {
                        new IngredientDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chocolate",
                            Amount = 100,
                            Unit = "g",
                        },
                        new IngredientDTO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Milk",
                            Amount = 100,
                            Unit = "g",
                        }
                    }
                }
            };
        }
    }
}

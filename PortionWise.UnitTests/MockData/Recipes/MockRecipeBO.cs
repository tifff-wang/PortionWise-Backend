using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Recipe.BO;

namespace PortionWise.UnitTests.MockData.Recipes
{
  public class MockRecipeBO
    {
        private static Guid _id1 = Guid.NewGuid();
        private static Guid _id2 = Guid.NewGuid();

        public static List<RecipeBO> CreateMockRecipeBO()
        {
            return new List<RecipeBO>
            {
                new RecipeBO
                {
                    Id = _id1,
                    Name = "Banana Bread",
                    CreatedAt = DateTime.UtcNow,
                    PortionSize = 12,
                    Instruction = "Bake the banana cake",
                    Ingredients = new List<IngredientBO>
                    {
                        new IngredientBO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Banana",
                            Amount = 300,
                            Unit = "g",
                            RecipeId = _id1,
                        },
                        new IngredientBO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Butter",
                            Amount = 75,
                            Unit = "g",
                            RecipeId = _id1,
                        }
                    }
                },
                new RecipeBO
                {
                    Id = _id2,
                    Name = "Chocolate Cake",
                    CreatedAt = DateTime.UtcNow,
                    PortionSize = 8,
                    Instruction = "Bake chocolate cake",
                    Ingredients = new List<IngredientBO>
                    {
                        new IngredientBO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Chocolate",
                            Amount = 100,
                            Unit = "g",
                            RecipeId = _id2,
                        },
                        new IngredientBO
                        {
                            Id = Guid.NewGuid(),
                            Name = "Milk",
                            Amount = 100,
                            Unit = "g",
                            RecipeId = _id2,
                        }
                    }
                },
            };
        }
    }
}

using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Recipe.Entities;

public static class MockRecipeEntity
{
    private static Guid _id1 = Guid.NewGuid();
    private static Guid _id2 = Guid.NewGuid();

    public static List<RecipeEntity> CreateMockRecipeEntity()
    {
        return new List<RecipeEntity>
        {
            new RecipeEntity
            {
                Id = _id1,
                Name = "Banana Bread",
                CreatedAt = DateTime.UtcNow,
                PortionSize = 12,
                Instruction = "Bake the banana cake",
                Ingredients = new List<IngredientEntity>
                {
                    new IngredientEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Banana",
                        Amount = 300,
                        Unit = "g",
                        RecipeId = _id1,
                    },
                    new IngredientEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Butter",
                        Amount = 75,
                        Unit = "g",
                        RecipeId = _id1,
                    }
                }
            },
            new RecipeEntity
            {
                Id = _id2,
                Name = "Chocolate Cake",
                CreatedAt = DateTime.UtcNow,
                PortionSize = 8,
                Instruction = "Bake chocolate cake",
                Ingredients = new List<IngredientEntity>
                {
                    new IngredientEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Chocolate",
                        Amount = 100,
                        Unit = "g",
                        RecipeId = _id2,
                    },
                    new IngredientEntity
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

    public static List<RecipeEntity> CreateMockSummariesEntity()
    {
        return new List<RecipeEntity>
        {
            new RecipeEntity
            {
                Id = _id1,
                Name = "Banana Bread",
                CreatedAt = DateTime.UtcNow,
                PortionSize = 12,
                Instruction = "",
            },
            new RecipeEntity
            {
                Id = _id2,
                Name = "Chocolate Cake",
                CreatedAt = DateTime.UtcNow,
                PortionSize = 8,
                Instruction = "",
            }
        };
    }
}

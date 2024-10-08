using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Nutrition.Entity;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.Entities;

public static class MockEntity
{
    private static Guid _id1 = Guid.NewGuid();
    private static Guid _id2 = Guid.NewGuid();

    public static List<RecipeEntity> CreateMockEntity()
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
                    },
                    new IngredientEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Flour",
                        Amount = 200,
                        Unit = "g",
                        RecipeId = _id1,
                    },
                    new IngredientEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Milk",
                        Amount = 100,
                        Unit = "g",
                        RecipeId = _id1,
                    }
                },
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
                },
                NutritionInfo = new List<NutritionEntity>
                {
                    new NutritionEntity
                    {
                        Id = Guid.NewGuid(),
                        SugarGram = 10.5,
                        FiberGram = 1.0,
                        ServingSize = 200,
                        SodiumMg = 200,
                        PotassiumMg = 150,
                        FatSaturatedGram = 2.5,
                        FatTotalGram = 5,
                        Calories = 180,
                        CholesterolMg = 30,
                        ProteinGram = 2.5,
                        CarbohydratesTotalGram = 27,
                        RecipeId = _id2
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

    public static List<RecipeBO> CreateMockSummariesBO()
    {
        return new List<RecipeBO>
        {
            new RecipeBO
            {
                Id = _id1,
                Name = "Banana Bread",
                CreatedAt = DateTime.UtcNow,
                PortionSize = 12,
                Instruction = "",
            },
            new RecipeBO
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

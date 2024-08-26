using AutoMapper;
using Moq;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Nutrition;
using PortionWise.Models.Nutrition.BOs;
using PortionWise.Models.Nutrition.DTOs;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Repositories;
using PortionWise.Services;
using PortionWise.UnitTests.MockData.Recipes;

namespace PortionWise.UnitTests.Services
{
    public class NutritionServiceTests
    {
        private readonly NutritionService _nutritionService;
        private readonly Mock<INutritionRepo> _mockNutritionRepo;
        private readonly Mock<IRecipeService> _mockRecipeService;
        private List<RecipeDTO> _mockData = MockDTO.CreateMockDTO();

        public NutritionServiceTests()
        {
            var nutritionProfile = new NutritionMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(nutritionProfile);
            });
            IMapper mapper = new Mapper(configuration);

            _mockNutritionRepo = new Mock<INutritionRepo>();
            _mockRecipeService = new Mock<IRecipeService>();
            _nutritionService = new NutritionService(
                _mockRecipeService.Object,
                _mockNutritionRepo.Object,
                mapper
            );
        }

        [Fact]
        public void IngredientsToString_ReturnValidString()
        {
            var result = _nutritionService.IngredientsToString(_mockData[0].Ingredients!);

            var expected = "300g Banana and 75g Butter";

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IngredientsToString_OneIngredient_ReturnStringWithoutAnd()
        {
            var ingredient = new List<IngredientDTO>
            {
                new IngredientDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Banana",
                    Amount = 300,
                    Unit = "g",
                },
            };
            var result = _nutritionService.IngredientsToString(ingredient);

            var expected = "300g Banana";

            Assert.Equal(expected, result);
        }

        [Fact]
        public async void GetRecipeNutrition_NoException_ReturnTotalNutritionDTO()
        {
            var recipeId = _mockData[0].Id;
            var ingredientString = "300g Banana and 75g Butter";
            var totalNutritonBO = new TotalNutritionBO
            {
                SugarGram = 10.5,
                FiberGram = 1.0,
                ServingSize = 375,
                SodiumMg = 200,
                PotassiumMg = 150,
                FatSaturatedGram = 2.5,
                FatTotalGram = 5,
                Calories = 180,
                CholesterolMg = 30,
                ProteinGram = 2.5,
                CarbohydratesTotalGram = 27,
            };

            _mockRecipeService
                .Setup(service => service.GetRecipeById(recipeId))
                .ReturnsAsync(_mockData[0]);
            _mockNutritionRepo
                .Setup(repo => repo.GetTotalNutritionInfo(ingredientString, recipeId))
                .ReturnsAsync(totalNutritonBO);

            var result = await _nutritionService.GetRecipeNutrition(recipeId);

            _mockNutritionRepo.Verify(
                repo => repo.GetTotalNutritionInfo(ingredientString, recipeId)
            );

            Assert.IsType<TotalNutritionDTO>(result);
        }

        [Fact]
        public async void GetRecipeNutrition_NoRecipe_ThrowException()
        {
            var recipeId = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.GetRecipeById(recipeId))
                .Returns(Task.FromResult((RecipeDTO)null));

            await Assert.ThrowsAsync<RecipeNotFoundException>(
                () => _nutritionService.GetRecipeNutrition(recipeId)
            );
        }

        [Fact]
        public async void GetRecipeNutrition_NoIngredients_ThrowException()
        {
            var recipe = _mockData[0];
            _mockRecipeService
                .Setup(service => service.GetRecipeById(recipe.Id))
                .ReturnsAsync(recipe);

            recipe.Ingredients = null;

            await Assert.ThrowsAsync<IngredientNotFoundException>(
                () => _nutritionService.GetRecipeNutrition(recipe.Id)
            );
        }
    }
}

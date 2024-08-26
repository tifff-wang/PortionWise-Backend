using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition.Entity;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.UnitTests.Database.DAO
{
  public class NutritionDAOTests
    {
        private readonly MockDBContext _mockContext;
        private readonly NutritionDAO _nutritionDAO;
        private readonly List<RecipeEntity> _mockEntityData = MockEntity.CreateMockEntity();

        public NutritionDAOTests()
        {
            _mockContext = new MockDBContext("nutritionDAOTests");
            _nutritionDAO = new NutritionDAO(_mockContext.Context);
            _mockContext.ClearTestingData();
        }

        [Fact]
        public async void GetNutritionByRecipeId_NutritionExists_ReturnNutrition()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var recipeId = _mockEntityData[1].Id;

            var nutrition = await _nutritionDAO.GetNutritionByRecipeId(recipeId);

            Assert.NotNull(nutrition);
            Assert.Equal(180, nutrition.Calories);
        }

        [Fact]
        public async void GetNutritionByRecipeId_NutritionNotExists_ThrowsException()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var recipeId = _mockEntityData[0].Id;

            await Assert.ThrowsAsync<NutritionInfoNotFoundException>(
                async () => await _nutritionDAO.GetNutritionByRecipeId(recipeId)
            );
        }

        [Fact]
        public async void InsertNutritiion_NoException_Return1()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var nutrition = new NutritionEntity
            {
                Id = Guid.NewGuid(),
                SugarGram = 14.5,
                FiberGram = 1.5,
                ServingSize = 60,
                SodiumMg = 200,
                PotassiumMg = 150,
                FatSaturatedGram = 2.5,
                FatTotalGram = 5,
                Calories = 180,
                CholesterolMg = 30,
                ProteinGram = 2.5,
                CarbohydratesTotalGram = 27,
                RecipeId = _mockEntityData[0].Id
            };

            var affectedRow = await _nutritionDAO.InsertNutritionInfo(nutrition);

            Assert.Equal(1, affectedRow);
            var existingNutrition = await _mockContext.Context.NutritionInfo.FindAsync(
                nutrition.Id
            );
            Assert.NotNull(existingNutrition);
        }

        [Fact]
        public async void DeleteNutritionInfoIfExist_NutritonExists_Return1()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var recipeId = _mockEntityData[1].Id;

            var affectedRow = await _nutritionDAO.DeleteNutritionInfoIfExist(recipeId);

            Assert.Equal(1, affectedRow);
        }

        [Fact]
        public async void DeleteNutritionInfoIfExist_NutritonNotExists_Return0()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var recipeId = _mockEntityData[0].Id;

            var affectedRow = await _nutritionDAO.DeleteNutritionInfoIfExist(recipeId);

            Assert.Equal(0, affectedRow);
        }
    }
}

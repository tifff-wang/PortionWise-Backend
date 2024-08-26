using AutoMapper;
using Moq;
using PortionWise.Api;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition;
using PortionWise.Models.Nutrition.BOs;
using PortionWise.Models.Nutrition.DLs;
using PortionWise.Models.Nutrition.Entity;
using PortionWise.Models.Recipe.Entities;
using PortionWise.Repositories;

namespace PortionWise.UnitTests.Repositories
{
  public class NutritionRepoTests
    {
        private readonly NutritionRepo _nutritionRepo;
        private readonly Mock<INutritionApi> _mockNutritionApi;
        private readonly Mock<INutritionDAO> _mockNutritionDAO;
        private readonly List<RecipeEntity> _mockEntityData = MockEntity.CreateMockEntity();
        private readonly NutritionDL _mockNutritionData = new NutritionDL
        {
            Items = new List<NutritionItem>
            {
                new NutritionItem
                {
                    Name = "Tomato",
                    SugarGram = 10.5,
                    FiberGram = 2.5,
                    ServingSize = 1,
                    SodiumMg = 150,
                    PotassiumMg = 300,
                    FatSaturatedGram = 5,
                    FatTotalGram = 15,
                    Calories = 200,
                    CholesterolMg = 5,
                    ProteinGram = 10,
                    CarbohydratesTotalGram = 20
                },
            }
        };

        public NutritionRepoTests()
        {
            var nutritionProfile = new NutritionMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(nutritionProfile);
            });
            IMapper mapper = new Mapper(configuration);

            _mockNutritionApi = new Mock<INutritionApi>();
            _mockNutritionDAO = new Mock<INutritionDAO>();
            _nutritionRepo = new NutritionRepo(
                _mockNutritionApi.Object,
                _mockNutritionDAO.Object,
                mapper
            );
        }

        [Fact]
        public async void GetTotalNutritionInfo_NutritionNotFound_LoadsFromApiAndCaches()
        {
            var recipeId = _mockEntityData[0].Id;
            var query = "banana";

            _mockNutritionDAO
                .Setup(dao => dao.GetNutritionByRecipeId(recipeId))
                .ThrowsAsync(new NutritionInfoNotFoundException());
            _mockNutritionApi
                .Setup(api => api.GetNutritionInfo(query))
                .ReturnsAsync(_mockNutritionData);

            var result = await _nutritionRepo.GetTotalNutritionInfo(query, recipeId);

            _mockNutritionDAO.Verify(dao => dao.InsertNutritionInfo(It.IsAny<NutritionEntity>()));
        }

        [Fact]
        public async Task GetTotalNutritionInfo_ValidNutrition_ReturnsNutritionBO()
        {
            var recipeId = _mockEntityData[1].Id;
            var query = "banana";
            _mockNutritionDAO
                .Setup(dao => dao.GetNutritionByRecipeId(recipeId))
                .ReturnsAsync(_mockEntityData[1].NutritionInfo!.First());

            var result = await _nutritionRepo.GetTotalNutritionInfo(query, recipeId);

            Assert.NotNull(result);
            Assert.IsType<TotalNutritionBO>(result);
        }

        [Fact]
        public async Task GetTotalNutritionInfo_ExpiredNutrition_DeletesOldAndCachesNew()
        {
            var recipeId = _mockEntityData[1].Id;
            var query = "banana";
            var nutritionInDB = _mockEntityData[1].NutritionInfo!.First();
            nutritionInDB.CacheExpirationTime = DateTime.UtcNow.AddHours(-1);
            _mockNutritionDAO
                .Setup(dao => dao.GetNutritionByRecipeId(recipeId))
                .ReturnsAsync(nutritionInDB);
            _mockNutritionDAO
                .Setup(dao => dao.DeleteNutritionInfoIfExist(recipeId))
                .Returns(Task.FromResult(1));
            _mockNutritionApi
                .Setup(api => api.GetNutritionInfo(query))
                .ReturnsAsync(_mockNutritionData);

            var result = await _nutritionRepo.GetTotalNutritionInfo(query, recipeId);

            _mockNutritionDAO.Verify(dao => dao.InsertNutritionInfo(It.IsAny<NutritionEntity>()));
            Assert.IsType<TotalNutritionBO>(result);
        }

        [Fact]
        public async void CacheNutritionInfoToDB_DAOInsertNutritionCalled()
        {
            var recipeId = _mockEntityData[0].Id;
            _mockNutritionDAO
                .Setup(dao => dao.InsertNutritionInfo(It.IsAny<NutritionEntity>()))
                .Returns(Task.FromResult(1));

            var totalNutrition = new TotalNutritionDL
            {
                SugarGram = 30.5,
                FiberGram = 5.5,
                ServingSize = 3,
                SodiumMg = 250.0,
                PotassiumMg = 500.0,
                FatSaturatedGram = 12.0,
                FatTotalGram = 35.0,
                Calories = 450.0,
                CholesterolMg = 7.0,
                ProteinGram = 25.0,
                CarbohydratesTotalGram = 50.0
            };

            await _nutritionRepo.CacheNutritionInfoToDB(totalNutrition, recipeId);

            _mockNutritionDAO.Verify(dao => dao.InsertNutritionInfo(It.IsAny<NutritionEntity>()));
        }

        [Fact]
        public void SumNutritionInfo_WithValidData_CalculatesCorrectTotals()
        {
            var items = new List<NutritionItem>
            {
                new NutritionItem
                {
                    Name = "Tomato",
                    SugarGram = 10.5,
                    FiberGram = 2.5,
                    ServingSize = 1,
                    SodiumMg = 150,
                    PotassiumMg = 300,
                    FatSaturatedGram = 5,
                    FatTotalGram = 15,
                    Calories = 200,
                    CholesterolMg = 5,
                    ProteinGram = 10,
                    CarbohydratesTotalGram = 20
                },
                new NutritionItem
                {
                    Name = "Flour",
                    SugarGram = 20,
                    FiberGram = 3,
                    ServingSize = 2,
                    SodiumMg = 100,
                    PotassiumMg = 200,
                    FatSaturatedGram = 7,
                    FatTotalGram = 20,
                    Calories = 250,
                    CholesterolMg = 2,
                    ProteinGram = 15,
                    CarbohydratesTotalGram = 30
                }
            };

            var nutritionData = new NutritionDL { Items = items };
            var expectedTotal = new TotalNutritionDL
            {
                SugarGram = 30.5,
                FiberGram = 5.5,
                ServingSize = 3,
                SodiumMg = 250.0,
                PotassiumMg = 500.0,
                FatSaturatedGram = 12.0,
                FatTotalGram = 35.0,
                Calories = 450.0,
                CholesterolMg = 7.0,
                ProteinGram = 25.0,
                CarbohydratesTotalGram = 50.0
            };

            var result = nutritionData.SumNutritionInfo();

            Assert.Equal(expectedTotal.SugarGram, result.SugarGram);
            Assert.Equal(expectedTotal.FiberGram, result.FiberGram);
            Assert.Equal(expectedTotal.ServingSize, result.ServingSize);
            Assert.Equal(expectedTotal.FatTotalGram, result.FatTotalGram);
            Assert.Equal(expectedTotal.Calories, result.Calories);
        }

        [Fact]
        public void SumNutritionInfo_WithOneItem_ReturnsTheItem()
        {
            var result = _mockNutritionData.SumNutritionInfo();

            var expectedTotal = new TotalNutritionDL
            {
                SugarGram = 10.5,
                FiberGram = 2.5,
                ServingSize = 1,
                SodiumMg = 150,
                PotassiumMg = 300,
                FatSaturatedGram = 5,
                FatTotalGram = 15,
                Calories = 200,
                CholesterolMg = 5,
                ProteinGram = 10,
                CarbohydratesTotalGram = 20
            };

            Assert.Equal(expectedTotal.SugarGram, result.SugarGram);
            Assert.Equal(expectedTotal.FiberGram, result.FiberGram);
            Assert.Equal(expectedTotal.ServingSize, result.ServingSize);
            Assert.Equal(expectedTotal.FatTotalGram, result.FatTotalGram);
            Assert.Equal(expectedTotal.Calories, result.Calories);
        }
    }
}

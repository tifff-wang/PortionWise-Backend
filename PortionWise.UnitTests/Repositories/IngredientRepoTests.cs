using AutoMapper;
using Moq;
using PortionWise.Database.DAOs.Ingredient;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Ingredient;
using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Recipe.Entities;
using PortionWise.Repositories;

namespace PortionWise.UnitTests.Repositories
{
  public class IngredientRepoTests
    {
        private readonly IngredientRepo _ingredientRepo;
        private readonly Mock<IIngredientDAO> _mockIngredientDAO;
        private readonly Mock<INutritionDAO> _mockNutritionDAO;
        private readonly List<RecipeEntity> _mockEntityData = MockEntity.CreateMockEntity();

        public IngredientRepoTests()
        {
            var ingredientProfile = new IngredientMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(ingredientProfile);
            });
            IMapper mapper = new Mapper(configuration);

            _mockIngredientDAO = new Mock<IIngredientDAO>();
            _mockNutritionDAO = new Mock<INutritionDAO>();
            _ingredientRepo = new IngredientRepo(
                _mockIngredientDAO.Object,
                _mockNutritionDAO.Object,
                mapper
            );
        }

        [Fact]
        public async void GetPopularIngredientNames_ReturnNameList()
        {
            var mockSummariesData = MockEntity.CreateMockSummariesEntity();

            _mockIngredientDAO
                .Setup(DAO => DAO.GetPopularIngredientNames(2))
                .ReturnsAsync(new List<string> { "Milk", "Flour" });

            var popularIngredients = await _ingredientRepo.GetPopularIngredientNames(2);

            Assert.Equal(2, popularIngredients.Count());
            Assert.IsType<List<PopularIngredientsBO>>(popularIngredients);
        }

        [Fact]
        public async void GetIngredientById_RecipeExists_ReturnIngredient()
        {
            var id = new Guid();
            _mockIngredientDAO
                .Setup(DAO => DAO.GetIngredientById(id))
                .Returns(Task.FromResult(_mockEntityData[0].Ingredients!.First()));

            var ingredient = await _ingredientRepo.GetIngredientById(id);

            Assert.NotNull(ingredient);
            Assert.Equal("Banana", ingredient.Name);
        }

        [Fact]
        public async void CreateIngredient_Return1()
        {
            _mockIngredientDAO
                .Setup(DAO => DAO.InsertIngredient(It.IsAny<IngredientEntity>()))
                .Returns(Task.FromResult(1));

            var ingredient = new IngredientBO
            {
                Id = Guid.NewGuid(),
                Name = "Sugar",
                Amount = 50,
                Unit = "g",
                RecipeId = _mockEntityData[0].Id
            };

            var affectedRow = await _ingredientRepo.CreateIngredient(ingredient);

            Assert.Equal(1, affectedRow);
        }

        [Fact]
        public async void DeleteIngredientForId_DAODeleteIngredientShouldBeCalled()
        {
            var ingredient = _mockEntityData[0].Ingredients!.First();
            var id = ingredient.Id;
            _mockIngredientDAO
                .Setup(DAO => DAO.GetIngredientById(id))
                .Returns(Task.FromResult(ingredient));
            _mockIngredientDAO.Setup(DAO => DAO.DeleteIngredient(id)).Returns(Task.FromResult(1));
            _mockNutritionDAO
                .Setup(DAO => DAO.DeleteNutritionInfoIfExist(ingredient.RecipeId))
                .Returns(Task.FromResult(1));

            await _ingredientRepo.DeleteIngredient(id);

            _mockIngredientDAO.Verify(DAO => DAO.GetIngredientById(id));
            _mockIngredientDAO.Verify(DAO => DAO.DeleteIngredient(id));
            _mockNutritionDAO.Verify(DAO => DAO.DeleteNutritionInfoIfExist(ingredient.RecipeId));
        }

        [Fact]
        public async void UpdateIngredient_DAOUpdateIngredientShouldBeCalled()
        {
            var ingredient = _mockEntityData[0].Ingredients!.First();
            var id = ingredient.Id;
            var ingredientBO = new UpdateIngredientBO
            {
                Id = ingredient.Id,
                Name = "Sugar",
                Amount = 50,
                Unit = "g"
            };

            _mockIngredientDAO
                .Setup(DAO => DAO.GetIngredientById(id))
                .Returns(Task.FromResult(ingredient));
            _mockNutritionDAO
                .Setup(DAO => DAO.DeleteNutritionInfoIfExist(ingredient.RecipeId))
                .Returns(Task.FromResult(1));
            _mockIngredientDAO
                .Setup(DAO => DAO.UpdateIngredient(It.IsAny<IngredientEntity>()))
                .Returns(Task.FromResult(1));

            await _ingredientRepo.UpdateIngredient(ingredientBO);

            _mockIngredientDAO.Verify(DAO => DAO.UpdateIngredient(It.IsAny<IngredientEntity>()));
            _mockIngredientDAO.Verify(DAO => DAO.GetIngredientById(id));
            _mockNutritionDAO.Verify(DAO => DAO.DeleteNutritionInfoIfExist(ingredient.RecipeId));
        }
    }
}

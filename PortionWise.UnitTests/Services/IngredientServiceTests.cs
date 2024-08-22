using AutoMapper;
using Moq;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient;
using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Recipe.BO;
using PortionWise.Repositories;
using PortionWise.Services;
using PortionWise.UnitTests.MockData.Recipes;

namespace PortionWise.UnitTests.Services
{
  public class IngredientServiceTests
    {
        private readonly IngredientService _ingredientService;
        private readonly Mock<IIngredientRepo> _mockIngredientRepo;
        private readonly List<RecipeBO> _mockBOData = MockBO.CreateMockBO();

        public IngredientServiceTests()
        {
            var ingredientProfile = new IngredientMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(ingredientProfile);
            });
            IMapper mapper = new Mapper(configuration);

            _mockIngredientRepo = new Mock<IIngredientRepo>();
            _ingredientService = new IngredientService(_mockIngredientRepo.Object, mapper);
        }

        [Fact]
        public async void GetPopularIngredientNames_ReturnPopularIngredientDTO()
        {
            var popularIngredientBO = MockBO.CreateMockPopularIngredients();
            _mockIngredientRepo
                .Setup(repo => repo.GetPopularIngredientNames(2))
                .Returns(Task.FromResult(popularIngredientBO));

            var popularIngredients = await _ingredientService.GetPopularIngredientNames(2);

            Assert.Equal(2, popularIngredients.Count());
            Assert.Equal("Milk", popularIngredients.First().Name);
            Assert.IsAssignableFrom<IEnumerable<PopularIngredientDTO>>(popularIngredients);
        }

        [Fact]
        public async void GetPopularIngredientNames_CountIs0_ThrowException()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _ingredientService.GetPopularIngredientNames(0)
            );
        }

        [Fact]
        public async void GetIngredientById_ReturnIngredientDTO()
        {
            var ingredient = _mockBOData[0].Ingredients!.First();
            var id = ingredient.Id;
            _mockIngredientRepo
                .Setup(repo => repo.GetIngredientById(id))
                .Returns(Task.FromResult(ingredient));

            var result = await _ingredientService.GetIngredientById(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Banana", result.Name);
        }

        [Fact]
        public async void GetIngredientById_MissingId_ThrowException()
        {
            var id = Guid.Empty;

            await Assert.ThrowsAsync<IngredientMissingIdException>(
                async () => await _ingredientService.GetIngredientById(id)
            );
        }

        [Fact]
        public async void CreateRecipe_NoException()
        {
            _mockIngredientRepo
                .Setup(repo => repo.CreateIngredient(It.IsAny<IngredientBO>()))
                .Returns(Task.FromResult(1));

            var ingredient = new CreateIngredientDTO
            {
                Name = "Strawberry",
                Amount = 300,
                Unit = "g",
                RecipeId = _mockBOData[0].Id
            };

            await _ingredientService.CreateIngredient(ingredient);

            _mockIngredientRepo.Verify(repo => repo.CreateIngredient(It.IsAny<IngredientBO>()));
        }

        [Fact]
        public async void CreateIngredient_MissingName_ThrowException()
        {
            var ingredient = new CreateIngredientDTO
            {
                Name = "",
                Amount = 300,
                Unit = "g",
                RecipeId = _mockBOData[0].Id
            };

            await Assert.ThrowsAsync<IngredientMissingNameException>(
                async () => await _ingredientService.CreateIngredient(ingredient)
            );
        }

        [Fact]
        public async void reateIngredient_InvalidAmount_ThrowException()
        {
            var ingredient = new CreateIngredientDTO
            {
                Name = "Strawberry",
                Amount = 0,
                Unit = "g",
                RecipeId = _mockBOData[0].Id
            };

            await Assert.ThrowsAsync<IngredientInvalidAmountException>(
                async () => await _ingredientService.CreateIngredient(ingredient)
            );
        }

        [Fact]
        public async void DeleteIngredientForId_NoException()
        {
            var id = Guid.NewGuid();
            _mockIngredientRepo
                .Setup(repo => repo.DeleteIngredient(id))
                .Returns(Task.CompletedTask);

            await _ingredientService.DeleteIngredient(id);

            _mockIngredientRepo.Verify(repo => repo.DeleteIngredient(id));
        }

        [Fact]
        public async void DeleteIngredientForId_MissingId_ThrowException()
        {
            var id = Guid.Empty;

            await Assert.ThrowsAsync<IngredientMissingIdException>(
                async () => await _ingredientService.DeleteIngredient(id)
            );
        }

        [Fact]
        public async void UpdateRecipe_NoException()
        {
            _mockIngredientRepo
                .Setup(repo => repo.UpdateIngredient(It.IsAny<UpdateIngredientBO>()))
                .Returns(Task.CompletedTask);

            var ingredientDTO = new IngredientDTO
            {
                Id = Guid.NewGuid(),
                Name = "Banana",
                Amount = 300,
                Unit = "g",
            };
            await _ingredientService.UpdateIngredient(ingredientDTO);

            _mockIngredientRepo.Verify(
                repo => repo.UpdateIngredient(It.IsAny<UpdateIngredientBO>())
            );
        }
    }
}

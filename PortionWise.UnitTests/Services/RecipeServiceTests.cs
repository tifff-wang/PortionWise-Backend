using AutoMapper;
using Moq;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient;
using PortionWise.Models.Recipe;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.BOs;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Repository;
using PortionWise.Services;
using PortionWise.UnitTests.MockData.Recipes;

namespace PortionWise.UnitTests.Services
{
    public class RecipeServiceTests
    {
        private readonly RecipeService _recipeService;
        private readonly Mock<IRecipeRepo> _mockRecipeRepo;
        private readonly List<RecipeBO> _mockRecipeBO = MockRecipeBO.CreateMockRecipeBO();

        public RecipeServiceTests()
        {
            var recipeProfile = new RecipeMapping();
            var ingredientProfile = new IngredientMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(recipeProfile);
                cfg.AddProfile(ingredientProfile);
            });
            IMapper mapper = new Mapper(configuration);

            _mockRecipeRepo = new Mock<IRecipeRepo>();
            _recipeService = new RecipeService(_mockRecipeRepo.Object, mapper);
        }

        [Fact]
        public async void GetAllRecipeSummaries_ReturnRecipeSummariesDTO()
        {
            var summariesBO = MockEntity.CreateMockSummariesBO();
            _mockRecipeRepo
                .Setup(repo => repo.GetAllRecipeSummaries())
                .Returns(Task.FromResult(summariesBO));

            var recipeSummaries = await _recipeService.GetAllRecipeSummaries();

            Assert.Equal(2, recipeSummaries.Count());
            Assert.Equal("Banana Bread", recipeSummaries.First().Name);
            Assert.IsAssignableFrom<IEnumerable<RecipeSummaryDTO>>(recipeSummaries);
        }

        [Fact]
        public async void GetRecipeById_ReturnRecipeDTO()
        {
            var id = _mockRecipeBO[0].Id;
            _mockRecipeRepo
                .Setup(repo => repo.GetRecipeById(id))
                .Returns(Task.FromResult(_mockRecipeBO[0]));

            var recipe = await _recipeService.GetRecipeById(id);

            Assert.NotNull(recipe);
            Assert.Equal(id, recipe.Id);
            Assert.Equal("Banana Bread", recipe.Name);
        }

        [Fact]
        public async void GetRecipeById_MissingId_ThrowException()
        {
            var id = Guid.Empty;

            await Assert.ThrowsAsync<RecipeMissingIdException>(
                async () => await _recipeService.GetRecipeById(id)
            );
        }

        [Fact]
        public async void CreateRecipe_NoException()
        {
            _mockRecipeRepo
                .Setup(repo => repo.CreateRecipe(It.IsAny<RecipeBO>()))
                .Returns(Task.CompletedTask);

            var recipe = new CreateRecipeDTO
            {
                Name = "Strawberry cake",
                PortionSize = 6,
                Instruction = "pick some strawberries"
            };

            await _recipeService.CreateRecipe(recipe);

            _mockRecipeRepo.Verify(repo => repo.CreateRecipe(It.IsAny<RecipeBO>()));
        }

        [Fact]
        public async void CreateRecipe_MissingName_ThrowException()
        {
            var recipe = new CreateRecipeDTO
            {
                Name = "",
                PortionSize = 6,
                Instruction = "pick some strawberries"
            };

            await Assert.ThrowsAsync<RecipeMissingNameException>(
                async () => await _recipeService.CreateRecipe(recipe)
            );
        }

        [Fact]
        public async void CreateRecipe_InvalidPortionSize_ThrowException()
        {
            var recipe = new CreateRecipeDTO
            {
                Name = "Strawberry cake",
                PortionSize = 0,
                Instruction = "pick some strawberries"
            };

            await Assert.ThrowsAsync<RecipeInvalidPortionSizeException>(
                async () => await _recipeService.CreateRecipe(recipe)
            );
        }

        [Fact]
        public async void DeleteRecipeForId_NoException()
        {
            var id = Guid.NewGuid();
            _mockRecipeRepo.Setup(repo => repo.DeleteRecipeForId(id)).Returns(Task.CompletedTask);

            await _recipeService.DeleteRecipeForId(id);

            _mockRecipeRepo.Verify(repo => repo.DeleteRecipeForId(id));
        }

        [Fact]
        public async void DeleteRecipeForId_MissingId_ThrowException()
        {
            var id = Guid.Empty;

            await Assert.ThrowsAsync<RecipeMissingIdException>(
                async () => await _recipeService.DeleteRecipeForId(id)
            );
        }

        [Fact]
        public async void UpdateRecipe_NoException()
        {
            _mockRecipeRepo
                .Setup(repo => repo.UpdateRecipe(It.IsAny<UpdateRecipeBO>()))
                .Returns(Task.CompletedTask);

            var recipe = new UpdateRecipeDTO
            {
                Id = new Guid(),
                Name = "Tiramisu",
                PortionSize = 4,
                Instruction = "bake, bake, bake",
            };
            await _recipeService.UpdateRecipe(recipe);

            _mockRecipeRepo.Verify(repo => repo.UpdateRecipe(It.IsAny<UpdateRecipeBO>()));
        }
    }
}

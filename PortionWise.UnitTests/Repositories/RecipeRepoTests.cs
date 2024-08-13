using AutoMapper;
using Moq;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Ingredient;
using PortionWise.Models.Recipe;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.BOs;
using PortionWise.Models.Recipe.Entities;
using PortionWise.Repository;

namespace PortionWise.UnitTests.Repositories
{
  public class RecipeRepoTests
    {
        private readonly RecipeRepo _recipeRepo;
        private readonly Mock<IRecipeDAO> _mockRecipeDAO;
        private readonly List<RecipeEntity> _mockRecipeEntityData =
            MockRecipeEntity.CreateMockRecipeEntity();
       
        public RecipeRepoTests()
        {
            var recipeProfile = new RecipeMapping();
            var ingredientProfile = new IngredientMapping();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(recipeProfile);
                cfg.AddProfile(ingredientProfile);
            });
            IMapper mapper = new Mapper(configuration);

            _mockRecipeDAO = new Mock<IRecipeDAO>();
            _recipeRepo = new RecipeRepo(_mockRecipeDAO.Object, mapper);
        }

        [Fact]
        public async void GetAllRecipeSummaries_ReturnCorrectSummaries()
        {
           var mockSummariesData =
            MockRecipeEntity.CreateMockSummariesEntity();

            _mockRecipeDAO
                .Setup(DAO => DAO.GetAllRecipeSummaries())
                .Returns(Task.FromResult(mockSummariesData));

            var recipeSummaries = await _recipeRepo.GetAllRecipeSummaries();

            Assert.Equal(2, recipeSummaries.Count());
            Assert.All(
                recipeSummaries,
                recipe => Assert.True(string.IsNullOrEmpty(recipe.Instruction))
            );
        }

        [Fact]
        public async void GetRecipeById_RecipeExists_ReturnRecipe()
        {
            var id = new Guid();
            _mockRecipeDAO
                .Setup(DAO => DAO.GetRecipeById(id))
                .Returns(Task.FromResult(_mockRecipeEntityData[0]));

            var recipe = await _recipeRepo.GetRecipeById(id);

            Assert.NotNull(recipe);
            Assert.Equal("Banana Bread", recipe.Name);
        }

        [Fact]
        public async void CreateRecipe_DAOInsertRecipeShouldBeCalled()
        {
            _mockRecipeDAO
                .Setup(DAO => DAO.InsertRecipe(It.IsAny<RecipeEntity>()))
                .Returns(Task.FromResult(3));

            await _recipeRepo.CreateRecipe(It.IsAny<RecipeBO>());

            _mockRecipeDAO.Verify(DAO => DAO.InsertRecipe(It.IsAny<RecipeEntity>()));
        }

        [Fact]
        public async void DeleteRecipeForId_DAODeleteRecipeShouldBeCalled()
        {
            var id = Guid.NewGuid();
            _mockRecipeDAO.Setup(DAO => DAO.DeleteRecipeForId(id)).Returns(Task.CompletedTask);

            await _recipeRepo.DeleteRecipeForId(id);

            _mockRecipeDAO.Verify(DAO => DAO.DeleteRecipeForId(id));
        }

        [Fact]
        public async void UpdateRecipe_DAOUpdateRecipeShouldBeCalled()
        {
            _mockRecipeDAO
                .Setup(DAO => DAO.UpdateRecipe(It.IsAny<RecipeEntity>()))
                .Returns(Task.CompletedTask);

            var recipe = new UpdateRecipeBO
            {
                Id = new Guid(),
                Name = "Tiramisu",
                PortionSize = 4,
                Instruction = "bake, bake, bake",
            };

            await _recipeRepo.UpdateRecipe(recipe);

            _mockRecipeDAO.Verify(DAO => DAO.UpdateRecipe(It.IsAny<RecipeEntity>()));
        }
    }
}

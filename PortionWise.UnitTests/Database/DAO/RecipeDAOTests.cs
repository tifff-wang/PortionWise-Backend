using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.UnitTests.Database.DAO
{
  public class RecipeDAOTests
    {
        private MockDBContext _mockContext;
        private RecipeDAO _recipeDAO;
        private List<RecipeEntity> _mockRecipeEntityData = MockRecipeData.CreateMockRecipeEntity();

        public RecipeDAOTests()
        {
            _mockContext = new MockDBContext("recipe");
            _recipeDAO = new RecipeDAO(_mockContext.Context);
            _mockContext.ClearTestingData();
        }

        [Fact]
        public async void GetAllRecipeSummaries_ReturnCorrectSummaries()
        {
            _mockContext.AddTestingData(_mockRecipeEntityData);

            var recipeSummaries = await _recipeDAO.GetAllRecipeSummaries();

            Assert.Equal(2, recipeSummaries.Count());
            Assert.All(
                recipeSummaries,
                recipe => Assert.True(string.IsNullOrEmpty(recipe.Instruction))
            );
            Assert.Contains(recipeSummaries, recipe => recipe.Name == "Banana Bread");
            Assert.Contains(recipeSummaries, recipe => recipe.Name == "Chocolate Cake");
        }

        [Fact]
        public async void GetRecipeById_RecipeExists_ReturnRecipe()
        {
            _mockContext.AddTestingData(_mockRecipeEntityData);
            var id = _mockRecipeEntityData[0].Id;

            var recipe = await _recipeDAO.GetRecipeById(id);

            Assert.NotNull(recipe);
            Assert.Equal("Banana Bread", recipe.Name);
        }

        [Fact]
        public async void GetRecipeById_RecipeNotExists_ThrowsException()
        {
            var id = _mockRecipeEntityData[0].Id;

            await Assert.ThrowsAsync<RecipeNotFoundException>(
                async () => await _recipeDAO.GetRecipeById(id)
            );
        }

        [Fact]
        public async void InsertRecipe_NoException_Return3()
        {
            var recipe = _mockRecipeEntityData[0];

            var affectedRow = await _recipeDAO.InsertRecipe(recipe);

            Assert.Equal(3, affectedRow);
        }

        [Fact]
        public async void DeleteRecipeForId_NoException()
        {
            _mockContext.AddTestingData(_mockRecipeEntityData);
            var id = _mockRecipeEntityData[0].Id;

            await _recipeDAO.DeleteRecipeForId(id);

            Assert.False(_mockContext.Context.Recipes.Any(r => r.Id == id));
        }

        [Fact]
        public async void DeleteRecipeForId_ReciptNotExists_ThrowException()
        {
            await Assert.ThrowsAsync<RecipeNotFoundException>(
                async () => await _recipeDAO.DeleteRecipeForId(_mockRecipeEntityData[0].Id)
            );
        }

        [Fact]
        public async void UpdateRecipe_NoException()
        {
            _mockContext.AddTestingData(_mockRecipeEntityData);
            var recipe = _mockRecipeEntityData[0];
            recipe.Name = "Tiramisu";
            recipe.Instruction = "Bake, bake, bake";

            await _recipeDAO.UpdateRecipe(recipe);

            var updatedRecipe = _mockContext.Context.Recipes.FirstOrDefault(r => r.Id == recipe.Id);
            Assert.NotNull(updatedRecipe);
            Assert.Equal("Tiramisu", updatedRecipe.Name);
            Assert.Equal("Bake, bake, bake", updatedRecipe.Instruction);
        }
    }
}

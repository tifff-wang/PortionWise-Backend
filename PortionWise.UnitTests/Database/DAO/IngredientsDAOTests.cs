using PortionWise.Database.DAOs.Ingredient;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.UnitTests.Database.DAO
{
  public class IngredientsDAOTests
    {
        private readonly MockDBContext _mockContext;
        private readonly IngredientDAO _ingredientDAO;
        private readonly List<RecipeEntity> _mockEntityData = MockEntity.CreateMockEntity();

        public IngredientsDAOTests()
        {
            _mockContext = new MockDBContext("ingredientDAOTests");
            _ingredientDAO = new IngredientDAO(_mockContext.Context);
            _mockContext.ClearTestingData();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(10, 5)]
        public async void GetPopularIngredientNames_ReturnNameList(int value, int expected)
        {
            _mockContext.AddTestingData(_mockEntityData);

            var popularIngredients = await _ingredientDAO.GetPopularIngredientNames(value);

            Assert.Equal(expected, popularIngredients.Count());
            if (popularIngredients.Any())
            {
                Assert.Equal("Milk", popularIngredients.First());
            }
        }

        [Fact]
        public async void GetIngredientById_IngredientExists_ReturnIngredient()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var id = _mockEntityData[0].Ingredients!.First().Id;

            var ingredient = await _ingredientDAO.GetIngredientById(id);

            Assert.NotNull(ingredient);
            Assert.Equal("Banana", ingredient.Name);
        }

        [Fact]
        public async void GetIngredientById_IngredientNotExists_ThrowsException()
        {
            var id = new Guid();

            await Assert.ThrowsAsync<IngredientNotFoundException>(
                async () => await _ingredientDAO.GetIngredientById(id)
            );
        }

        [Fact]
        public async void InsertIngredient_NoException_Return1()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var ingredient = new IngredientEntity
            {
                Id = Guid.NewGuid(),
                Name = "Salt",
                Amount = 10,
                Unit = "g",
                RecipeId = _mockEntityData[0].Id
            };

            var affectedRow = await _ingredientDAO.InsertIngredient(ingredient);

            Assert.Equal(1, affectedRow);
            var existingIngredient = await _mockContext.Context.Ingredients.FindAsync(
                ingredient.Id
            );
            Assert.NotNull(existingIngredient);
        }

        [Fact]
        public async void DeleteIngredientForId_NoException()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var id = _mockEntityData[0].Ingredients!.First().Id;
            await _ingredientDAO.DeleteIngredient(id);

            Assert.False(_mockContext.Context.Ingredients.Any(i => i.Id == id));
        }

        [Fact]
        public async void DeleteRecipeForId_ReciptNotExists_ThrowException()
        {
            var id = new Guid();
            await Assert.ThrowsAsync<IngredientNotFoundException>(
                async () => await _ingredientDAO.DeleteIngredient(id)
            );
        }

        [Fact]
        public async void UpdateIngredient_NoException()
        {
            _mockContext.AddTestingData(_mockEntityData);
            var ingredient = _mockEntityData[0].Ingredients!.First();
            ingredient.Amount = 300;
            ingredient.Unit = "ml";

            await _ingredientDAO.UpdateIngredient(ingredient);

            var updatedIngredient = _mockContext.Context.Ingredients.FirstOrDefault(
                i => i.Id == ingredient.Id
            );
            Assert.NotNull(updatedIngredient);
            Assert.Equal(300, updatedIngredient.Amount);
            Assert.Equal("ml", updatedIngredient.Unit);
        }
    }
}

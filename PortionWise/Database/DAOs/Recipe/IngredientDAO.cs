using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database.DAOs.Ingredient
{
    public interface IIngredientDAO
    {
        Task<List<string>> GetPopularIngredientNames(int count);
        Task<IngredientEntity> GetIngredientById(Guid id);
        Task<int> InsertIngredient(IngredientEntity ingredient);
        Task<int> DeleteIngredient(Guid id);
        Task<int> UpdateIngredient(IngredientEntity ingredient);
    }

    public class IngredientDAO : IIngredientDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public IngredientDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<IngredientEntity> Ingredients => _dbContext.Ingredients;
        public DbSet<RecipeEntity> Recipes => _dbContext.Recipes;

        public async Task<List<String>> GetPopularIngredientNames(int count)
        {
            var popularIngredients = await Ingredients
                .GroupBy(i => i.Name)
                .Select(group => new { PopularName = group.Key, Count = group.Count() })
                .OrderByDescending(result => result.Count)
                .Take(count)
                .Select(result => result.PopularName)
                .ToListAsync();

            return popularIngredients;
        }

        public async Task<IngredientEntity> GetIngredientById(Guid id)
        {
            var existingIngredient = await Ingredients
                .Where(ingredient => ingredient.Id == id)
                .FirstOrDefaultAsync();

            if (existingIngredient == null)
            {
                throw new IngredientNotFoundException();
            }

            return existingIngredient;
        }

        public async Task<int> InsertIngredient(IngredientEntity ingredient)
        {
            var recipe = await Recipes.FindAsync(ingredient.RecipeId);
            if (recipe == null)
            {
                throw new RecipeNotFoundException();
            }

            var newIngredient = new IngredientEntity
            {
                Id = Guid.NewGuid(),
                Name = ingredient.Name,
                Amount = ingredient.Amount,
                Unit = ingredient.Unit,
                RecipeId = ingredient.RecipeId,
                Recipe = recipe
            };

            Ingredients.Add(newIngredient);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteIngredient(Guid id)
        {
            var ingredientToDelete = await Ingredients
                .Where(ingredient => ingredient.Id == id)
                .FirstOrDefaultAsync();

            if (ingredientToDelete == null)
            {
                throw new IngredientNotFoundException();
            }

            Ingredients.Remove(ingredientToDelete);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateIngredient(IngredientEntity ingredient)
        {
            Ingredients.Update(ingredient);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

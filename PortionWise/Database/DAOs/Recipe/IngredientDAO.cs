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

        private DbSet<IngredientEntity> _ingredients => _dbContext.Ingredients;

        public async Task<List<String>> GetPopularIngredientNames(int count)
        {
            var popularIngredients = await _ingredients
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
            var existingIngredient = await _ingredients
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
            _ingredients.Add(ingredient);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteIngredient(Guid id)
        {
            var ingredientToDelete = await _ingredients
                .Where(ingredient => ingredient.Id == id)
                .FirstOrDefaultAsync();

            if (ingredientToDelete == null)
            {
                throw new IngredientNotFoundException();
            }

            _ingredients.Remove(ingredientToDelete);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateIngredient(IngredientEntity ingredient)
        {
            _ingredients.Update(ingredient);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

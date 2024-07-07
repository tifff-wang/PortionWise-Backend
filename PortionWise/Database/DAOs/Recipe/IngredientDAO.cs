using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.Entities;

namespace PortionWise.Database.DAOs.Ingredient
{
    public interface IIngredientDAO { }

    public class IngredientDAO : IIngredientDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public IngredientDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<IngredientEntity> Ingredients => _dbContext.Ingredients;

        public async Task<List<string>> GetPopularIngredientNames()
        {
            var popularIngredients = await Ingredients
                .GroupBy(i => i.Name)
                .Select(group => new { Name = group.Key, Count = group.Count() })
                .OrderByDescending(result => result.Count)
                .Take(8)
                .Select(result => result.Name)
                .ToListAsync();

            return popularIngredients;
        }

        public async Task<int> InsertIngredient(IngredientEntity ingredient)
        {
            Ingredients.Add(ingredient);
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

using System.Data;
using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database.DAOs.Recipe
{
    public interface IRecipeDAO
    {
        Task<int> InsertRecipe(RecipeEntity recipe);
        Task<List<RecipeEntity>> GetAllRecipeSummaries();
        Task<RecipeEntity> GetRecipeById(Guid id);
        Task DeleteRecipeForId(Guid id);
        Task UpdateRecipe(RecipeEntity recipe);
    }

    public class RecipeDAO : IRecipeDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public RecipeDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<RecipeEntity> Recipes => _dbContext.Recipes;

        public async Task<List<RecipeEntity>> GetAllRecipeSummaries()
        {
            return await Recipes
                .Select(
                    x =>
                        new RecipeEntity
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CreatedAt = x.CreatedAt,
                            Instruction = "",
                        }
                )
                .ToListAsync();
        }

        public async Task<RecipeEntity> GetRecipeById(Guid id)
        {
            var existingRecipe = await Recipes
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.NutritionInfo)
                .Where(recipe => recipe.Id == id)
                .FirstOrDefaultAsync();

            if (existingRecipe == null)
            {
                throw new RecipeNotFoundException();
            }

            return existingRecipe;
        }

        public async Task<int> InsertRecipe(RecipeEntity recipe)
        {
            Recipes.Add(recipe);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRecipeForId(Guid id)
        {
            var existingRecipe = await Recipes
                .Include(recipe => recipe.Ingredients)
                .Where(recipe => recipe.Id == id)
                .FirstOrDefaultAsync();

            if (existingRecipe == null)
            {
                throw new RecipeNotFoundException();
            }

            Recipes.Remove(existingRecipe);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRecipe(RecipeEntity recipe)
        {
            Recipes.Update(recipe);
            await _dbContext.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database.DAOs.Recipe
{
    public interface IRecipeDAO
    {
        Task<int> InsertRecipeWithoutSaving(RecipeEntity recipe);
    }

    public class RecipeDAO : IRecipeDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public RecipeDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<RecipeEntity> Recipes => _dbContext.Recipes;

        public async Task<int> InsertRecipeWithoutSaving(RecipeEntity recipe)
        {
            Recipes.Add(recipe);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

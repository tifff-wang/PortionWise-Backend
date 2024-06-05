using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database.DAOs.Recipe
{
    public interface IRecipeDAO
    {
        Task<int> InsertRecipeWithoutSaving(RecipeEntity recipe);
        Task<List<RecipeEntity>> GetAllRecipeSummaries();
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
                .Select(x => new RecipeEntity
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedAt = x.CreatedAt,
                    Instruction = "",
                })
                .ToListAsync();
        }

        public async Task<int> InsertRecipeWithoutSaving(RecipeEntity recipe)
        {
            Recipes.Add(recipe);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

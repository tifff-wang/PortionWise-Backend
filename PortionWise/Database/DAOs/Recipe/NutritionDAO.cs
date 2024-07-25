using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition.Entity;

namespace PortionWise.Database.DAOs.Recipe
{
  public interface INutritionDAO
    {
        Task<NutritionEntity> GetNutritionByRecipeId(Guid id);
        Task<int> InsertNutritionInfo(NutritionEntity nutrition);
        Task<int> DeleteNutritionInfoIfExist(Guid id);
    }

    public class NutritionDAO : INutritionDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public NutritionDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<NutritionEntity> _nutrition => _dbContext.NutritionInfo;

        public async Task<NutritionEntity> GetNutritionByRecipeId(Guid recipeId)
        {
            var existingNutrition = await _nutrition
                .Where(n => n.RecipeId == recipeId)
                .FirstOrDefaultAsync();
            if (existingNutrition == null)
            {
                throw new NutritionInfoNotFoundException();
            }

            return existingNutrition;
        }

        public async Task<int> InsertNutritionInfo(NutritionEntity nutrition)
        {
            _nutrition.Add(nutrition);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteNutritionInfoIfExist(Guid recipeId)
        {
            var nutrition = await _nutrition
                .Where(n => n.RecipeId == recipeId)
                .FirstOrDefaultAsync();

            if (nutrition == null)
            {
                return 0;
            }

            _nutrition.Remove(nutrition);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

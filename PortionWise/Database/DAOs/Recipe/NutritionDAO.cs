using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition.Entity;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database.DAOs.Recipe
{
    public interface INutritionDAO
    {
        Task<NutritionEntity> GetNutritionByRecipeId(Guid id);
        Task<int> InsertNutritionInfo(NutritionEntity nutrition);
        Task<int> DeleteNutritionInfo(Guid id);
    }

    public class NutritionDAO : INutritionDAO
    {
        private readonly ApplicationDBContext _dbContext;

        public NutritionDAO(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<NutritionEntity> Nutrition => _dbContext.NutritionInfo;
        public DbSet<RecipeEntity> Recipes => _dbContext.Recipes;

        public async Task<NutritionEntity> GetNutritionByRecipeId(Guid recipeId)
        {
            var existingNutrition = await Nutrition
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
            var recipe = await Recipes.Where(r => r.Id == nutrition.RecipeId).FirstOrDefaultAsync();
            if (recipe == null)
            {
                Console.WriteLine($"error in insertNutrition recipeId = {nutrition.RecipeId})");
                throw new RecipeNotFoundException();
            }

            var newNutritionInfo = new NutritionEntity
            {
                Id = Guid.NewGuid(),
                SugarGram = nutrition.SugarGram,
                FiberGram = nutrition.FiberGram,
                ServingSize = nutrition.ServingSize,
                SodiumMg = nutrition.SodiumMg,
                PotassiumMg = nutrition.PotassiumMg,
                FatSaturatedGram = nutrition.FatSaturatedGram,
                FatTotalGram = nutrition.FatTotalGram,
                Calories = nutrition.Calories,
                CholesterolMg = nutrition.CholesterolMg,
                ProteinGram = nutrition.ProteinGram,
                CacheExpirationTime = DateTime.UtcNow.AddMinutes(5),
                RecipeId = nutrition.RecipeId,
                Recipe = recipe
            };

            Console.WriteLine("data inserted");

            Nutrition.Add(newNutritionInfo);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteNutritionInfo(Guid id)
        {
            var nutrition = await Nutrition.Where(n => n.Id == id).FirstOrDefaultAsync();
            if (nutrition == null)
            {
                throw new NutritionInfoNotFoundException();
            }

            Nutrition.Remove(nutrition);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

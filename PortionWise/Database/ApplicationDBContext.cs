using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Nutrition.Entity;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<IngredientEntity> Ingredients { get; set; }
        public DbSet<RecipeEntity> Recipes { get; set; }

        public DbSet<NutritionEntity> NutritionInfo { get; set; }

        protected readonly IConfiguration Configuration;

        public ApplicationDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("DefaultSQLConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _configReceipt(modelBuilder);
            _configNutrition(modelBuilder);
        }

        private void _configReceipt(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RecipeEntity>()
                .HasMany(recipe => recipe.Ingredients)
                .WithOne(ingredient => ingredient.Recipe)
                .HasForeignKey(ingredient => ingredient.RecipeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        private void _configNutrition(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RecipeEntity>()
                .HasMany(recipe => recipe.NutritionInfo)
                .WithOne(nutritionInfo => nutritionInfo.Recipe)
                .HasForeignKey(nutritionInfo => nutritionInfo.RecipeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}

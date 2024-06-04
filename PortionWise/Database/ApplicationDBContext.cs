using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Ingredient.Entities;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Database
{
    public class ApplicationDBContext : DbContext
    {

        public DbSet<IngredientEntity> Ingredients { get; set; }
        public DbSet<RecipeEntity> Recipes { get; set; }

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
        }

        private void _configReceipt(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RecipeEntity>()
                .HasMany(recipe => recipe.Ingredients)
                .WithOne(ingredient => ingredient.Recipe)
                .HasForeignKey(ingredient => ingredient.RecipeId)
                .IsRequired(false);
        }
    }

}


using PortionWise.Database.DAOs.Ingredient;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Repository;
using PortionWise.Services;

namespace PortionWise.Configs
{
    class DIConfig
    {
        public static void config(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRecipeService, RecipeService>();
            builder.Services.AddTransient<IRecipeRepo, RecipeRepo>();
            builder.Services.AddTransient<IRecipeDAO, RecipeDAO>();

            builder.Services.AddTransient<IIngredientDAO, IngredientDAO>();
        }
    }
}

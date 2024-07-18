using AutoMapper;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Nutrition.DTOs;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Repositories;

namespace PortionWise.Services
{
    public interface INutritionService
    {
        Task<TotalNutritionDTO> GetRecipeNutrition(Guid recipeId);
    }

    public class NutritionService : INutritionService
    {
        private readonly IRecipeService _recipeService;
        private readonly INutritionRepo _nuritionRepo;
        private IMapper _mapper;

        public NutritionService(
            IRecipeService recipeService,
            INutritionRepo nuritionRepo,
            IMapper mapper
        )
        {
            _recipeService = recipeService;
            _nuritionRepo = nuritionRepo;
            _mapper = mapper;
        }

        public async Task<TotalNutritionDTO> GetRecipeNutrition(Guid recipeId)
        {
            RecipeDTO recipe = await _recipeService.GetRecipeById(recipeId);
            if (recipe == null)
            {
                throw new RecipeNotFoundException();
            }

            List<IngredientDTO> ingredients = recipe.Ingredients;

            var ingredientStrings = ingredients.Select(
                i => $"{Math.Round(i.Amount)}{i.Unit} {i.Name}"
            );
            string ingredientString = string.Join(" and ", ingredientStrings);
            Console.WriteLine($"ingredient string: {ingredientString}");
            var totalNutrition = await _nuritionRepo.GetTotalNutritionInfo(ingredientString);

            return _mapper.Map<TotalNutritionDTO>(totalNutrition);
        }
    }
}

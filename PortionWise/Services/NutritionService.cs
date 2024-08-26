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

            if (recipe.Ingredients == null)
            {
                throw new IngredientNotFoundException();
            }

            var ingredientString = IngredientsToString(recipe.Ingredients);

            var totalNutrition = await _nuritionRepo.GetTotalNutritionInfo(
                ingredientString,
                recipeId
            );

            return _mapper.Map<TotalNutritionDTO>(totalNutrition);
        }

        public string IngredientsToString(List<IngredientDTO> ingredients)
        {
            IEnumerable<string> ingredientStrings = ingredients.Select(
                i => $"{Math.Round(i.Amount)}{i.Unit} {i.Name}"
            );
            string ingredientString = string.Join(" and ", ingredientStrings);
            return ingredientString;
        }
    }
}

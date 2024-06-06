using AutoMapper;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Repository;

namespace PortionWise.Services
{
    public interface IRecipeService
    {
        Task CreateRecipe(CreateRecipeDTO recipe);
        Task<IEnumerable<RecipeSummaryDTO>> GetAllRecipeSummaries();
        Task DeleteRecipeForId(Guid id);
        Task UpdateRecipe(RecipeDTO recipe);
    }

    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepo _recipeRepo;
        private IMapper _mapper;

        public RecipeService(
            IRecipeRepo recipeRepo,
            IMapper mapper
        )
        {
            _recipeRepo = recipeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RecipeSummaryDTO>> GetAllRecipeSummaries()
        {
            var recipes = await _recipeRepo.GetAllRecipeSummaries();
            return _mapper.Map<List<RecipeSummaryDTO>>(recipes);
        }

        public async Task CreateRecipe(CreateRecipeDTO recipe)
        {
            if (string.IsNullOrWhiteSpace(recipe.Name))
            {
                throw new RecipeMissingNameException();
            }
            else if (recipe.portionSize <= 0)
            {
                throw new RecipeInvalidPortionSizeException();
            }

            var bo = _mapper.Map<RecipeBO>(recipe);
            await _recipeRepo.CreateRecipe(bo);
        }

        public async Task DeleteRecipeForId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new RecipeMissingIdException();
            }

            await _recipeRepo.DeleteRecipeForId(id);
        }

        public async Task UpdateRecipe(RecipeDTO recipe)
        {
            var bo = _mapper.Map<RecipeBO>(recipe);
            await _recipeRepo.UpdateRecipe(bo);
        }
    }
}

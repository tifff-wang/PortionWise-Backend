using AutoMapper;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Repository
{
    public interface IRecipeRepo
    {
        Task<int> CreateRecipe(RecipeBO recipe);
        Task<List<RecipeBO>> GetAllRecipeSummaries();
        Task<RecipeBO> GetRecipeById(Guid id);
        Task DeleteRecipeForId(Guid id);
        Task UpdateRecipe(RecipeBO recipe);
    }

    public class RecipeRepo : IRecipeRepo
    {
        private readonly IRecipeDAO _recipeDAO;
        private IMapper _mapper;

        public RecipeRepo(
            IRecipeDAO recipeDAO,
            IMapper mapper
        )
        {
            _recipeDAO = recipeDAO;
            _mapper = mapper;
        }

        public async Task<List<RecipeBO>> GetAllRecipeSummaries()
        {
            var recipes = await _recipeDAO.GetAllRecipeSummaries();
            return _mapper.Map<List<RecipeBO>>(recipes);
        }

        public async Task<RecipeBO> GetRecipeById(Guid id)
        {
            var recipe = await _recipeDAO.GetRecipeById(id);
            return _mapper.Map<RecipeBO>(recipe);
        }

        public async Task<int> CreateRecipe(RecipeBO recipe)
        {
            var entity = _mapper.Map<RecipeEntity>(recipe);
            return await _recipeDAO.InsertRecipe(entity);
        }

        public async Task DeleteRecipeForId(Guid id)
        {
            await _recipeDAO.DeleteRecipeForId(id);
        }

        public async Task UpdateRecipe(RecipeBO recipe)
        {
            var existingRecipe = await _recipeDAO.GetRecipeById(recipe.Id);
            existingRecipe = _mapper.Map(recipe, existingRecipe);

            await _recipeDAO.UpdateRecipe(existingRecipe);
        }
    }
}

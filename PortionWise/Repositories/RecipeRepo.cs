using AutoMapper;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Recipe.BO;
using PortionWise.Models.Recipe.Entities;

namespace PortionWise.Repository
{
    public interface IRecipeRepo
    {
        Task<int> CreateRecipe(RecipeBO recipe);
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

        public async Task<int> CreateRecipe(RecipeBO recipe)
        {
            var entity = _mapper.Map<RecipeEntity>(recipe);
            return await _recipeDAO.InsertRecipeWithoutSaving(entity);
        }
    }
}

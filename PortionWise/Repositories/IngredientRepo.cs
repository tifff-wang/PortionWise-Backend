using AutoMapper;
using PortionWise.Database.DAOs.Ingredient;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Ingredient.Entities;

namespace PortionWise.Repositories
{
    public interface IIngredientRepo
    {
        Task<List<PopularIngredientsBO>> GetPopularIngredientNames(int count);
        Task<IngredientBO> GetIngredientById(Guid id);
        Task<int> CreateIngredient(IngredientBO ingredient);
        Task DeleteIngredient(Guid id);
        Task UpdateIngredient(UpdateIngredientBO ingredient);
    }

    public class IngredientRepo : IIngredientRepo
    {
        private readonly IIngredientDAO _ingredientDAO;
        private readonly INutritionDAO _nutritionDAO;

        private IMapper _mapper;

        public IngredientRepo(
            IIngredientDAO ingredientDAO,
            INutritionDAO nutritionDAO,
            IMapper mapper
        )
        {
            _ingredientDAO = ingredientDAO;
            _nutritionDAO = nutritionDAO;
            _mapper = mapper;
        }

        public async Task<List<PopularIngredientsBO>> GetPopularIngredientNames(int count)
        {
            var ingredients = await _ingredientDAO.GetPopularIngredientNames(count);
            return _mapper.Map<List<PopularIngredientsBO>>(ingredients);
        }

        public async Task<IngredientBO> GetIngredientById(Guid id)
        {
            var ingredient = await _ingredientDAO.GetIngredientById(id);
            return _mapper.Map<IngredientBO>(ingredient);
        }

        public async Task<int> CreateIngredient(IngredientBO ingredient)
        {
            await _nutritionDAO.DeleteNutritionInfoIfExist(ingredient.RecipeId);

            var entity = _mapper.Map<IngredientEntity>(ingredient);
            return await _ingredientDAO.InsertIngredient(entity);
        }

        public async Task DeleteIngredient(Guid id)
        {
            var ingredient = await _ingredientDAO.GetIngredientById(id);
            if (ingredient == null)
            {
                throw new IngredientNotFoundException();
            }
            await _nutritionDAO.DeleteNutritionInfoIfExist(ingredient.RecipeId);
            await _ingredientDAO.DeleteIngredient(id);
        }

        public async Task UpdateIngredient(UpdateIngredientBO ingredient)
        {
            var existingIngredient = await _ingredientDAO.GetIngredientById(ingredient.Id);
            await _nutritionDAO.DeleteNutritionInfoIfExist(existingIngredient.RecipeId);

            existingIngredient = _mapper.Map(ingredient, existingIngredient);
            await _ingredientDAO.UpdateIngredient(existingIngredient);
        }
    }
}

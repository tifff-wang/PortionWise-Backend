using AutoMapper;
using PortionWise.Database.DAOs.Ingredient;
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

        private IMapper _mapper;

        public IngredientRepo(IIngredientDAO ingredientDAO, IMapper mapper)
        {
            _ingredientDAO = ingredientDAO;
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
            var entity = _mapper.Map<IngredientEntity>(ingredient);
            return await _ingredientDAO.InsertIngredient(entity);
        }

        public async Task DeleteIngredient(Guid id)
        {
            await _ingredientDAO.DeleteIngredient(id);
        }

        public async Task UpdateIngredient(UpdateIngredientBO ingredient)
        {
            var existingIngredient = await _ingredientDAO.GetIngredientById(ingredient.Id);
            existingIngredient = _mapper.Map(ingredient, existingIngredient);

            await _ingredientDAO.UpdateIngredient(existingIngredient);
        }
    }
}

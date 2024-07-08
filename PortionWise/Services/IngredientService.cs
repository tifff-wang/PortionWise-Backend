using AutoMapper;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.BOs;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Repositories;

namespace PortionWise.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<PopularIngredientDTO>> GetPopularIngredientNames(int count);
        Task<IngredientDTO> GetIngredientById(Guid id);
        Task CreateIngredient(CreateIngredientDTO ingredient);
        Task DeleteIngredient(Guid id);
        Task UpdateIngredient(IngredientDTO ingredient);
    }

    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepo _ingredientRepo;
        private readonly IMapper _mapper;

        public IngredientService(IIngredientRepo ingredientRepo, IMapper mapper)
        {
            _ingredientRepo = ingredientRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PopularIngredientDTO>> GetPopularIngredientNames(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than zero.");
            }
            var ingredientNames = await _ingredientRepo.GetPopularIngredientNames(count);
            return _mapper.Map<IEnumerable<PopularIngredientDTO>>(ingredientNames);
        }

        public async Task<IngredientDTO> GetIngredientById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new IngredientMissingIdException();
            }

            var ingredient = await _ingredientRepo.GetIngredientById(id);
            return _mapper.Map<IngredientDTO>(ingredient);
        }

        public async Task CreateIngredient(CreateIngredientDTO ingredient)
        {
            if (string.IsNullOrEmpty(ingredient.Name))
            {
                throw new IngredientMissingNameException();
            }
            else if (ingredient.Amount <= 0)
            {
                throw new IngredientInvalidAmountException();
            }

            var bo = _mapper.Map<IngredientBO>(ingredient);
            await _ingredientRepo.CreateIngredient(bo);
        }

        public async Task DeleteIngredient(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new IngredientMissingIdException();
            }

            await _ingredientRepo.DeleteIngredient(id);
        }

        public async Task UpdateIngredient(IngredientDTO ingredient)
        {
            var bo = _mapper.Map<IngredientBO>(ingredient);
            await _ingredientRepo.UpdateIngredient(bo);
        }
    }
}

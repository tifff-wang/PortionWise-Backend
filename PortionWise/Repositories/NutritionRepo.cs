using AutoMapper;
using PortionWise.Api;
using PortionWise.Models.Nutrition.BOs;

namespace PortionWise.Repositories
{
  public interface INutritionRepo 
    { 
      Task<TotalNutritionBO> GetTotalNutritionInfo(string query);
    }

    public class NutritionRepo : INutritionRepo
    {
        private readonly INutritionApi _nutritionApi;
        private IMapper _mapper;

        public NutritionRepo(INutritionApi nutritionApi, IMapper mapper)
        {
            _nutritionApi = nutritionApi;
            _mapper = mapper;
        }

        public async Task<TotalNutritionBO> GetTotalNutritionInfo(string query)
        {
            var totalNutrition = await _nutritionApi.GetNutritionInfo(query);
            return _mapper.Map<TotalNutritionBO>(totalNutrition);
        }
    }
}

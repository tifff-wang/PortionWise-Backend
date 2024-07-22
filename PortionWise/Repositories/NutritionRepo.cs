using System.Linq.Expressions;
using AutoMapper;
using PortionWise.Api;
using PortionWise.Database.DAOs.Recipe;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition.BOs;
using PortionWise.Models.Nutrition.DLs;
using PortionWise.Models.Nutrition.Entity;

namespace PortionWise.Repositories
{
    public interface INutritionRepo
    {
        Task<TotalNutritionBO> GetTotalNutritionInfo(string query, Guid recipeId);
    }

    public class NutritionRepo : INutritionRepo
    {
        private readonly INutritionApi _nutritionApi;
        private readonly INutritionDAO _nutritionDAO;
        private IMapper _mapper;

        public NutritionRepo(INutritionApi nutritionApi, INutritionDAO nutritionDAO, IMapper mapper)
        {
            _nutritionApi = nutritionApi;
            _nutritionDAO = nutritionDAO;
            _mapper = mapper;
        }

        public async Task<TotalNutritionBO> GetTotalNutritionInfo(string query, Guid recipeId)
        {
            try
            {
                var nutritionFromDB = await _nutritionDAO.GetNutritionByRecipeId(recipeId);
                if (nutritionFromDB.IsValid())
                {
                    await _nutritionDAO.DeleteNutritionInfoIfExist(nutritionFromDB.Id);
                    return await CacheNutritionInfoToDB(query, recipeId);
                }

                return _mapper.Map<TotalNutritionBO>(nutritionFromDB);
            }
            catch (NutritionInfoNotFoundException)
            {
                return await CacheNutritionInfoToDB(query, recipeId);
            }
        }

        public async Task<TotalNutritionBO> CacheNutritionInfoToDB(string query, Guid recipeId)
        {
            var nutritionFromExternalApi = await _nutritionApi.GetNutritionInfo(query);
            var nutritionAddtoDB = _mapper.Map<NutritionEntity>(nutritionFromExternalApi);
            nutritionAddtoDB.RecipeId = recipeId;
            await _nutritionDAO.InsertNutritionInfo(nutritionAddtoDB);
            return _mapper.Map<TotalNutritionBO>(nutritionFromExternalApi);
        }
    }

    static class NutritionEntityExtensions
    {
        public static bool IsValid(this NutritionEntity value)
        {
            return value.CacheExpirationTime <= DateTime.UtcNow;
        }
    }
}

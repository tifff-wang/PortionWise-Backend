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
                Console.WriteLine(recipeId);
                var nutritionFromDB = await _nutritionDAO.GetNutritionByRecipeId(recipeId);
                if (nutritionFromDB.CacheExpirationTime <= DateTime.UtcNow)
                {
                    await _nutritionDAO.DeleteNutritionInfoIfExist(nutritionFromDB.Id);
                    Console.WriteLine("data expired, get info from external api");
                    return await CheckNutritionFromDB(query, recipeId);
                }

                Console.WriteLine("Get info from db");
                return _mapper.Map<TotalNutritionBO>(nutritionFromDB);
            }
            catch (NutritionInfoNotFoundException)
            {
                Console.WriteLine(recipeId);
                Console.WriteLine("no record, Get info from external api");
                return await CheckNutritionFromDB(query, recipeId);
            }
        }

        public async Task<TotalNutritionBO> CheckNutritionFromDB(string query, Guid recipeId)
        {
            var nutritionFromExternalApi = await _nutritionApi.GetNutritionInfo(query);
            var nutritionAddtoDB = _mapper.Map<NutritionEntity>(nutritionFromExternalApi);
            nutritionAddtoDB.RecipeId = recipeId;
            await _nutritionDAO.InsertNutritionInfo(nutritionAddtoDB);
            return _mapper.Map<TotalNutritionBO>(nutritionFromExternalApi);
        }
    }
}

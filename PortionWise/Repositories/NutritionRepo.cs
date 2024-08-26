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
            NutritionEntity? nutritionFromDB = null;
            try
            {
                nutritionFromDB = await _nutritionDAO.GetNutritionByRecipeId(recipeId);
            }
            catch (NutritionInfoNotFoundException)
            {
                // ignore
            }

            bool isValid = !nutritionFromDB?.IsExpired() ?? false;

            if (!isValid)
            {
                if (nutritionFromDB?.IsExpired() == true)
                {
                    await _nutritionDAO.DeleteNutritionInfoIfExist(recipeId);
                }

                var totalNutrition = await LoadTotalNutritionFromApi(query);
                await CacheNutritionInfoToDB(totalNutrition, recipeId);
                return _mapper.Map<TotalNutritionBO>(totalNutrition);
            }

            return _mapper.Map<TotalNutritionBO>(nutritionFromDB);
        }

        public async Task<TotalNutritionDL> LoadTotalNutritionFromApi(string query)
        {
            var nutritionFromExternalApi = await _nutritionApi.GetNutritionInfo(query);
            return nutritionFromExternalApi.SumNutritionInfo();
        }

        public async Task CacheNutritionInfoToDB(TotalNutritionDL totalNutriton, Guid recipeId)
        {
            var nutritionAddtoDB = _mapper.Map<NutritionEntity>(totalNutriton);
            nutritionAddtoDB.RecipeId = recipeId;
            await _nutritionDAO.InsertNutritionInfo(nutritionAddtoDB);
        }
    }

    static class NutritionEntityExtensions
    {
        public static bool IsExpired(this NutritionEntity value)
        {
            return value.CacheExpirationTime <= DateTime.UtcNow;
        }
    }

    public static class NutritionDLExtensions
    {
        public static TotalNutritionDL SumNutritionInfo(this NutritionDL nutritionData)
        {
            TotalNutritionDL totalNutrition = new TotalNutritionDL
            {
                SugarGram = Math.Round(nutritionData.Items.Sum(item => item.SugarGram), 1),
                FiberGram = Math.Round(nutritionData.Items.Sum(item => item.FiberGram), 1),
                ServingSize = nutritionData.Items.Sum(item => item.ServingSize),
                SodiumMg = Math.Round(nutritionData.Items.Sum(item => item.SodiumMg), 1),
                PotassiumMg = Math.Round(nutritionData.Items.Sum(item => item.PotassiumMg), 1),
                FatSaturatedGram = Math.Round(
                    nutritionData.Items.Sum(item => item.FatSaturatedGram),
                    1
                ),
                FatTotalGram = Math.Round(nutritionData.Items.Sum(item => item.FatTotalGram), 1),
                Calories = Math.Round(nutritionData.Items.Sum(item => item.Calories)),
                CholesterolMg = Math.Round(nutritionData.Items.Sum(item => item.CholesterolMg), 1),
                ProteinGram = Math.Round(nutritionData.Items.Sum(item => item.ProteinGram), 1),
                CarbohydratesTotalGram = Math.Round(
                    nutritionData.Items.Sum(item => item.CarbohydratesTotalGram),
                    1
                )
            };
            return totalNutrition;
        }
    }
}

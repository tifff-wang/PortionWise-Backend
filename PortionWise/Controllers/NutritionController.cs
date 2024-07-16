using Microsoft.AspNetCore.Mvc;
using PortionWise.Api;
using PortionWise.Models.Errors;
using PortionWise.Models.Nutrition;

namespace PortionWise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionApi _nutritionApi;

        public NutritionController(INutritionApi nutritionApi)
        {
            _nutritionApi = nutritionApi;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NutritionDL>> GetNutrition(string query)
        {
            try
            {
                var result = await _nutritionApi.GetNutritionInfo(query);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }
        }
    }
}

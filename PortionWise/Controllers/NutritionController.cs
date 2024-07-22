using Microsoft.AspNetCore.Mvc;
using PortionWise.Models.Errors;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Nutrition.DTOs;
using PortionWise.Services;

namespace PortionWise.Controllers
{
    [ApiController]
    [Route("api/Nutrition")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;

        public NutritionController(INutritionService nutritionService)
        {
            _nutritionService = nutritionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TotalNutritionDTO>> GetNutrition(Guid recipeId)
        {
            try
            {
                var result = await _nutritionService.GetRecipeNutrition(recipeId);
                return Ok(result);
            }
            catch (RecipeNotFoundException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }
        }
    }
}

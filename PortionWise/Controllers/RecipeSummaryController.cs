using Microsoft.AspNetCore.Mvc;
using PortionWise.Models.Errors;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Services;

namespace PortionWise.Controllers
{
  [Route("api/RecipeSummaries")]
    [ApiController]
    public class RecipeSummaryController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeSummaryController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RecipeSummaryDTO>>> GetAllRecipeSummaries()
        {
            try
            {
                return Ok(await _recipeService.GetAllRecipeSummaries());
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }
        }
    }
}

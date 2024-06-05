using Microsoft.AspNetCore.Mvc;
using PortionWise.Models.Errors;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Services;

namespace PortionWise.Controllers.Recipes
{
    [Route("api/Recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {

        private readonly IRecipeService _recipeService;
        public RecipeController(
            IRecipeService recipeService
        )
        {
            _recipeService = recipeService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateRecipe([FromBody] CreateRecipeDTO recipe)
        {
            if (recipe == null)
            {
                return BadRequest(new ErrorDTO());
            }

            try
            {
                await _recipeService.CreateRecipe(recipe);
                return StatusCode(201);
            }
            catch (RecipeMissingNameException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (RecipeInvalidPortionSizeException exception)
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
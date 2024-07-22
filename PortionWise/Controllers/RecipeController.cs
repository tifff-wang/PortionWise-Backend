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

        
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeDTO>> GetRecipeById(Guid id)
        {
            try
            {
                var recipe = await _recipeService.GetRecipeById(id);
                return Ok(recipe);
            }
            catch (RecipeMissingIdException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (RecipeNotFoundException exception)
            {
                return NotFound(new ErrorDTO(exception.ErrorMessage));
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRecipeForId(Guid id)
        {
            try
            {
                await _recipeService.DeleteRecipeForId(id);
                return NoContent();
            }
            catch (RecipeMissingIdException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (RecipeNotFoundException exception)
            {
                return NotFound(new ErrorDTO(exception.ErrorMessage));
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRecipeForId(
            Guid id,
            [FromBody] UpdateRecipeDTO recipe
        )
        {
            if (recipe == null || id != recipe.Id)
            {
                return BadRequest(new ErrorDTO());
            }

            try
            {
                await _recipeService.UpdateRecipe(recipe);
                return NoContent();
            }
            catch (RecipeNotFoundException exception)
            {
                return NotFound(new ErrorDTO(exception.ErrorMessage));
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }

        }
    }
}
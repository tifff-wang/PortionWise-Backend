using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortionWise.Models.Errors;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Services;

namespace PortionWise.Controllers
{
    [Route("api/Ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<
            ActionResult<IEnumerable<PopularIngredientDTO>>
        > GetPopularIngredientNames(int count)
        {
            try
            {
                var popularIngredients = await _ingredientService.GetPopularIngredientNames(count);
                return Ok(popularIngredients);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, ErrorDTO.internalError());
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IngredientDTO>> GetIngredientById(Guid id)
        {
            try
            {
                var ingredient = await _ingredientService.GetIngredientById(id);
                return Ok(ingredient);
            }
            catch (IngredientMissingIdException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (IngredientNotFoundException exception)
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateIngredient([FromBody] CreateIngredientDTO ingredient)
        {
            if (ingredient == null)
            {
                return BadRequest(new ErrorDTO());
            }

            try
            {
                await _ingredientService.CreateIngredient(ingredient);
                return StatusCode(201);
            }
            catch (IngredientMissingNameException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (IngredientInvalidAmountException exception)
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteIngredient(Guid id)
        {
            try
            {
                await _ingredientService.DeleteIngredient(id);
                return NoContent();
            }
            catch (IngredientMissingIdException exception)
            {
                return BadRequest(new ErrorDTO(exception.ErrorMessage));
            }
            catch (IngredientNotFoundException exception)
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
        public async Task<ActionResult> UpdateIngredient(
            Guid id,
            [FromBody] IngredientDTO ingredient
        )
        {
            if (ingredient == null || id != ingredient.Id)
            {
                return BadRequest(new ErrorDTO());
            }

            try
            {
                await _ingredientService.UpdateIngredient(ingredient);
                return NoContent();
            }
            catch (IngredientNotFoundException exception)
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

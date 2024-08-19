using Microsoft.AspNetCore.Mvc;
using Moq;
using PortionWise.Controllers.Recipes;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Services;
using PortionWise.UnitTests.MockData.Recipes;

namespace PortionWise.UnitTests.Controller
{
    public class RecipeControllerTests
    {
        private readonly RecipeController _recipeController;
        private readonly Mock<IRecipeService> _mockRecipeService;
        private readonly List<RecipeDTO> _mockRecipeDTOData = MockRecipeDTO.CreateMockRecipeDTO();
        private UpdateRecipeDTO _recipeUpdate;

        private CreateRecipeDTO _recipeToCreate = new CreateRecipeDTO
        {
            Name = "Strawberry Cake",
            PortionSize = 6,
            Instruction = "pick some strawberries"
        };

        public RecipeControllerTests()
        {
            _mockRecipeService = new Mock<IRecipeService>();
            _recipeController = new RecipeController(_mockRecipeService.Object);
            _recipeUpdate = new UpdateRecipeDTO
            {
                Id = _mockRecipeDTOData[0].Id,
                Name = "Tiramisu",
                PortionSize = 4,
                Instruction = "bake, bake, bake",
            };
        }

        [Fact]
        public async void GetRecipeById_RecipeExists_Return200andRecipe()
        {
            var id = Guid.NewGuid();

            _mockRecipeService
                .Setup(service => service.GetRecipeById(id))
                .Returns(Task.FromResult(_mockRecipeDTOData[0]));

            var response = await _recipeController.GetRecipeById(id);

            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedRecipe = Assert.IsType<RecipeDTO>(okResult.Value);
            Assert.Equal(_mockRecipeDTOData[0].Id, returnedRecipe.Id);
            Assert.Equal(_mockRecipeDTOData[0].Name, returnedRecipe.Name);
        }

        [Fact]
        public async void GetRecipeById_MissingId_ReturnBadRequest()
        {
            var id = Guid.Empty;
            _mockRecipeService
                .Setup(service => service.GetRecipeById(id))
                .ThrowsAsync(new RecipeMissingIdException());

            var response = await _recipeController.GetRecipeById(id);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response.Result).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void GetRecipeById_RecipeNotFound_ReturnNotFound()
        {
            var id = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.GetRecipeById(id))
                .ThrowsAsync(new RecipeNotFoundException());

            var response = await _recipeController.GetRecipeById(id);

            var statusCode = Assert.IsType<NotFoundObjectResult>(response.Result).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void GetRecipeById_UnexpectedException_Return500()
        {
            var id = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.GetRecipeById(id))
                .ThrowsAsync(new Exception());

            var response = await _recipeController.GetRecipeById(id);

            var statusCode = Assert.IsType<ObjectResult>(response.Result).StatusCode;
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public async void CreateRecipe_Return201Created()
        {
            _mockRecipeService
                .Setup(service => service.CreateRecipe(_recipeToCreate))
                .Returns(Task.CompletedTask);

            var response = await _recipeController.CreateRecipe(_recipeToCreate);

            var statusCode = Assert.IsType<StatusCodeResult>(response).StatusCode;
            Assert.Equal(201, statusCode);
        }

        [Fact]
        public async Task CreateRecipe_NullRecipe_ReturnsBadRequest()
        {
            var response = await _recipeController.CreateRecipe(null);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void CreateRecipe_MissingName_ReturnBadRequest()
        {
            _recipeToCreate.Name = "";
            _mockRecipeService
                .Setup(service => service.CreateRecipe(_recipeToCreate))
                .ThrowsAsync(new RecipeMissingNameException());

            var response = await _recipeController.CreateRecipe(_recipeToCreate);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void CreateRecipe_InvalidPortionSize_ReturnBadRequest()
        {
            _recipeToCreate.PortionSize = 0;
            _mockRecipeService
                .Setup(service => service.CreateRecipe(_recipeToCreate))
                .ThrowsAsync(new RecipeInvalidPortionSizeException());

            var response = await _recipeController.CreateRecipe(_recipeToCreate);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void CreateRecipe_UnexpectedExeption_Return500()
        {
            _mockRecipeService
                .Setup(service => service.CreateRecipe(_recipeToCreate))
                .ThrowsAsync(new Exception());

            var response = await _recipeController.CreateRecipe(_recipeToCreate);

            var statusCode = Assert.IsType<ObjectResult>(response).StatusCode;
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public async void DeleteRecipeForId_NoException_Return204()
        {
            var id = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.DeleteRecipeForId(id))
                .Returns(Task.CompletedTask);

            var response = await _recipeController.DeleteRecipeForId(id);

            var statusCode = Assert.IsType<NoContentResult>(response).StatusCode;
            Assert.Equal(204, statusCode);
        }

        [Fact]
        public async void DeleteRecipeForId_MissingId_ReturnBadRequest()
        {
            var id = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.DeleteRecipeForId(id))
                .ThrowsAsync(new RecipeMissingIdException());

            var response = await _recipeController.DeleteRecipeForId(id);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void DeleteRecipeForId_NoRecipe_ReturnNotFound()
        {
            var id = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.DeleteRecipeForId(id))
                .ThrowsAsync(new RecipeNotFoundException());

            var response = await _recipeController.DeleteRecipeForId(id);

            var statusCode = Assert.IsType<NotFoundObjectResult>(response).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void DeleteRecipeForId_UnexpectExeption_Return500()
        {
            var id = Guid.NewGuid();
            _mockRecipeService
                .Setup(service => service.DeleteRecipeForId(id))
                .ThrowsAsync(new Exception());

            var response = await _recipeController.DeleteRecipeForId(id);

            var statusCode = Assert.IsType<ObjectResult>(response).StatusCode;
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public async void UpdateRecipeForId_NoException_Return204()
        {
            var id = _mockRecipeDTOData[0].Id;

            _mockRecipeService
                .Setup(service => service.UpdateRecipe(_recipeUpdate))
                .Returns(Task.CompletedTask);

            var response = await _recipeController.UpdateRecipeForId(id, _recipeUpdate);

            var statusCode = Assert.IsType<NoContentResult>(response).StatusCode;
            Assert.Equal(204, statusCode);
        }

        [Fact]
        public async void UpdateRecipeForId_RecipeIsNull_ReturnBadRequest()
        {
            var id = _mockRecipeDTOData[0].Id;
            var response = await _recipeController.UpdateRecipeForId(id, null);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void UpdateRecipeForId_IdNotMatch_ReturnBadRequest()
        {
            var response = await _recipeController.UpdateRecipeForId(new Guid(), _recipeUpdate);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void UpdateRecipeForId_NoRecipe_ReturnNotFound()
        {
            _mockRecipeService
                .Setup(service => service.UpdateRecipe(_recipeUpdate))
                .ThrowsAsync(new RecipeNotFoundException());

            var response = await _recipeController.UpdateRecipeForId(
                _mockRecipeDTOData[0].Id,
                _recipeUpdate
            );

            var statusCode = Assert.IsType<NotFoundObjectResult>(response).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void UpdateRecipeForId_UnexpectException_Return500()
        {
            _mockRecipeService
                .Setup(service => service.UpdateRecipe(_recipeUpdate))
                .ThrowsAsync(new Exception());

            var response = await _recipeController.UpdateRecipeForId(
                _mockRecipeDTOData[0].Id,
                _recipeUpdate
            );

            var statusCode = Assert.IsType<ObjectResult>(response).StatusCode;
            Assert.Equal(500, statusCode);
        }
    }
}

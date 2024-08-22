using Microsoft.AspNetCore.Mvc;
using Moq;
using PortionWise.Controllers;
using PortionWise.Models.Exceptions;
using PortionWise.Models.Ingredient.DTOs;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Services;
using PortionWise.UnitTests.MockData.Recipes;

namespace PortionWise.UnitTests.Controller
{
    public class IngredientControllerTests
    {
        private readonly IngredientController _ingredientController;
        private readonly Mock<IIngredientService> _mockIngredientService;
        private static List<RecipeDTO> _mockDTOData = MockDTO.CreateMockDTO();

        private CreateIngredientDTO _ingredientToCreate = new CreateIngredientDTO
        {
            Name = "Strawberry",
            Amount = 200,
            Unit = "g",
            RecipeId = _mockDTOData[0].Id
        };

        private IngredientDTO _ingredientUpdate = new IngredientDTO
        {
            Id = _mockDTOData[0].Ingredients!.First().Id,
            Name = "Strawberry",
            Amount = 200,
            Unit = "g"
        };

        public IngredientControllerTests()
        {
            _mockIngredientService = new Mock<IIngredientService>();
            _ingredientController = new IngredientController(_mockIngredientService.Object);
        }

        [Fact]
        public async void GetPopularIngredientNames_ReturnOkWithIngredientNames()
        {
            var popularIngredients = MockDTO.CreateMockPopularIngredients();
            _mockIngredientService
                .Setup(service => service.GetPopularIngredientNames(2))
                .Returns(Task.FromResult(popularIngredients.AsEnumerable()));

            var response = await _ingredientController.GetPopularIngredientNames(2);

            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedPopularIngredient = Assert.IsAssignableFrom<
                IEnumerable<PopularIngredientDTO>
            >(okResult.Value);
            Assert.Equal(2, returnedPopularIngredient.Count());
        }

        [Fact]
        public async void GetPopularIngredientNames_InvalidCount_ReturnBadRequest()
        {
            var expectedMessage = "Count must be greater than zero.";
            _mockIngredientService
                .Setup(service => service.GetPopularIngredientNames(0))
                .ThrowsAsync(new ArgumentException(expectedMessage));

            var response = await _ingredientController.GetPopularIngredientNames(0);

            var result = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal(404, result.StatusCode);
            var message = Assert.IsType<string>(result.Value);
            Assert.Contains(expectedMessage, message);
        }

        [Fact]
        public async void GetPopularIngredientNames_UnexpectException_Return500()
        {
            _mockIngredientService
                .Setup(service => service.GetPopularIngredientNames(2))
                .ThrowsAsync(new Exception());

            var response = await _ingredientController.GetPopularIngredientNames(2);

            var okResult = Assert.IsType<ObjectResult>(response.Result);
            Assert.Equal(500, okResult.StatusCode);
        }

        [Fact]
        public async void GetIngredientById_IngredientExists_Return200andRecipe()
        {
            var id = Guid.NewGuid();
            var ingredient = _mockDTOData[0].Ingredients!.First();

            _mockIngredientService
                .Setup(service => service.GetIngredientById(id))
                .Returns(Task.FromResult(ingredient));

            var response = await _ingredientController.GetIngredientById(id);

            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(200, okResult.StatusCode);

            var returnedIngredient = Assert.IsType<IngredientDTO>(okResult.Value);
            Assert.Equal(ingredient.Id, returnedIngredient.Id);
            Assert.Equal(ingredient.Name, returnedIngredient.Name);
        }

        [Fact]
        public async void GetIngredientById_MissingId_ReturnBadRequest()
        {
            var id = Guid.Empty;
            _mockIngredientService
                .Setup(service => service.GetIngredientById(id))
                .ThrowsAsync(new IngredientMissingIdException());

            var response = await _ingredientController.GetIngredientById(id);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response.Result).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void GetIngredientById_IngredientNotFound_ReturnNotFound()
        {
            var id = Guid.NewGuid();
            _mockIngredientService
                .Setup(service => service.GetIngredientById(id))
                .ThrowsAsync(new IngredientNotFoundException());

            var response = await _ingredientController.GetIngredientById(id);

            var statusCode = Assert.IsType<NotFoundObjectResult>(response.Result).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void GetIngredientById_UnexpectedException_Return500()
        {
            var id = Guid.NewGuid();
            _mockIngredientService
                .Setup(service => service.GetIngredientById(id))
                .ThrowsAsync(new Exception());

            var response = await _ingredientController.GetIngredientById(id);

            var statusCode = Assert.IsType<ObjectResult>(response.Result).StatusCode;
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public async void CreateIngredient_Return201Created()
        {
            _mockIngredientService
                .Setup(service => service.CreateIngredient(_ingredientToCreate))
                .Returns(Task.CompletedTask);

            var response = await _ingredientController.CreateIngredient(_ingredientToCreate);

            var statusCode = Assert.IsType<StatusCodeResult>(response).StatusCode;
            Assert.Equal(201, statusCode);
        }

        [Fact]
        public async Task CreateIngredient_NullIngredient_ReturnsBadRequest()
        {
            var response = await _ingredientController.CreateIngredient(null);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void CreateIngredient_MissingName_ReturnBadRequest()
        {
            _ingredientToCreate.Name = "";
            _mockIngredientService
                .Setup(service => service.CreateIngredient(_ingredientToCreate))
                .ThrowsAsync(new IngredientMissingNameException());

            var response = await _ingredientController.CreateIngredient(_ingredientToCreate);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void CreateIngredient_InvalidAmount_ReturnBadRequest()
        {
            _ingredientToCreate.Amount = 0;
            _mockIngredientService
                .Setup(service => service.CreateIngredient(_ingredientToCreate))
                .ThrowsAsync(new IngredientInvalidAmountException());

            var response = await _ingredientController.CreateIngredient(_ingredientToCreate);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void CreateIngredient_RecipeNotFound_ReturnNotFound()
        {
            _mockIngredientService
                .Setup(service => service.CreateIngredient(_ingredientToCreate))
                .ThrowsAsync(new RecipeNotFoundException());

            var response = await _ingredientController.CreateIngredient(_ingredientToCreate);

            var statusCode = Assert.IsType<NotFoundObjectResult>(response).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void CreateIngredient_UnexpectedExeption_Return500()
        {
            _mockIngredientService
                .Setup(service => service.CreateIngredient(_ingredientToCreate))
                .ThrowsAsync(new Exception());

            var response = await _ingredientController.CreateIngredient(_ingredientToCreate);

            var statusCode = Assert.IsType<ObjectResult>(response).StatusCode;
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public async void DeleteIngredient_NoException_Return204()
        {
            var id = Guid.NewGuid();
            _mockIngredientService
                .Setup(service => service.DeleteIngredient(id))
                .Returns(Task.CompletedTask);

            var response = await _ingredientController.DeleteIngredient(id);

            var statusCode = Assert.IsType<NoContentResult>(response).StatusCode;
            Assert.Equal(204, statusCode);
        }

        [Fact]
        public async void DeleteIngredient_MissingId_ReturnBadRequest()
        {
            var id = Guid.Empty;
            _mockIngredientService
                .Setup(service => service.DeleteIngredient(id))
                .ThrowsAsync(new IngredientMissingIdException());

            var response = await _ingredientController.DeleteIngredient(id);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void DeleteIngredient_IngredientNotFound_ReturnNotFound()
        {
            var id = Guid.NewGuid();
            _mockIngredientService
                .Setup(service => service.DeleteIngredient(id))
                .ThrowsAsync(new IngredientNotFoundException());

            var response = await _ingredientController.DeleteIngredient(id);

            var statusCode = Assert.IsType<NotFoundObjectResult>(response).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void DeleteIngredient_UnexpectExeption_Return500()
        {
            var id = Guid.NewGuid();
            _mockIngredientService
                .Setup(service => service.DeleteIngredient(id))
                .ThrowsAsync(new Exception());

            var response = await _ingredientController.DeleteIngredient(id);

            var statusCode = Assert.IsType<ObjectResult>(response).StatusCode;
            Assert.Equal(500, statusCode);
        }

        [Fact]
        public async void UpdateIngredien_NoException_Return204()
        {
            var id = _mockDTOData[0].Ingredients!.First().Id;

            _mockIngredientService
                .Setup(service => service.UpdateIngredient(_ingredientUpdate))
                .Returns(Task.CompletedTask);

            var response = await _ingredientController.UpdateIngredient(id, _ingredientUpdate);

            var statusCode = Assert.IsType<NoContentResult>(response).StatusCode;
            Assert.Equal(204, statusCode);
        }

        [Fact]
        public async void UpdateIngredient_IngredientIsNull_ReturnBadRequest()
        {
            var id = _mockDTOData[0].Id;
            var response = await _ingredientController.UpdateIngredient(id, null);

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void UpdateIngredient_IdNotMatch_ReturnBadRequest()
        {
            var response = await _ingredientController.UpdateIngredient(
                new Guid(),
                _ingredientUpdate
            );

            var statusCode = Assert.IsType<BadRequestObjectResult>(response).StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public async void UpdateIngredient_IngredientNotFound_ReturnNotFound()
        {
            _mockIngredientService
                .Setup(service => service.UpdateIngredient(_ingredientUpdate))
                .ThrowsAsync(new IngredientNotFoundException());

            var response = await _ingredientController.UpdateIngredient(
                _ingredientUpdate.Id,
                _ingredientUpdate
            );

            var statusCode = Assert.IsType<NotFoundObjectResult>(response).StatusCode;
            Assert.Equal(404, statusCode);
        }

        [Fact]
        public async void UpdateIngredient_UnexpectException_Return500()
        {
            _mockIngredientService
                .Setup(service => service.UpdateIngredient(_ingredientUpdate))
                .ThrowsAsync(new Exception());

            var response = await _ingredientController.UpdateIngredient(
                _ingredientUpdate.Id,
                _ingredientUpdate
            );

            var statusCode = Assert.IsType<ObjectResult>(response).StatusCode;
            Assert.Equal(500, statusCode);
        }
    }
}

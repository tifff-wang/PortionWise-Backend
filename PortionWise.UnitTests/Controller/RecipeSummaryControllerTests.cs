using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PortionWise.Controllers;
using PortionWise.Models.Recipe.DTOs;
using PortionWise.Services;

namespace PortionWise.UnitTests.Controller
{
    public class RecipeSummaryControllerTests
    {
        private readonly RecipeSummaryController _recipeSummaryController;
        private readonly Mock<IRecipeService> _mockRecipeService;

        public RecipeSummaryControllerTests()
        {
            _mockRecipeService = new Mock<IRecipeService>();
            _recipeSummaryController = new RecipeSummaryController(_mockRecipeService.Object);
        }

        [Fact]
        public async Task GetAllRecipeSummaries_NoException_ReturnOk()
        {
            var mockData = new List<RecipeSummaryDTO>
            {
                new RecipeSummaryDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Recipe 1",
                    CreatedAt = DateTime.UtcNow
                },
                new RecipeSummaryDTO
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Recipe 2",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockRecipeService
                .Setup(service => service.GetAllRecipeSummaries())
                .Returns(Task.FromResult(mockData.AsEnumerable()));

            var response = await _recipeSummaryController.GetAllRecipeSummaries();

            var okResult = Assert.IsType<OkObjectResult>(response.Result);

            var returnValue = Assert.IsAssignableFrom<IEnumerable<RecipeSummaryDTO>>(
                okResult.Value
            );

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetAllRecipeSummaries_UnexpectExeption_Return500()
        {
            _mockRecipeService
                .Setup(service => service.GetAllRecipeSummaries())
                .ThrowsAsync(new Exception());

            var response = await _recipeSummaryController.GetAllRecipeSummaries();

            var statusCode = Assert.IsType<ObjectResult>(response.Result).StatusCode;

            Assert.Equal(500, statusCode);
        }
    }
}

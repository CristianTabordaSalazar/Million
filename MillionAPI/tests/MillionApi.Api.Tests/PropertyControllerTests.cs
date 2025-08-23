using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using MillionApi.Api.Controllers;
using MillionApi.Application.Services.Property;
using MillionApi.Contracts.Property;

namespace MillionApi.Api.Tests
{
    [TestFixture]
    public class PropertyControllerTests
    {
        private Mock<IPropertyService> _serviceMock = default!;
        private PropertyController _sut = default!;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IPropertyService>(MockBehavior.Strict);
            _sut = new PropertyController(_serviceMock.Object);
        }

        [Test]
        public async Task GetByName_ShouldReturnOk_WithPropertyResponse()
        {
            // Arrange
            var ct = new CancellationTokenSource().Token;
            var inputName = "  Casa Azul  ";
            var trimmed = "Casa Azul";

            var resultFromService = new PropertyResult(
                Id: Guid.NewGuid(),
                Name: trimmed,
                Address: "Calle 123",
                Price: 1000000m,
                CodeInternal: "ABC-123",
                Year: 2020
            );

            _serviceMock
                .Setup(s => s.GetPropertyByNameAsync(trimmed, ct))
                .ReturnsAsync(resultFromService);

            var req = new GetPropertyByNameRequest(inputName);

            // Act
            var actionResult = await _sut.GetByName(req, ct);

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            var ok = (OkObjectResult)actionResult;
            ok.Value.Should().BeOfType<PropertyResponse>();

            var payload = (PropertyResponse)ok.Value!;
            payload.Id.Should().Be(resultFromService.Id);
            payload.Name.Should().Be(resultFromService.Name);
            payload.Address.Should().Be(resultFromService.Address);
            payload.Price.Should().Be(resultFromService.Price);
            payload.CodeInternal.Should().Be(resultFromService.CodeInternal);
            payload.Year.Should().Be(resultFromService.Year);

            _serviceMock.Verify(s => s.GetPropertyByNameAsync(trimmed, ct), Times.Once);
        }
    }
}

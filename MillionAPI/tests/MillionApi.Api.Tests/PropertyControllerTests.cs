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
using MillionApi.Application.Common.Exceptions;
using MillionApi.Contracts.Common;

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
            var inputName = "  Blue House  ";
            var trimmed = "Blue House";

            var resultFromService = new PropertyResult(
                Id: Guid.NewGuid(),
                Name: trimmed,
                Address: "Street 123",
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

        [Test]
        public async Task GetById_ShouldReturnOk_WithPropertyResponse()
        {
            // Arrange
            var ct = new CancellationTokenSource().Token;
            var id = Guid.NewGuid();

            var resultFromService = new PropertyResult(
                Id: Guid.NewGuid(),
                Name: "Blue House",
                Address: "Calle 123",
                Price: 1000000m,
                CodeInternal: "ABC-123",
                Year: 2020
            );

            _serviceMock
                .Setup(s => s.GetByIdAsync(id, ct))
                .ReturnsAsync(resultFromService);

            // Act
            var actionResult = await _sut.GetById(id, ct);

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

            _serviceMock.Verify(s => s.GetByIdAsync(id, ct), Times.Once);
        }

        [Test]
        public void GetById_Should_Propagate_NotFoundException()
        {
            var id = Guid.NewGuid();
            _serviceMock
                .Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException(nameof(MillionApi.Domain.Entities.Property), id.ToString()));

            Func<Task> act = async () => await _sut.GetById(id, CancellationToken.None);

            act.Should().ThrowAsync<NotFoundException>();
            _serviceMock.Verify(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task List_Should_Return_Ok_With_PagedResponse_And_Map_Items()
        {
            // Arrange
            var items = new List<PropertyResult>
            {
                new(Guid.NewGuid(), " Casa Azul ", " Calle 1 ", 100_000, "A1", 2022),
                new(Guid.NewGuid(), "Casa Roja", "Calle 2", 200_000, "B2", 2021)
            }.AsReadOnly();

            const long total = 2;

            _serviceMock.Setup(s => s.SearchAsync(
                    "Casa",
                    "Dir",
                    10,
                    300_000,
                    3,
                    25,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((items, total));

            var query = new PropertiesQuery(
                "  Casa  ",      // Name
                "  Dir  ",       // Address
                10,              // MinPrice
                300_000,         // MaxPrice
                3,               // Page
                25               // PageSize
            );

            // Act
            var result = await _sut.List(query, CancellationToken.None);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(200);

            var payload = ok.Value as PagedResponse<PropertyResponse>;
            payload.Should().NotBeNull();
            payload!.Total.Should().Be(total);
            payload.Items.Should().HaveCount(2);
            payload.Items[0].Name.Should().Be(" Casa Azul ");
            payload.Items[0].Address.Should().Be(" Calle 1 ");

            _serviceMock.VerifyAll();
        }

        [Test]
        public async Task List_Should_Normalize_Page_And_PageSize_And_Trim_Filters_Before_Calling_Service()
        {
            // Arrange
            string? capturedName = null, capturedAddress = null;
            int capturedPage = -1, capturedPageSize = -1;

            _serviceMock.Setup(s => s.SearchAsync(
                    It.IsAny<string?>(), It.IsAny<string?>(),
                    It.IsAny<decimal?>(), It.IsAny<decimal?>(),
                    It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<CancellationToken>()))
                .Callback((string? name, string? address, decimal? minP, decimal? maxP, int page, int pageSize, CancellationToken _ ) =>
                {
                    capturedName = name;
                    capturedAddress = address;
                    capturedPage = page;
                    capturedPageSize = pageSize;
                })
                .ReturnsAsync((Array.Empty<PropertyResult>().ToList().AsReadOnly(), 0L));

            var query = new PropertiesQuery(
                "  A  ",
                "  B  ",
                null,
                null,
                0,          // invalid
                9999    // invalid
            );

            // Act
            var result = await _sut.List(query, CancellationToken.None);

            // Asser
            result.Should().BeOfType<OkObjectResult>();

            // Assert
            capturedName.Should().Be("A");
            capturedAddress.Should().Be("B");
            capturedPage.Should().Be(1);
            capturedPageSize.Should().Be(10);

            _serviceMock.VerifyAll();
        }

        [Test]
        public async Task GetById_Should_Return_Ok_With_Mapped_Response()
        {
            // Arrange
            var id = Guid.NewGuid();
            var pr = new PropertyResult(id, "Casa", "Calle", 500, "C1", 2020);

            _serviceMock.Setup(s => s.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(pr);

            // Act
            var result = await _sut.GetById(id, CancellationToken.None);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(200);

            var payload = ok.Value as PropertyResponse;
            payload.Should().NotBeNull();
            payload!.Id.Should().Be(id);
            payload.Name.Should().Be("Casa");
            payload.Address.Should().Be("Calle");
            payload.Price.Should().Be(500);
            payload.CodeInternal.Should().Be("C1");
            payload.Year.Should().Be(2020);

            _serviceMock.VerifyAll();
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using MillionApi.Application.Services.Property;
using MillionApi.Application.Common.Interfaces.Persistence;
using MillionApi.Application.Common.Exceptions;
using MillionApi.Domain.Entities;

namespace MillionApi.Application.Tests
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _repoMock = default!;
        private PropertyService _sut = default!; // System Under Test

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IPropertyRepository>(MockBehavior.Strict);
            _sut = new PropertyService(_repoMock.Object);
        }

        [Test]
        public async Task GetPropertyByNameAsync_ShouldReturnResult_WhenPropertyExists()
        {
            // Arrange
            var ct = new CancellationTokenSource().Token;
            var name = "Casa Azul";

            var entity = new Property
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = "Calle 123",
                Price = 1000000m,
                CodeInternal = "ABC-123",
                Year = 2020
            };

            _repoMock
                .Setup(r => r.GetByNameAsync(name, ct))
                .ReturnsAsync(entity);

            // Act
            var result = await _sut.GetPropertyByNameAsync(name, ct);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(entity.Id);
            result.Name.Should().Be(entity.Name);
            result.Address.Should().Be(entity.Address);
            result.Price.Should().Be(entity.Price);
            result.CodeInternal.Should().Be(entity.CodeInternal);
            result.Year.Should().Be(entity.Year);

            _repoMock.Verify(r => r.GetByNameAsync(name, ct), Times.Once);
        }

        [Test]
        public async Task GetPropertyByNameAsync_ShouldThrowNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var ct = new CancellationTokenSource().Token;
            var name = "Inexistente";

            _repoMock
                .Setup(r => r.GetByNameAsync(name, ct))
                .ReturnsAsync((Property?)null);

            // Act
            var act = async () => await _sut.GetPropertyByNameAsync(name, ct);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("*Property*Inexistente*");

            _repoMock.Verify(r => r.GetByNameAsync(name, ct), Times.Once);
        }

        [Test]
        public async Task GetPropertyByNameAsync_ShouldPassCancellationToken_ToRepository()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            var ct = cts.Token;
            var name = "Casa Azul";

            _repoMock
                .Setup(r => r.GetByNameAsync(name, ct))
                .ReturnsAsync((Property?)null);

            // Act / Assert
            var act = async () => await _sut.GetPropertyByNameAsync(name, ct);
            await act.Should().ThrowAsync<NotFoundException>();

            _repoMock.Verify(r => r.GetByNameAsync(name, ct), Times.Once);
        }
    }
}

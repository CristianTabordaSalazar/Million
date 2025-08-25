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
        private PropertyService _sut = default!;

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

        [Test]
        public async Task SearchAsync_Should_Map_Repository_Entities_To_PropertyResult_And_Return_Total()
        {
            // Arrange
            var entities = new[]
            {
                new MillionApi.Domain.Entities.Property
                {
                    Id = Guid.NewGuid(), Name = "Casa Azul", Address = "Calle 1",
                    Price = 100_000, CodeInternal = "A1", Year = 2022
                },
                new MillionApi.Domain.Entities.Property
                {
                    Id = Guid.NewGuid(), Name = "Casa Roja", Address = "Calle 2",
                    Price = 200_000, CodeInternal = "B2", Year = 2021
                }
            }.ToList().AsReadOnly();

            const long total = 42;

            _repoMock
                .Setup(r => r.SearchAsync("ca", "calle", 50_000, 250_000, 2, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync((entities, total));

            // Act
            var (items, t) = await _sut.SearchAsync(
                name: "ca",
                address: "calle",
                minPrice: 50_000,
                maxPrice: 250_000,
                page: 2,
                pageSize: 10,
                ct: CancellationToken.None);

            // Assert
            t.Should().Be(total);
            items.Should().HaveCount(2);
            items.Should().ContainSingle(x => x.Name == "Casa Azul" && x.Address == "Calle 1" && x.Price == 100_000 && x.CodeInternal == "A1" && x.Year == 2022);
            items.Should().ContainSingle(x => x.Name == "Casa Roja" && x.Address == "Calle 2" && x.Price == 200_000 && x.CodeInternal == "B2" && x.Year == 2021);

            _repoMock.VerifyAll();
        }

        [Test]
        public async Task GetByIdAsync_Should_Return_PropertyResult_When_Found()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new MillionApi.Domain.Entities.Property
            {
                Id = id, Name = "House", Address = "Street", Price = 123, CodeInternal = "X", Year = 2020
            };

            _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            // Act
            var result = await _sut.GetByIdAsync(id, CancellationToken.None);

            // Assert
            result.Id.Should().Be(id);
            result.Name.Should().Be("House");
            result.Address.Should().Be("Street");
            result.Price.Should().Be(123);
            result.CodeInternal.Should().Be("X");
            result.Year.Should().Be(2020);

            _repoMock.VerifyAll();
        }

        [Test]
        public async Task GetByIdAsync_Should_Throw_NotFoundException_When_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MillionApi.Domain.Entities.Property?)null);

            // Act
            Func<Task> act = async () => await _sut.GetByIdAsync(id, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*Property*")
                .WithMessage($"*{id}*");

            _repoMock.VerifyAll();
        }
    }
}

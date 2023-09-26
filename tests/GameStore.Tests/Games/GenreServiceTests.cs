using AutoMapper;
using Castle.Core.Resource;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using Moq;
using System.Linq.Expressions;

namespace GameStore.Tests.Games
{
    public class GenreServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Genre>> _repositoryMock;
        private readonly IGenreService _genreService;
        public GenreServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IRepository<Genre>>();
            _genreService = new GenreService(
                _mapperMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnCreatedResult()
        {
            // Arrange
            var genreCreationDto = new GenreCreationDto { Name = "Action" };

            var expectedGenre = new Genre
            {
                Id = 1,
                Name = "Action",
            };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Genre, bool>> predicate, string[] includes) => null);


            _mapperMock.Setup(m => m.Map<Genre>(genreCreationDto))
                .Returns(new Genre { Name = "Action" });

            _unitOfWorkMock.Setup(u => u.CreateTransactionAsync());

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Genre>()))
                .ReturnsAsync(new Genre { Id = 1, Name = "Action" });

            _unitOfWorkMock.Setup(u => u.SaveAsync());
            _unitOfWorkMock.Setup(u => u.CommitAsync());

            _mapperMock.Setup(m => m.Map<GenreResultDto>(It.IsAny<Genre>()))
               .Returns(new GenreResultDto { Id = 1, Name = "Action" });

            // Act
            var result = await _genreService.AddAsync(genreCreationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGenre.Id, result.Id);
            Assert.Equal(expectedGenre.Name, result.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowAlreadyExistException()
        {
            // Arrange
            var genreCreationDto = new GenreCreationDto { Name = "ExsistingGenre" };

            var returnGenre = new Genre
            {
                Id = 1,
                Name = "ExsistingGenre",
            };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Genre, bool>> predicate, string[] includes) => returnGenre);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _genreService.AddAsync(genreCreationDto));

            // Assert
            Assert.Equal(409, exception.Code);
            Assert.Equal("Genre already exists.", exception.Message);
        }

        [Fact]
        public async Task ModifyAsync_ShouldReturnUpdatedResult()
        {
            // Arrange
            var exsistedGenre = new Genre { Id = 1, Name = "RPG", CreatedAt = DateTime.UtcNow };
            
            var genreUpdateDto = new GenreUpdateDto { Name = "Action" };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Genre, bool>> predicate, string[] includes) => exsistedGenre);

            _mapperMock.Setup(m => m.Map(genreUpdateDto, exsistedGenre))
                .Returns(new Genre { Name = "Action" });

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Genre>()))
                .ReturnsAsync(new Genre { Id = 1, Name = "Action" });

            _unitOfWorkMock.Setup(u => u.SaveAsync());

            _mapperMock.Setup(m => m.Map<GenreResultDto>(It.IsAny<Genre>()))
               .Returns(new GenreResultDto { Id = 1, Name = "Action" });

            // Act
            var result = await _genreService.ModifyAsync(1, genreUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(exsistedGenre.Id, result.Id);
            Assert.Equal(genreUpdateDto.Name, result.Name);
        }

        [Fact]
        public async Task ModifyAsync_ShouldThrowNotFoundException()
        {
            // Arrange
            var exsistedGenre = new Genre { Id = 2, Name = "RPG", CreatedAt = DateTime.UtcNow };

            var genreUpdateDto = new GenreUpdateDto { Name = "Action" };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Genre, bool>> predicate, string[] includes) => null);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _genreService.ModifyAsync(1, genreUpdateDto));

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal("Genre not found.", exception.Message);
        }
    }
}

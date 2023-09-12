using AutoMapper;
using GameStore.Data.DbContexts;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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
            
            _repositoryMock.Setup(p => p.InsertAsync(It.IsAny<Genre>()))
                .ReturnsAsync(expectedGenre);

            // Act
            var result = await _genreService.AddAsync(genreCreationDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGenre.Id, result.Id);
            Assert.Equal(expectedGenre.Name, result.Name);
        }
    }
}

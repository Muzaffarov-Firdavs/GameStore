using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using System.Linq.Expressions;

namespace GameStore.Tests.Games
{
    public class GenreTestWithException
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Genre>> _repositoryMock;
        private readonly IGenreService _genreService;
        public GenreTestWithException()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IRepository<Genre>>();
            _genreService = new GenreService(
                _mapperMock.Object, _unitOfWorkMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnFilteredResults()
        {
            // Arrange
            IEnumerable<GenreResultDto> expectedResults = new List<GenreResultDto>()
            {
                new GenreResultDto {Id = 1, Name = "Action" },
                new GenreResultDto {Id = 2, Name = "RPG" },
                new GenreResultDto {Id = 3, Name = "Strategy" },
                new GenreResultDto {Id = 4, Name = "Thinkable" },
                new GenreResultDto {Id = 5, Name = "Race" },
                new GenreResultDto {Id = 6, Name = "Warfare" },
                new GenreResultDto {Id = 7, Name = "Fire" }
            };

            var genres = new List<Genre>()
            {
                new Genre {Id = 1, Name = "Action" },
                new Genre {Id = 2, Name = "RPG" },
                new Genre {Id = 3, Name = "Strategy" },
                new Genre {Id = 4, Name = "Thinkable" },
                new Genre {Id = 5, Name = "Race" },
                new Genre {Id = 6, Name = "Warfare" },
                new Genre {Id = 7, Name = "Fire" },
            }.AsQueryable();

            _repositoryMock
                .Setup(r => r.SelectAll(
                    It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .Returns((genres));

            _mapperMock.Setup(m => m.Map<IEnumerable<GenreResultDto>>(It.IsAny<IEnumerable<Genre>>()))
                .Returns(It.IsAny<IEnumerable<GenreResultDto>>());

            // Act
            var results = await _genreService.RetrieveAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(expectedResults, results);
            //foreach (var result in results)
            //{
            //    Assert.Contains(search, result.Name);
            //}
        }
    }
}

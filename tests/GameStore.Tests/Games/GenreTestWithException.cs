using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.DTOs.Games;
using GameStore.Service.DTOs.Genres;
using GameStore.Service.Interfaces.Files;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using MockQueryable.Moq;
using System.Linq.Expressions;

namespace GameStore.Tests.Games
{
    public class GenreTestWithException
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<IRepository<Game>> _repositoryMock;
        private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
        private readonly IGameService _gameService;

        public GenreTestWithException()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _imageServiceMock = new Mock<IImageService>();
            _repositoryMock = new Mock<IRepository<Game>>();
            _genreRepositoryMock = new Mock<IRepository<Genre>>();
            _gameService = new GameService(_mapperMock.Object,
                _unitOfWorkMock.Object, _imageServiceMock.Object,
                _repositoryMock.Object, _genreRepositoryMock.Object);
        }

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnFilteredResults()
        {
            // Arrange
            var search = "tor";

            var dtoResults = new List<GameResultDto>
            {
                new GameResultDto { Id = 1, Name = "Contor Strike", Description = "Lorem upsilum new ukamlum."},
                new GameResultDto { Id = 4, Name = "Terminator", Description = "Lorem upsilum new ukamlum."},
                new GameResultDto { Id = 6, Name = "FreeTor", Description = "Lorem upsilum new ukamlum."},
                new GameResultDto { Id = 9, Name = "Ahillus Tor - Back to Home", Description = "Lorem upsilum new ukamlum."},
            };

            var games = new List<Game>
            {
                new Game { Id = 1, Name = "Contor Strike", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 2, Name = "Call Of Duty", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 3, Name = "Minecraft", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 4, Name = "Terminator", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 5, Name = "Transformers", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 6, Name = "FreeTor", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 7, Name = "SteelRats", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 8, Name = "PUBG", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 9, Name = "Ahillus Tor - Back to Home", Description = "Lorem upsilum new ukamlum."},
            };

            var mockGames = games.AsQueryable().BuildMock();
            _repositoryMock.Setup(r => r.SelectAll(
                It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
            .Returns(mockGames);

            _mapperMock.Setup(m => m.Map<IEnumerable<GameResultDto>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(dtoResults);

            // Act
            var results = await _gameService.RetrieveAllAsync(search);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(dtoResults, results);
            foreach (var result in results)
            {
                Assert.Contains(search, result.Name.ToLower());
            }
        }
    }
}

using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Games;
using GameStore.Service.Interfaces.Files;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using MockQueryable.Moq;
using System.Linq.Expressions;

namespace GameStore.Tests.Games
{
    public class GameServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly Mock<IRepository<Game>> _repositoryMock;
        private readonly Mock<IRepository<Genre>> _genreRepositoryMock;
        private readonly IGameService _gameService;

        public GameServiceTests()
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
        public async Task AddAsync_ShouldreturnResult()
        {
            // Arrange
            var expectedResult = new Game { Id = 1, Name = "Test" };
            var dto = new GameCreationDto { Name = "Test" };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => null);

            _mapperMock.Setup(m => m.Map<Game>(dto))
                .Returns(new Game { Name = "Action" });

            // set up genres with moq package
            List<Genre> genres = new List<Genre>();
            var mockGenres = genres.AsQueryable().BuildMock();
            _genreRepositoryMock.Setup(g => g.SelectAll(
                It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .Returns(mockGenres);

            
            _unitOfWorkMock.Setup(u => u.CreateTransactionAsync());

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Game>()))
                .ReturnsAsync(expectedResult);

            _unitOfWorkMock.Setup(u => u.SaveAsync());
            _unitOfWorkMock.Setup(u => u.CommitAsync());

            _mapperMock.Setup(m => m.Map<GameResultDto>(It.IsAny<Game>()))
               .Returns(new GameResultDto { Id = 1, Name = "Test" });

            // Act
            var result = await _gameService.AddAsync(dto, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        [Fact]
        public async Task ModifyAsync_ShouldThrowException()
        {
            // Arrange
            long id = 1;
            var dto = new GameUpdateDto { Name = "Test" };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => null);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _gameService.ModifyAsync(id, dto));

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal("Game is not found.", exception.Message);
        }

        [Fact]
        public async Task ModifyAsync_ShouldReturnResult()
        {
            // Arrange
            var existGame = new Game { Id = 1, Name = "Contor" };

            long id = 1;
            var dto = new GameUpdateDto { Name = "Test" };

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => existGame);

            _mapperMock.Setup(m => m.Map(dto, existGame))
                .Returns(new Game { Id = 1, Name = "Test" });

            // set up genres with moq package
            List<Genre> genres = new List<Genre>();
            var mockGenres = genres.AsQueryable().BuildMock();
            _genreRepositoryMock.Setup(g => g.SelectAll(
                It.IsAny<Expression<Func<Genre, bool>>>(), It.IsAny<string[]>()))
                .Returns(mockGenres);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Game>()))
                .ReturnsAsync(new Game { Id = 1, Name = "Test" });

            _unitOfWorkMock.Setup(u => u.SaveAsync());

            _mapperMock.Setup(m => m.Map<GameResultDto>(It.IsAny<Game>()))
               .Returns(new GameResultDto { Id = 1, Name = "Test" });

            // Act
            var result = await _gameService.ModifyAsync(1, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existGame.Id, result.Id);
            Assert.Equal(dto.Name, result.Name);
            Assert.NotEqual(existGame.Name, result.Name);
        }

        [Fact]
        public async Task RemoveByIdAsync_ShouldThrowException()
        {
            // Arrange
            long id = 1;

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => null);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _gameService.RemoveByIdAsync(id));

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal("Game is not found.", exception.Message);
        }

        [Fact]
        public async Task RemoveByIdAsync_ShouldReturnResult()
        {
            // Arrange
            var existGame = new Game { Id = 1, Name = "Contor" };
            long id = 1;

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => existGame);

            _repositoryMock.Setup(r => r.DeleteAsync(existGame));
            _unitOfWorkMock.Setup(u => u.SaveAsync());

            // Act
            var result = await _gameService.RemoveByIdAsync(id);

            // Assert
            Assert.True(result);
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

        [Fact]
        public async Task RetrieveAllAsync_ShouldReturnAllResults()
        {
            // Arrange
            var dtoResults = new List<GameResultDto>
            {
                new GameResultDto { Id = 1, Name = "Contor Strike", Description = "Lorem upsilum new ukamlum."},
                new GameResultDto { Id = 2, Name = "Call Of Duty", Description = "Lorem upsilum new ukamlum."},
                new GameResultDto { Id = 3, Name = "Minecraft", Description = "Lorem upsilum new ukamlum."},
            };

            var games = new List<Game>
            {
                 new Game { Id = 1, Name = "Contor Strike", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 2, Name = "Call Of Duty", Description = "Lorem upsilum new ukamlum."},
                new Game { Id = 3, Name = "Minecraft", Description = "Lorem upsilum new ukamlum."},
            };

            var mockGames = games.AsQueryable().BuildMock();
            _repositoryMock.Setup(r => r.SelectAll(
                It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
            .Returns(mockGames);

            _mapperMock.Setup(m => m.Map<IEnumerable<GameResultDto>>(It.IsAny<IEnumerable<Game>>()))
                .Returns(dtoResults);

            // Act
            var results = await _gameService.RetrieveAllAsync();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(dtoResults, results);
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldThrowException()
        {
            // Arrange
            long id = 1;

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => null);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _gameService.RetrieveByIdAsync(id));

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal("Game is not found.", exception.Message);
        }

        [Fact]
        public async Task RetrieveByIdAsync_ShouldReturnResult()
        {
            var existGame = new Game { Id = 1, Name = "Contor" };
            long id = 1;

            _repositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => existGame);

            _mapperMock.Setup(m => m.Map<GameResultDto>(It.IsAny<Game>()))
               .Returns(new GameResultDto { Id = 1, Name = "Contor" });

            // Act 
            var result = await _gameService.RetrieveByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existGame.Id, result.Id);
            Assert.Equal(existGame.Name, result.Name);
        }
    }
}

using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Users;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.Games;
using GameStore.Service.DTOs.Users;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;
using MockQueryable.Moq;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace GameStore.Tests.Games
{
    public class CommentServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<Comment>> _commentRepositoryMock;
        private readonly ICommentService _commentService;

        public CommentServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _commentRepositoryMock = new Mock<IRepository<Comment>>();
            _commentService = new CommentService(_mapperMock.Object,
                _unitOfWorkMock.Object, _userRepositoryMock.Object,
                _gameRepositoryMock.Object, _commentRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateAndReturnResult()
        {
            // Arrange
            var expectedResult = new CommentResultDto
            {
                Id = 1,
                Text = "Cool!",
                User = new UserResultDto { Id = 1, FirstName = "Firdavs", LastName = "Muzaffarov" }
            };
            var inputComment = new Comment { Text = "Cool!", UserId = 1, GameId = 1 };

            var user = new User { Id = 1, FirstName = "Firdavs", LastName = "Muzaffarov" };
            _userRepositoryMock
               .Setup(r => r.SelectAsync(
                   It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string[]>()))
               .ReturnsAsync((Expression<Func<User, bool>> predicate, string[] includes) => user);

            var game = new Game { Id = 1, Name = "Ahllius Promo Hero" };
            _gameRepositoryMock
               .Setup(r => r.SelectAsync(
                   It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
               .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => game);


            _unitOfWorkMock.Setup(u => u.CreateTransactionAsync());

            _commentRepositoryMock.Setup(r => r.InsertAsync(It.IsAny<Comment>()))
                .ReturnsAsync(
                    new Comment
                    {
                        Id = 1,
                        Text = "Cool!",
                        GameId = 1,
                        Game = game,
                        UserId = 1,
                        User = user
                    });

            _unitOfWorkMock.Setup(u => u.SaveAsync());
            _unitOfWorkMock.Setup(u => u.CommitAsync());

            _mapperMock.Setup(m => m.Map<CommentResultDto>(It.IsAny<Comment>()))
               .Returns(
                    new CommentResultDto
                    {
                        Id = 1,
                        Text = "Cool!",
                        User = new UserResultDto { Id = 1, FirstName = "Firdavs", LastName = "Muzaffarov" }
                    });

            // Act
            var result = await _commentService.AddAsync(inputComment);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Text, result.Text);
            Assert.Equal(expectedResult.User.Id, result.User.Id);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowCustomException()
        {
            // Arrange
            var inputComment = new Comment { Text = "   ", UserId = 1, GameId = 1 };

            var user = new User { Id = 1, FirstName = "Firdavs", LastName = "Muzaffarov" };
            _userRepositoryMock
               .Setup(r => r.SelectAsync(
                   It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<string[]>()))
               .ReturnsAsync((Expression<Func<User, bool>> predicate, string[] includes) => user);

            var game = new Game { Id = 1, Name = "Ahllius Promo Hero" };
            _gameRepositoryMock
               .Setup(r => r.SelectAsync(
                   It.IsAny<Expression<Func<Game, bool>>>(), It.IsAny<string[]>()))
               .ReturnsAsync((Expression<Func<Game, bool>> predicate, string[] includes) => game);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _commentService.AddAsync(inputComment));

            // Assert
            Assert.Equal(422, exception.Code);
            Assert.Equal("Text should not be whitespace or empty.", exception.Message);
        }

        [Fact]
        public async Task ModifyAsync_ShouldThrowException()
        {
            // Arrange
            long id = 1;
            var dto = new CommentUpdateDto { Text = "This is great game!" };

            _commentRepositoryMock
                .Setup(r => r.SelectAsync(
                    It.IsAny<Expression<Func<Comment, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Expression<Func<Comment, bool>> predicate, string[] includes) => null);

            // Act
            var exception = await Assert.ThrowsAsync<CustomException>(
                async () => await _commentService.ModifyAsync(id, dto));

            // Assert
            Assert.Equal(404, exception.Code);
            Assert.Equal("Comment is not found.", exception.Message);
        }

        [Fact]
        public async Task ModifyAsync_ShouldUpdateAndReturnResult()
        {
            // Arrange
            var userDto = new UserResultDto { Id = 1, FirstName = "Firdavs", LastName = "Muzaffarov" };
            var expectedResult = new CommentResultDto
            {
                Id = 1,
                Text = "This is great game!",
                User = userDto
            };

            var commentId = 1;
            var commentDto = new CommentUpdateDto { Text = "This is great game!" };

            var user = new User { Id = 1, FirstName = "Firdavs", LastName = "Muzaffarov" };
            var existComment = new Comment
            {
                Id = 1,
                Text = "Game is goood.",
                User = user
            };

            _commentRepositoryMock
                .Setup(r => r.SelectAsync(
                   It.IsAny<Expression<Func<Comment, bool>>>(), It.IsAny<string[]>()))
               .ReturnsAsync((Expression<Func<Comment, bool>> predicate, string[] includes) => existComment);

            _mapperMock.Setup(m => m.Map(commentDto, existComment))
                .Returns(new Comment { Id = 1, Text = "This is great game!", UserId = 1, User = user });

            _unitOfWorkMock.Setup(u => u.SaveAsync());

            _mapperMock.Setup(m => m.Map<CommentResultDto>(It.IsAny<Comment>()))
               .Returns(
                    new CommentResultDto
                    {
                        Id = 1,
                        Text = "This is great game!",
                        User = userDto
                    });

            // Act
            var result = await _commentService.ModifyAsync(commentId, commentDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.User);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Text, result.Text);
            Assert.Equal(expectedResult.User.Id, result.User.Id);
        }
    }
}

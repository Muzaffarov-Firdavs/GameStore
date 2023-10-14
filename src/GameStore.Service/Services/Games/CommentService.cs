using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Users;
using GameStore.Service.Commons.Exceptions;
using GameStore.Service.DTOs.Comments;
using GameStore.Service.Interfaces.Games;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Service.Services.Games
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Comment> _commentRepository;

        public CommentService(IMapper mapper,
            IUnitOfWork unitOfWork,
            IRepository<User> userRepository,
            IRepository<Game> gameRepository,
            IRepository<Comment> commentRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _commentRepository = commentRepository;
        }

        public async ValueTask<CommentResultDto> AddAsync(Comment dto)
        {
            var user = await _userRepository.SelectAsync(u => u.Id == dto.UserId && !u.IsDeleted);
            if (user == null)
                throw new CustomException(404, "User is not found");

            var game = await _gameRepository.SelectAsync(g => g.Id == dto.GameId && !g.IsDeleted);
            if (game == null)
                throw new CustomException(404, "Game is not found");

            if (string.IsNullOrWhiteSpace(dto.Text))
                throw new CustomException(404, "Text should not be whitespace or empty.");
            
            var mappedComment = _mapper.Map<Comment>(dto);
            mappedComment.CreatedAt = DateTime.UtcNow;

            if (mappedComment.Parent != null)
                mappedComment.Parent = await _commentRepository.SelectAsync(
                        c => c.Id == mappedComment.Parent.Id && !c.IsDeleted);

            await _unitOfWork.CreateTransactionAsync();
            var result = await _commentRepository.InsertAsync(mappedComment);
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CommentResultDto>(result);
        }

        public async ValueTask<CommentResultDto> ModifyAsync(long id, CommentUpdateDto dto)
        {
            var comment = await _commentRepository.SelectAsync(p => p.Id == id && !p.IsDeleted,
                new string[] { "User" });
            if (comment == null)
                throw new CustomException(404, "Comment is not found.");

            if (string.IsNullOrWhiteSpace(dto.Text))
                throw new CustomException(404, "Text should not be whitespace or empty.");

            var mappedComment = _mapper.Map(dto, comment);
            mappedComment.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CommentResultDto>(mappedComment);
        }

        public async ValueTask<bool> RemoveByIdAsync(long id)
        {
            var comment = await _commentRepository.SelectAsync(p => p.Id == id && !p.IsDeleted);
            if (comment == null)
                throw new CustomException(404, "Comment is not found.");

            await _commentRepository.DeleteAsync(comment);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async ValueTask<IEnumerable<CommentResultDto>> RetrieveAllAsync(string search = null)
        {
            var comments = await _commentRepository.SelectAll(p => !p.IsDeleted)
                .Include(p => p.User)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(search))
                comments = comments.FindAll(p => p.Text.ToLower()
                .Contains(search.ToLower()));

            return _mapper.Map<IEnumerable<CommentResultDto>>(comments);
        }

        public async ValueTask<CommentResultDto> RetrieveByIdAsync(long id)
        {
            var comment = await _commentRepository.SelectAsync(p => p.Id == id && !p.IsDeleted,
                new string[] { "User" });
            if (comment == null)
                throw new CustomException(404, "Comment is not found.");

            return _mapper.Map<CommentResultDto>(comment);
        }
    }
}

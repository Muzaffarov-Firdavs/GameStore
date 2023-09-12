using GameStore.Service.DTOs.Comments;
using GameStore.Service.Interfaces.Games;

namespace GameStore.Service.Services.Games
{
    public class CommentService : ICommentService
    {
        public ValueTask<CommentResultDto> AddAsync(CommentCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<CommentResultDto> ModifyAsync(long id, CommentUpdateDto dto)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> RemoveByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<CommentResultDto>> RetrieveAllAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<CommentResultDto> RetrieveByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}

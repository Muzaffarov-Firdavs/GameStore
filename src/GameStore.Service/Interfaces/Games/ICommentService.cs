using GameStore.Domain.Entities.Games;
using GameStore.Service.DTOs.Comments;

namespace GameStore.Service.Interfaces.Games
{
    public interface ICommentService
    {
        ValueTask<CommentResultDto> AddAsync(Comment dto);
        ValueTask<CommentResultDto> ModifyAsync(long id, CommentUpdateDto dto);
        ValueTask<bool> RemoveByIdAsync(long id);
        ValueTask<Comment> RetrieveByIdAsync(long id);
        ValueTask<IEnumerable<CommentResultDto>> RetrieveAllAsync(string search = null);
    }
}
using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.SubComments;

namespace GameStore.Service.Interfaces.Games
{
    public interface ICommentService
    {
        ValueTask<CommentResultDto> AddAsync(CommentCreationDto dto);
        ValueTask<CommentResultDto> ModifyAsync(long id, CommentUpdateDto dto);
        ValueTask<bool> RemoveByIdAsync(long id);
        ValueTask<CommentResultDto> RetrieveByIdAsync(long id);
        ValueTask<IEnumerable<CommentResultDto>> RetrieveAllAsync(string search = null);
        ValueTask<SubCommentResultDto> AddAsync(SubCommentCreationDto dto);
    }
}

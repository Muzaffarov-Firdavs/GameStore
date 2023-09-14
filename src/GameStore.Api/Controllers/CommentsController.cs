using GameStore.Service.DTOs.Comments;
using GameStore.Service.Interfaces.Games;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CommentCreationDto dto)
            => Ok(await _commentService.AddAsync(dto));

        [HttpPut("id")]
        public async Task<IActionResult> PutAsync(long id, CommentUpdateDto dto)
            => Ok(await _commentService.ModifyAsync(id, dto));

        [HttpDelete("id")]
        public async Task<IActionResult> DaleteByIdAsync(long id)
            => Ok(await _commentService.RemoveByIdAsync(id));

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(await _commentService.RetrieveByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search = null)
            => Ok(await _commentService.RetrieveAllAsync(search));

    }
}

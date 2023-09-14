using GameStore.Service.DTOs.Users;
using GameStore.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(UserCreationDto dto)
            => Ok(await _userService.AddAsync(dto));

        [HttpPut("id")]
        public async Task<IActionResult> PutAsync(long id, UserUpdateDto dto)
            => Ok(await _userService.ModifyAsync(id, dto));

        [HttpDelete("id")]
        public async Task<IActionResult> DaleteByIdAsync(long id)
            => Ok(await _userService.RemoveByIdAsync(id));

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(await _userService.RetrieveByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string? search = null)
            => Ok(await _userService.RetrieveAllAsync(search));
    }
}

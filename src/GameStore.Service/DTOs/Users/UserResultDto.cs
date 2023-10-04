using GameStore.Service.DTOs.Files;

namespace GameStore.Service.DTOs.Users
{
    public class UserResultDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public ImageResultDto Image { get; set; }
    }
}

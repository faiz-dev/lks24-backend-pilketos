using static BackendPilketos.Models.User;

namespace Backend1.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string DisplayName { get; set; }
        public RoleType? Role { get; set; }
        public int? GroupId { get; set; }

    }
}

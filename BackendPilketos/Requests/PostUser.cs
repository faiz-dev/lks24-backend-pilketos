using static BackendPilketos.Models.User;

namespace BackendPilketos.Requests
{
    public class PostUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public int? GroupId { get; set; }
    }
}

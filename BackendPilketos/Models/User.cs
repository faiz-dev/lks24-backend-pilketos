namespace BackendPilketos.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public UserGroup? Group { get; set; }
        
        public enum RoleType
        {
            USER, ADMIN, GURU, SISWA
        }
    }
}

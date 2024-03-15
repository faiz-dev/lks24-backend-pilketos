using Microsoft.EntityFrameworkCore;
using static BackendPilketos.Models.User;
namespace BackendPilketos.Models
{
    public class Periode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
        public string RoleTypes { get; set; } = String.Empty;
        public string Groups { get; set; } = String.Empty;
        public string? WaktuBerakhir { get; set; } = String.Empty;
    }
}

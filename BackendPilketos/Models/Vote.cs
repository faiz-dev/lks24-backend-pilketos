namespace BackendPilketos.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public string Waktu { get; set; }
        public User User { get; set; }
        public Calon Pilihan { get; set; }
        public Periode? Periode { get; set; }
    }
}

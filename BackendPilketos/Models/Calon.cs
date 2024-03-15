namespace BackendPilketos.Models
{
    public class Calon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string NoUrut { get; set; } = string.Empty;
        public string Photo { get; set; }  = string.Empty;
        public string VisiMisi { get; set; } = string.Empty;
        public string VisiMisiUrl { get; set; } = string.Empty;
        public Periode Periode { get; set; }

    }
}

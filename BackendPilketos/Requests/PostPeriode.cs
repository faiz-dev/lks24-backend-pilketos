namespace BackendPilketos.Requests
{
    public class PostPeriode
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        //public List<int>? GroupIds { get; set; }
        public string? GroupIds { get; set; }
        public string? WaktuBerakhir { get; set; }
    }
}

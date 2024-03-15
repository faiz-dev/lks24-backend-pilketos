namespace BackendPilketos.Models
{
    public class VWVoteUserGroup
    {
        public int Id { get; set; }
        public string Waktu { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int PeriodeId { get; set; }
    }
}

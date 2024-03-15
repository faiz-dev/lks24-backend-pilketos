using BackendPilketos.Models;

namespace BackendPilketos.Requests
{
    public class PostVote
    {
        public int pilihanId { get; set; }
        public int periodeId { get; set; }
    }
}

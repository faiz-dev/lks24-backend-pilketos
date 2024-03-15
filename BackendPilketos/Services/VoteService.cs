using BackendPilketos.Models;
using BackendPilketos.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BackendPilketos.Services
{
    public class VoteService
    {
        private DataContext _context;

        public VoteService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Vote>> Get()
        {
            var votes = await _context.Votes.Include(v => v.User.Group).Include(v => v.Periode).ToListAsync();
            return votes;
        }

        public async Task<Vote> Create(User user, Calon pilihan, Periode periode)
        {
            DateTime dateTime = DateTime.Now;
            Vote? vote = new Vote
            {
                User = user,
                Pilihan = pilihan,
                Periode = periode,
                Waktu = dateTime.ToString("yyyyMMddHHmmssfff"),
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<int> Count()
        {
            var count = await _context.Votes.CountAsync();
            return count;
        }

        public async Task<List<Vote>> GetVotes(User user)
        {
            var votes = await _context.Votes
                    .Where(v => v.User == user)
                    .Include(v => v.Periode)
                    .ToListAsync();
            return votes;
        }

        public async Task<int> CountByUserPeriode(Periode periode, User user)
        {
            var ct = await _context.Votes
                .Where(v => v.Periode == periode)
                .Where(v => v.User == user)
                .CountAsync();
            return ct;
        }

        public async Task<List<VoteResult>> CountVoteByPeriode(Periode periode)
        {
            var count = await _context.Votes.Where(v => v.Periode == periode).CountAsync();
            var calons = await _context.Calons.Where(v => v.Periode == periode).ToListAsync();
            List<VoteResult> vr = new List<VoteResult>();

            foreach (var calon in calons)
            {
                var countCalon = await _context.Votes
                    .Where(v => v.Periode == periode)
                    .Where(v => v.Pilihan == calon)
                    .CountAsync();
                vr.Add(new VoteResult
                {
                    VoteCount = countCalon,
                    TotalCount = count,
                    Calon = calon
                });
            };

            return vr;
        }

        public async Task<List<UserVoted>> GetUsersVotedByPeriode(int periodeId, int groupId)
        {
            List<UserVoted> usrs = await _context.VwUserAndVotes
                    .Where(v => v.PeriodeId == periodeId)
                    .Where(v => v.GroupId == groupId)
                    .Select(v => new UserVoted { Name = v.UserName, Email = v.Email, GroupName = v.GroupName, UserId = v.UserId})
                    .ToListAsync();
            return usrs;
        }

        public async Task<int> Count(Calon calon)
        {
            var count = await _context.Votes.CountAsync(v => v.Pilihan == calon);
            return count;
        }

        public async Task<List<GroupCountResult>> CountByPeriodeId(int periodeId) {
            var data = await _context
                    .VwVoteUserGroups
                    .Where(g => g.PeriodeId == periodeId)
                    .GroupBy(g => g.GroupId)
                    .Select(voteGroup => new GroupCountResult{Count = voteGroup.Count(), GroupId = voteGroup.Key, GroupName = voteGroup.Select(g => g.GroupName).FirstOrDefault()})
                    .ToListAsync();
            return data;

        }

        public class UserVoted
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string GroupName { get; set; }
            public int UserId { get; set; }
        }

        public class GroupCountResult
        {
            public int Count { get; set; }
            public int GroupId { get; set; }
            public string GroupName { get; set; }
        }

        public class VoteResult
        {
            public Calon Calon { get; set; }
            public int TotalCount { get; set; }
            public int VoteCount { get; set; }

        }


    }
}
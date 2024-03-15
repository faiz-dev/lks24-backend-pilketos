using BackendPilketos.Services;
using BackendPilketos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BackendPilketos.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BackendPilketos.Exceptions;

namespace BackendPilketos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private UserService _userService;
        private CalonService _calonService;
        private PeriodeService _periodeService;
        private VoteService _voteService;
        private GroupService _groupService;

        public VoteController(DataContext context)
        {
            _userService = new UserService(context);
            _calonService = new CalonService(context);
            _periodeService = new PeriodeService(context);
            _voteService = new VoteService(context);
            _groupService = new GroupService(context);
        }

        [HttpGet]
        public async Task<ActionResult<int>> CountVotes()
        {
            int count = await _voteService.Count();
            return Ok(count);
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<Vote>>> GetAll()
        {
            var votes = await _voteService.Get();
            return Ok(votes);
        }

        [HttpGet("periode-ids")]
        [Authorize]
        public async Task<ActionResult<List<int>>> GetVotedPeriodeIdsByUser()
        {
            User user = await _userService.Get(User?.Identity?.Name);
            var votes = await _voteService.GetVotes(user);
            var filteredVotes = votes.Select(v => v.Periode.Id).ToList();
            return Ok(filteredVotes);
        }

        [HttpGet("calon/{id}")]
        public async Task<ActionResult<int>> CountVotes(int calonId)
        {
            Calon calon = await _calonService.Get(calonId);
            int count = await _voteService.Count(calon);
            return count;
        }

        [HttpGet("periode/{periodeId}")]
        public async Task<ActionResult> CountVoteByPeriode(int periodeId)
        {
            Periode periode = await _periodeService.Get(periodeId);
            var vr = await _voteService.CountVoteByPeriode(periode);
            return Ok(vr);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> Vote(PostVote postVote)
        {
            try
            {
                Calon calon = await _calonService.Get(postVote.pilihanId);
                User user = await _userService.Get(User?.Identity?.Name);
                Periode periode = await _periodeService.Get(postVote.periodeId);
                int ct = await _voteService.CountByUserPeriode(periode, user);
                if (ct > 0)
                {
                    return BadRequest("Anda sudah pernah memilih");
                }
                Vote vote = await _voteService.Create(user, calon, periode);
                return Ok();
            } catch (InvariantError err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost("cekvote")]
        public async Task<ActionResult> CekVote(int periodeId, int userId)
        {
            Periode periode = await _periodeService.Get(periodeId);
            User user = await _userService.Get(userId);
            var ct = await _voteService.CountByUserPeriode(periode, user);
            return Ok(ct);
        }

        [HttpGet("voted/{periodeId}/{groupId}")]
        public async Task<ActionResult> Get(int periodeId, int groupId)
        {
            try
            {
                Periode periode = await _periodeService.Get(periodeId);
                UserGroup group = await _groupService.Get(groupId);
                List<User> users = await _userService.GetByGroup(group);
                var userVoted = await _voteService.GetUsersVotedByPeriode(periodeId, groupId);


                return Ok(new { userVoted, jmlUser = users.Count(), allUsers = users });
            } catch (InvariantError err) {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("rekap-periode/{periodeId}")]
        public async Task<ActionResult> Get(int periodeId)
        {
            try
            {
                Periode periode = await _periodeService.Get(periodeId);
                List<GroupCount> groupCounts = await _userService.CountMemberEachGroup();
                var data = await _voteService.CountByPeriodeId(periodeId);

                return Ok(new {dataGroup = groupCounts, dataGroupVotes = data });
            }
            catch (InvariantError err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}

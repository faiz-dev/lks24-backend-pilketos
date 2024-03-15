using BackendPilketos.Exceptions;
using BackendPilketos.Models;
using BackendPilketos.Requests;
using BackendPilketos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BackendPilketos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        private GroupService _groupService;

        public UserController(DataContext context)
        {
            _userService = new UserService(context);
            _groupService = new GroupService(context);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                List<User> users = await _userService.Get();
                return Ok(users);
            } catch (InvariantError err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            try
            {
                User user = await _userService.Get(id);
                return user;
            } catch (InvariantError err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<List<User>>> GetByGroup(int groupId)
        {
            try
            {
                UserGroup group = await _groupService.Get(groupId);
                List<User> users = await _userService.GetByGroup(group);
                return Ok(users);
            } catch (InvariantError err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<User>> Create(PostUser postUser)
        {

            try
            {
                User existingUser = await _userService.Get(postUser.Email);
                return BadRequest("User sudah terdaftar");
            } catch (InvariantError err) {}


            try
            {
                UserGroup? group = null;
                if (postUser.GroupId != null)
                {
                    group = await _groupService.Get(postUser.GroupId.GetValueOrDefault(0));
                }

                User user = await _userService.Create(postUser, group);
                return Ok(user);
            } catch (InvariantError err) {
                return BadRequest("Group tidak ditemukan");
            }
            

        }

        [HttpPost("bulk")]
        [Authorize]
        public async Task<ActionResult<List<User>>> CreateBulk(List<PostUser> postUsers, int? groupId)
        {
            UserGroup? group = null;
            if (groupId != null)
            {
                try
                {
                    group = await _groupService.Get(groupId.GetValueOrDefault(0));
                } catch (InvariantError err) { 
                    return BadRequest(err.Message);
                }
            }

            try
            {
                await _userService.CheckEmails(postUsers);
            } catch(InvariantError err)
            {
                return BadRequest(err.Message);
            }

            List<User> users = await _userService.Create(postUsers, group);
            return Ok("User berhasil ditambahkan");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<User>> Update(int id, PostUser postUser)
        {
            UserGroup? group = null;
            if (postUser.GroupId != null)
            {
                try
                {
                    group = await _groupService.Get(postUser.GroupId.GetValueOrDefault(0));
                }
                catch (InvariantError err)
                {
                    return BadRequest(err.Message);
                }
            }

            User users = await _userService.Update(id, postUser, group);
            return users;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }


        [HttpGet("count-each-group")]
        public async Task<ActionResult> GetCount()
        {
            var data = await _userService.CountMemberEachGroup();
            return Ok(data);
        }
    }
}

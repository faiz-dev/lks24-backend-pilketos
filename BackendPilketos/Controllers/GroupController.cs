using BackendPilketos.Models;
using BackendPilketos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BackendPilketos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private GroupService _groupService;
        public GroupController(DataContext context) 
        { 
            _groupService = new GroupService(context);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserGroup>>> Get()
        {
            var groups = await _groupService.Get();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroup>> Get(int id)
        {
            return await _groupService.Get(id);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<List<UserGroup>>> Create(string name)
        {
            var group = await _groupService.Create(name);
            return Ok(group);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<UserGroup>> Update(int id, string name)
        {
            var group = await _groupService.Update(id, name);
            return Ok(group);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(int id)
        {
            await _groupService.Delete(id);
            return Ok();
        }

    }
}

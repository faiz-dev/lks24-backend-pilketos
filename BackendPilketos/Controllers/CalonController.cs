using BackendPilketos.Models;
using BackendPilketos.Services;
using BackendPilketos.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackendPilketos.Exceptions;
using System.Collections.Generic;

namespace BackendPilketos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalonController : ControllerBase
    {
        private CalonService _calonService;
        private PeriodeService _periodeService;
        public CalonController(DataContext context)
        {
            _calonService = new CalonService(context);
            _periodeService = new PeriodeService(context);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Calon>>> Get()
        {
            List<Calon> calons = await _calonService.Get();
            return Ok(calons);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Calon>> Get(int id)
        {
            Calon calon = await _calonService.Get(id);
            return Ok(calon);
        }

        [HttpGet("periode/{periodeId}")]
        [Authorize]
        public async Task<ActionResult<List<Calon>>> GetByPeriode(int periodeId)
        {
            try
            {
                Periode? periode = await _periodeService.Get(periodeId);
                List<Calon> calons = await _calonService.GetByPeriode(periode!);

                return calons;
            } catch (InvariantError err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Calon>> Create(PostCalon postCalon)
        {
            try
            {
                Periode? periode = await _periodeService.Get(postCalon.PeriodeId);
                Calon calon = await _calonService.Create(postCalon, periode!);
                return Ok(calon);
            } catch (InvariantError ex) { 
                return BadRequest(ex.Message);
            }

            
            
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Calon>> Update(int id, PostCalon postCalon)
        {
            Calon calon = await _calonService.Update(id, postCalon);
            return Ok(calon);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(int id)
        {
            await _calonService.Delete(id);
            return Ok();
        }
    }
}

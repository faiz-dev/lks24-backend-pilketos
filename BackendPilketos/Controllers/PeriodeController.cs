using BackendPilketos.Models;
using BackendPilketos.Requests;
using BackendPilketos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendPilketos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodeController : ControllerBase
    {
        private PeriodeService _periodeService;
        public PeriodeController(DataContext context)
        {
            _periodeService = new PeriodeService(context);
        }

        [HttpGet]
        public async Task<ActionResult<List<Periode>>> Get()
        {
            List<Periode> periode = await _periodeService.Get();
            return Ok(periode);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Periode?>> Get(int id)
        {
            Periode? periode = await _periodeService.Get(id);
            return Ok(periode);   
        }

        [HttpPost]
        public async Task<ActionResult<Periode>> Create(PostPeriode postPeriode)
        {
            Periode periode = await _periodeService.Create(postPeriode);
            return Ok(periode);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Periode>> Update(int id, PostPeriode postPeriode)
        {
            Periode periode = await _periodeService.Update(id, postPeriode);
            return Ok(periode);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(int id)
        {
            await _periodeService.Delete(id);
            return Ok();
        }
    }
}

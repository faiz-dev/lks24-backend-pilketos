using BackendPilketos.Models;
using BackendPilketos.Requests;
using BackendPilketos.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BackendPilketos.Services
{
    public class PeriodeService
    {
        private DataContext _context;

        public PeriodeService(DataContext context) 
        {
            _context = context;
        }

        public async Task<List<Periode>> Get()
        {
            List<Periode> periodes = await _context.Periodes.ToListAsync();
            return periodes;
        }

        public async Task<Periode?> Get(int id)
        {
            Periode? periodes = await _context.Periodes.FindAsync(id);
            if (periodes == null)
                throw new InvariantError("Data Periode tidak ditemukan");
            return periodes;
        }

        public async Task<Periode> Create(PostPeriode postPeriode)
        {
            var periode = new Periode
            {
                Name = postPeriode.Name,
                Groups = postPeriode.GroupIds == null ? "" : postPeriode.GroupIds,
                IsActive = postPeriode.IsActive.GetValueOrDefault(true),
                WaktuBerakhir = postPeriode.WaktuBerakhir
            };

            _context.Periodes.Add(periode);
            await _context.SaveChangesAsync();
            return periode;
        }

        public async Task<Periode> Update(int id, PostPeriode postPeriode)
        {
            var periode = await _context.Periodes.FindAsync(id);
            if (periode == null)
                throw new InvariantError("Data Periode tidak ditemukan");

            periode.Name = postPeriode.Name;
            periode.IsActive = (postPeriode.IsActive == null || postPeriode.IsActive == false) ? false : true;
            periode.Groups = postPeriode.GroupIds;
            periode.WaktuBerakhir = postPeriode.WaktuBerakhir;

            await _context.SaveChangesAsync();
            return periode;
        }

        public async Task Delete(int id)
        {
            var periode = await _context.Periodes.FindAsync(id);
            if (periode == null)
                throw new InvariantError("Data Periode tidak ditemukan");

            _context.Periodes.Remove(periode);
            await _context.SaveChangesAsync();
        }
    }
}

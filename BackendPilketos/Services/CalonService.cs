using BackendPilketos.Models;
using BackendPilketos.Requests;
using BackendPilketos.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BackendPilketos.Services
{
    public class CalonService
    {
        private DataContext _context;
        public CalonService(DataContext context)
        {
            _context = context;
        }

        public async Task<Calon> Create(PostCalon postCalon, Periode periode)
        {
            Calon calon = new Calon
            {
                Name = postCalon.Name,
                Description = postCalon.Description,
                NoUrut = postCalon.NoUrut,
                Photo = postCalon.Photo,
                Periode = periode
            };

            _context.Calons.Add(calon);
            await _context.SaveChangesAsync();

            return calon;
        }

        public async Task<List<Calon>> Get()
        {
            // get semua data di tabel calon
            List<Calon> calons = await _context.Calons.ToListAsync();
            return calons;
        }

        public async Task<Calon> Get(int id)
        {
            // get dengan id tertentu
            Calon? calon = await _context.Calons.Include(c => c.Periode)
                .Where(c => c.Id == id)
                .FirstAsync();

            if (calon == null)
                throw new InvariantError("Data Calon tidak ada");

            return calon;
        }

        public async Task<List<Calon>> GetByPeriode(Periode periode)
        {
            List<Calon> calons = await _context.Calons.Where(c => c.Periode == periode).ToListAsync();
            return calons;
        }

        public async Task<Calon> Update(int id, PostCalon postCalon)
        {
            Calon? calon = await _context.Calons.FindAsync(id);
            if (calon == null)
                throw new InvariantError("Data Calon tidak ada");

            calon.Description = postCalon.Description;
            calon.NoUrut = postCalon.NoUrut;
            calon.Photo = postCalon.Photo;
            calon.Name = postCalon.Name;

            await _context.SaveChangesAsync();
            return calon;
        }

        public async Task Delete(int id)
        {
            Calon? calon = await _context.Calons.FindAsync(id);
            if (calon == null)
                throw new InvariantError("Data Calon tidak ada");

            _context.Calons.Remove(calon);
            await _context.SaveChangesAsync();
        }
    }
}

using BackendPilketos.Models;
using Microsoft.EntityFrameworkCore;
using BackendPilketos.Exceptions;

namespace BackendPilketos.Services
{
    public class GroupService
    {
        private DataContext _context;
        public GroupService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<UserGroup>> Get()
        {
            var groups = await _context.UserGroups.ToListAsync();
            return groups;
        }


        public async Task<UserGroup> Get(int id)
        {
            var group = await _context.UserGroups.FindAsync(id);
            if (group == null)
            {
                throw new InvariantError("Group tidak ditemukan");
            }
            return group;
        }

        public async Task<UserGroup> Create(string name)
        {
            UserGroup userGroup = new UserGroup
            {
                Name = name,
                Description = name
            };

            _context.UserGroups.Add(userGroup);

            await _context.SaveChangesAsync();
            return userGroup;
        }

        public async Task<UserGroup> Update(int id, string name)
        {
            UserGroup userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup == null) throw new InvariantError("Data Group tidak ditemukan");

            userGroup.Name = name;
            await _context.SaveChangesAsync();
            return userGroup;
        }

        public async Task Delete(int id)
        {
            UserGroup userGroup = await _context.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                throw new InvariantError("Data Group tidak ditemukan");
            }
            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();
        }

    }

}

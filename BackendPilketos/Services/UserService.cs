using BackendPilketos.Models;
using BackendPilketos.Requests;
using BackendPilketos.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BackendPilketos.Services
{
    public class UserService
    {
        private DataContext _ctx { get; set; }
        public UserService(DataContext context)
        {
            _ctx = context;
        }
        public async Task<User> Create(PostUser userdata, UserGroup? group)
        {
            User user = new User
            {
                Email = userdata.Email,
                Name = userdata.Name,
                Role = userdata.Role,
                Group = group
            };

            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> Create(List<PostUser> userdata, UserGroup? group)
        {
            foreach (PostUser pu in userdata)
            {
                User nuser = new User
                {
                    Email = pu.Email,
                    Name = pu.Name,
                    Role = pu.Role,
                    Group = group
                };

                _ctx.Users.Add(nuser);
            }

            await _ctx.SaveChangesAsync();
            return await _ctx.Users.ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            User? user = await _ctx.Users.Include(u => u.Group).Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new InvariantError("Data user tidak ditemukan");
            return user;
        }
        public async Task<User> Get(string email)
        {
            User user = await _ctx.Users.Include(u => u.Group).Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user == null)
                throw new InvariantError("Data user tidak ditemukan");
            return user;
        }
        public async Task<List<User>> Get()
        {
            List<User> users = await _ctx.Users.ToListAsync();

            return users;
        }

        public async Task<List<User>> GetByGroup(UserGroup group)
        {
            List<User> users = await _ctx.Users.Where(u => u.Group == group).ToListAsync();
            return users;
        }


        public async Task<List<GroupCount>> CountMemberEachGroup()
        {
            var groups = await _ctx.UserAndGroups
                .GroupBy(u => u.GroupId)
                .Select(ug => new GroupCount { 
                    GroupId = ug.Key, 
                    GroupMemberCount = ug.Count(), 
                    GroupName = ug.Select(ugc => ugc.GroupName).FirstOrDefault()
                })
                .ToListAsync();

            return groups;
        }



        public async Task<User> Update(int Id, PostUser userData, UserGroup? group)
        {
            User? user = await _ctx.Users.FindAsync(Id); if (user == null)
                throw new InvariantError("Data user tidak ditemukan");
            user.Email = userData.Email;
            user.Name = userData.Name;
            user.Role = userData.Role;
            if (group != null)
            {
                user.Group = group;
            }

            await _ctx.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int id)
        {
            User? user = await _ctx.Users.FindAsync(id);
            if (user == null)
                throw new InvariantError("Data user tidak ditemukan");
            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();
        }

        public async Task CheckEmails (List<PostUser> postUsers)
        {
            var emailList = postUsers.Select(u => u.Email).ToList();
            int count = await _ctx.Users.Where(u => emailList.Contains(u.Email)).CountAsync();
            if (count > 0)
            {
                throw new InvariantError("Email Sudah pernah terdaftar");
            }

        }

    }
    public class GroupCount
    {
        public int GroupId { get; set; }
        public int GroupMemberCount { get; set; }
        public string GroupName { get; set; }
    }
}

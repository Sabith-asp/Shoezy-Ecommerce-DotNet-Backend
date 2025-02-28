using Shoezy.Data;
using Shoezy.Models;
using Microsoft.EntityFrameworkCore;
using Shoezy.DTOs;


namespace Shoezy.Repositories
{
    public class UserRepository
    {
        private readonly ShoezyDbContext context;
        public UserRepository(ShoezyDbContext _context) {
            context= _context;
        }

        public async Task<User> GetUser(int userid) {
            return await context.Users.FirstOrDefaultAsync(x=>x.Id==userid);
        }
        public async Task<List<User>> GetUsers()
        {
            return await context.Users.ToListAsync();

        }
        public async Task<User> GetUserById(int id) {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> BlockOrUnblockUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            user.IsBlocked = !user.IsBlocked;

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false; 
            }
        }


        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}

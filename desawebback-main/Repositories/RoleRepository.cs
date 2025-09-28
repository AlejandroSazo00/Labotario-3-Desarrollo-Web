
using HelloApi.Models;
using HelloApi.Models.DTOs;
using HelloApi.Repositories.Interfaces;
using MessageApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext _context = context;

        public Task<Person> AddRoleAsync(RoleDto person)
        {
            throw new NotImplementedException();
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}
using HelloApi.Models;
using HelloApi.Models.DTOs;

namespace HelloApi.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Busca el nombre del rol y retorna el primer registro si hay coincidencia
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Retorna la primer coincidencia</returns>
        Task<Role?> GetByNameAsync(string name);
        Task<Person> AddRoleAsync(RoleDto person);
    }
}
using HelloApi.Models;
using HelloApi.Models.DTOs;

namespace HelloApi.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> AddPersonAsync(PersonCreateDto person, int userId);
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        Task<Person?> GetPersonByIdAsync(int id);
        Task<Person?> UpdatePersonAsync(Person person);
        Task<bool> DeletePersonAsync(int id);

    }
}

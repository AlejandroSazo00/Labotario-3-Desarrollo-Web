using Microsoft.AspNetCore.Mvc;
using HelloApi.Models.DTOs;
using HelloApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HelloApi.Utils;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonController(IPersonService service, CryptoHelper cryptoHelper) : ControllerBase
    {
        private readonly IPersonService _service = service;
        private readonly CryptoHelper _cryptoHelper = cryptoHelper;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _service.GetAllPersonsAsync();
            return Ok(persons);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await _service.GetPersonByIdAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonCreateDto dto)
        {
            var strUserId = User.FindFirst("Id")?.Value;
            strUserId ??= "0";
            var userId = _cryptoHelper.Decrypt(strUserId);
            var created = await _service.CreatePersonAsync(dto, int.Parse(userId));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonUpdateDto dto)
        {
            var updated = await _service.UpdatePersonAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeletePersonAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

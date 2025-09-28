using HelloApi.Models.DTOs;

namespace HelloApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
        Task<string?> RegisterAsync(RegisterRequestDto request);
    }
}

using MinimalApi.Demo.Models.DTO;

namespace MinimalApi.Demo.Repository.IRepository
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<UserDto> Register(RegistrationRequestDto requestDto);
    }
}

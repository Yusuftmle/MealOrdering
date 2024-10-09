using MealOrdering.Shared.DTO;

namespace MealOrdering.Server.Services.Infratructure
{
    public interface IUserService
    {

        public Task<UserDTO> GetUserById(Guid Id);
       
        public Task<List<UserDTO>> GetUsers();

        public Task<UserDTO> CreateUser(UserDTO User);

        public Task<UserDTO> UpdateUser(UserDTO User);
        public Task<bool> DeleteUserById(Guid User);

        public Task<UserLoginResponseDTO> Login (string Email, string Password);
    }
}

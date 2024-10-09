using MealOrdering.Server.Services.Infratructure;
using MealOrdering.Server.Services.Services;
using MealOrdering.Shared.DTO;
using MealOrdering.Shared.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace MealOrdering.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
        }
       
        [HttpPost("Login")]
		[AllowAnonymous]
		public async Task <ServiceResponse<UserLoginResponseDTO>> Login (UserLoginRequestDTO userRequest)
        {
            return new ServiceResponse<UserLoginResponseDTO>
            {
                Value =await UserService.Login(userRequest.Email, userRequest.Password)
            };
        }

        [HttpGet("Users")]
        public async Task<ServiceResponse<List<UserDTO>>> GetUsers()
        {
            return new ServiceResponse<List<UserDTO>>()
            {
                Value = await UserService.GetUsers()
            };
        }

        [HttpPost("Create")]
        public async Task<ServiceResponse<UserDTO>> CreateUser([FromBody] UserDTO User)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await UserService.CreateUser(User)
            };
        }

        [HttpPost("Update")]
        public async Task <ServiceResponse<UserDTO>> UpdateUser([FromBody]UserDTO user)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await UserService.UpdateUser(user)
            };
            
        }

        [HttpGet("UserById/{Id}")]
        public async Task <ServiceResponse<UserDTO>> GetUserById([FromBody]Guid Id)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await UserService.GetUserById(Id)
            };
        }

    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MealOrdering.Server.Data.Context;
using MealOrdering.Server.Services.Infratructure;
using MealOrdering.Shared.DTO;
using MealOrdering.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MealOrdering.Server.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly MealOrderingDbContext context;
        private readonly IConfiguration configuration;
        public UserService(IMapper Mapper,MealOrderingDbContext Context, IConfiguration Configuration)
        {
            mapper = Mapper;
            context = Context;
            configuration = Configuration;

        }

       

        public async Task<UserDTO> CreateUser(UserDTO User)
        {
            var dbUser = await context.Users.Where(u => u.Id == User.Id).FirstOrDefaultAsync();

            if (dbUser != null)
            {
                throw new Exception("İlgili kayıt zaten mevcut");
            }

            User.Password=PasswordEncrypter.Encrypt(User.Password);
            dbUser = mapper.Map<Data.Models.Users>(User);

            await context.Users.AddAsync(dbUser);
            int result = await context.SaveChangesAsync();
			
			return mapper.Map<UserDTO>(User);
        }

        public async Task<bool> DeleteUserById(Guid Id)
        {
           var dbUser=await context.Users.Where(u=>u.Id==Id).FirstOrDefaultAsync();

            if (dbUser==null) 
            {
                throw new Exception("kullanıcı bulunamadı");
            }

            context.Remove(dbUser);
          int result= await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<UserDTO> GetUserById(Guid Id)
        {
            return await context.Users
                 .Where(u => u.Id == Id)
                 .ProjectTo<UserDTO>(mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync() ?? new UserDTO();
        }

        public async Task<List<UserDTO>> GetUsers()
        {
          return await context.Users
                .Where(i => i.IsActive)
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

       
        public async Task<UserDTO> UpdateUser(UserDTO User)
        {
            var dbUser = await context.Users.Where(u => u.Id == User.Id).FirstOrDefaultAsync();

            if (dbUser == null)
            {
                throw new Exception("İlgili kayıt Bulunamadı");
            }
            mapper.Map(User, dbUser);

            await context.Users.AddAsync(dbUser);
            int result = await context.SaveChangesAsync();

            return mapper.Map<UserDTO>(User);
        }

        public async Task<UserLoginResponseDTO> Login(string Email, string Password)
        {
            //veritabanı kullanıcı doğrulama işlemi yapıldı
            var encyrptedPassword = PasswordEncrypter.Encrypt(Password);

            var dbUser = await context.Users.FirstOrDefaultAsync(i => i.EmailAdress == Email && i.Password == Password);

            if (dbUser == null)
                throw new Exception("kullanıcı bulunamadı veya bilgiler yalnış");

            if (!dbUser.IsActive)
                throw new Exception("kullanıcı pasif durumdadır");

            UserLoginResponseDTO result= new UserLoginResponseDTO();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(int.Parse(configuration["JwtExpiryInDays"].ToString()));
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,Email)
            };

            var token = new JwtSecurityToken(configuration["JwtIssuer"], configuration["JwtAudience"], claims, null, expiry, creds);
            result.ApiToken = new JwtSecurityTokenHandler().WriteToken(token);
            result.User = mapper.Map<UserDTO>(dbUser);
            return result;
        }
    }
}

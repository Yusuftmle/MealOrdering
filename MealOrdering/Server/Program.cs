using Microsoft.AspNetCore.ResponseCompression;
using Blazored.Modal;
using Blazored.Modal.Services;
using MealOrdering.Client.Utils;
using MealOrdering.Server.Services.Extensions;
using MealOrdering.Server.Services.Infratructure;
using MealOrdering.Server.Services.Services;
using MealOrdering.Server.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace MealOrdering
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddBlazoredModal();
            builder.Services.AddScoped<IModalService, ModalService>(); // Blazored Modal kullanýyorsanýz bu gerekli olabilir
            builder.Services.AddScoped<ModalManager>(); // ModalManager servisini ekleyin
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddDbContext<MealOrderingDbContext>(
                config => config.UseSqlServer(configuration["ConnectionStrings:String"])
                );
            builder.Services.AddControllersWithViews();
            builder.Services.ConfigureMapping();
            builder.Services.AddRazorPages();
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtIssuer"],
                    ValidAudience = configuration["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]))

                };
            });
			builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            // HttpClient'ý kaydet
            builder.Services.AddHttpClient();
			builder.Services.AddBlazoredLocalStorage();
			builder.Services.AddAuthorizationCore();
			var app = builder.Build();
                
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
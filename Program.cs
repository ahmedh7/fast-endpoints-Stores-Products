using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreProdFastEndpoints.Context;
using StoreProdFastEndpoints.Models;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var bld = WebApplication.CreateBuilder();

            #region DB
            var connectionString = "data source=DESKTOP-CKPTG0Q\\SQLSERVER;initial catalog=master;Database=StoreDB5;trusted_connection=true;encrypt=false;";
            bld.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(connectionString));
            #endregion

            #region Identity
            bld.Services.AddScoped<AdminService>();
            bld.Services.AddScoped<UserManager<Admin>>();
            bld.Services.AddIdentity<Admin, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<StoreContext>();
            #endregion


            #region CORS
            bld.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
            #endregion

            #region Authorization
            bld.Services
                .AddFastEndpoints()
                .AddJWTBearerAuth("TokenSigningKey@mysecretsigningkey");
            bld.Services.AddAuthentication(o => 
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            bld.Services.AddAuthorization(options =>
             {
                 options.AddPolicy("AdminsOnly", x => x.RequireRole("Admin"));
             });

            #endregion


            #region Services
            bld.Services.AddScoped<StoreService>();
            bld.Services.AddScoped<ProductService>();
            #endregion

            var app = bld.Build();
            app.UseAuthentication()
               .UseAuthorization()
               .UseFastEndpoints();
            app.Run();
        }
    }
}
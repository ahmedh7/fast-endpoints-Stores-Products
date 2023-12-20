using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using StoreProdFastEndpoints.DTOs;
using StoreProdFastEndpoints.Models;
using System.Security.Claims;

namespace StoreProdFastEndpoints.Services
{
    public class AdminService
    {
        private readonly UserManager<Admin> _userManager;

        public AdminService(UserManager<Admin> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> CredentialsAreValid(AdminDTO cred)
        {
            Admin? admin = await _userManager.FindByEmailAsync(cred.Email);
            if (admin is null)
            {
                return string.Empty;
            }

            bool isAuthenticated = await _userManager.CheckPasswordAsync(admin, cred.Password);

            if (!isAuthenticated)
            {
                return string.Empty;
            }
            var jwtToken = JWTBearer.CreateToken(
                    signingKey: "TokenSigningKey@mysecretsigningkey",
                    expireAt: DateTime.UtcNow.AddDays(1),
                    privileges: u =>
                    {
                        u.Roles.Add("Admin");
                        u.Claims.Add(new("Email", cred.Email));

                    });
            return jwtToken;
        }

        public async Task<RegistrationStatusDTO> Register(AdminDTO adminDTO)
        {
            try
            {
                var newAdmin = new Admin()
                {
                    UserName = adminDTO.Username,
                    Email = adminDTO.Email,
                };

                var result = await _userManager.CreateAsync(newAdmin, adminDTO.Password);
                
                if (!result.Succeeded)
                {
                    return new()
                    {
                        RegistartionStatus = false,
                        RegistrationString = result.ToString()!
                    };
                }

                var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.NameIdentifier, newAdmin.Id),
                                     new Claim(ClaimTypes.Role, "Admin"),
                                 };

                await _userManager.AddClaimsAsync(newAdmin, claims);

                return new()
                {
                    RegistartionStatus = true,
                    RegistrationString = "Registration Successful!"
                };

            }
            catch (Exception ex)
            {
                return new()
                {
                    RegistrationString = ex.ToString()
                };
            }

        }
    }
}

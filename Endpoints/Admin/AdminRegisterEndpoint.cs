using StoreProdFastEndpoints.Services;
using FastEndpoints;
using StoreProdFastEndpoints.DTOs;

namespace StoreProdFastEndpoints.Endpoints.Admin
{
    public class AdminRegisterEndpoint : Endpoint<AdminDTO>
    {
        private readonly AdminService _authService;
        public AdminRegisterEndpoint(AdminService autServiceh)
        {
            _authService = autServiceh;
        }
        public override void Configure()
        {
            Post("/api/register");
            AllowAnonymous();
        }
        public override async Task HandleAsync(AdminDTO req, CancellationToken ct)
        {
            var regStatus = await _authService.Register(req);
            if (regStatus.RegistartionStatus)
            {
                await SendAsync(new
                {
                    regStatus.RegistartionStatus,
                    regStatus.RegistrationString
                }
                , cancellation: ct);
            }
            else
            {
                await SendAsync(new
                {
                    regStatus.RegistartionStatus,
                    regStatus.RegistrationString
                }
                , cancellation: ct);
            }
        }
    }
}

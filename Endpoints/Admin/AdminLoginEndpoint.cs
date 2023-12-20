using FastEndpoints;
using StoreProdFastEndpoints.DTOs;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints.Endpoints.Admin
{
    public class AdminLoginEdnpoint : Endpoint<AdminDTO>
    {
        private readonly AdminService _service;
        public AdminLoginEdnpoint(AdminService service) : base()
        {
            _service = service;
        }
        public override void Configure()
        {
            Post("/api/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AdminDTO cred, CancellationToken ct)
        {
            var jwtToken = await _service.CredentialsAreValid(cred);
            if (jwtToken != string.Empty)
            {
                await SendAsync(new
                {
                    cred.Email,
                    Token = jwtToken
                }, cancellation: ct);
            }
            else
            {
                ThrowError("The supplied credentials are invalid!");
            }
        }
    }
}

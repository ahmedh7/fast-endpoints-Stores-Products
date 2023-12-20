using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using StoreProdFastEndpoints.DTOs;
using StoreProdFastEndpoints.Services;


namespace StoreProdFastEndpoints.Endpoints.Store
{
    
    public class GetStoresPaginatedEndpoint : Endpoint<PaginationDTO>
    {
        private readonly StoreService _service;
        public GetStoresPaginatedEndpoint(StoreService service)
        {
            _service = service;
        }
        public override void Configure()
        {
            Get("/api/store/get");
            Policies("AdminsOnly");
            //AllowAnonymous();
        }

        

        public override async Task HandleAsync(PaginationDTO dto, CancellationToken ct)
        {
            try
            {
                var stores = _service.GetStoresPaginated(dto);
                if (stores.IsNullOrEmpty())
                {
                    await SendAsync(new
                    {
                        products = "No products to show!"
                    }, cancellation: ct);
                }
                await SendAsync(new
                {
                    stores,
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    Message = "Fetching Failed",
                    Exception = ex.ToString(),
                }, cancellation: ct);
            }
        }
    }
}

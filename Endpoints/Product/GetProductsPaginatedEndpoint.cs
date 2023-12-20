using FastEndpoints;
using Microsoft.IdentityModel.Tokens;
using StoreProdFastEndpoints.DTOs;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints.Endpoints.Product
{
    public class GetProductsPaginatedEndpoint : Endpoint<PaginationDTO>
    {
        private readonly ProductService _service;
        public GetProductsPaginatedEndpoint(ProductService service)
        {
            _service = service;
        }
        public override void Configure()
        {
            Get("/api/product/get");
            //Policies("AdminsOnly");
            AllowAnonymous();
        }



        public override async Task HandleAsync(PaginationDTO dto, CancellationToken ct)
        {
            try
            {
                var prods = _service.GetProductsPaginated(dto);
                if (prods.IsNullOrEmpty())
                {
                    await SendAsync(new
                    {
                        products = "No products to show!"
                    }, cancellation: ct);
                }
                await SendAsync(new
                {
                    prods,
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

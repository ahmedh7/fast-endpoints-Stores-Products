using StoreProdFastEndpoints.DTOs;
using FastEndpoints;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints.Endpoints.Product
{
    public class CreateProductEndpoint : Endpoint<ProductDTO>
    {
        private readonly ProductService _service;
        public CreateProductEndpoint(ProductService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Post("/api/product/create");
            //Policies("AdminOnly");
            AllowAnonymous();
        }
        public override async Task HandleAsync(ProductDTO prodDTO, CancellationToken ct)
        {
            try
            {
                var createdProd = await _service.CreateProduct(prodDTO, ct);

                await SendAsync(new
                {
                    createdProd,
                    CreationStatus = "Created Successfully",
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    prodDTO,
                    CreationStatus = "Creation Failed",
                    Exception = ex.ToString()
                }, cancellation: ct);
            }

        }
    }
}

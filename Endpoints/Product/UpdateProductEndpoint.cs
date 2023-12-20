using FastEndpoints;
using StoreProdFastEndpoints.DTOs;
using StoreProdFastEndpoints.Services;
using System.Runtime.ExceptionServices;

namespace StoreProdFastEndpoints.Endpoints.Product
{
    public class UpdateProductEndpoint : Endpoint<ProductDTO>
    {
        private readonly ProductService _service;
        public UpdateProductEndpoint(ProductService service)
        {
            _service = service;
        }


        public override void Configure()
        {
            Patch("/api/product/update/{id}");
            //Policies("AdminsOnly");
            AllowAnonymous();
        }



        public override async Task HandleAsync(ProductDTO prodDTO, CancellationToken ct)
        {
            int id = Route<int>("id");
            try
            {
                var invalidStoreIDs = _service.UpdateProduct(prodDTO, id, ct);
                await SendAsync(new
                {
                    prodDTO,
                    UpdateStaus = "Updated Successfully",
                    InvalidProductsIDs = invalidStoreIDs
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    prodDTO,
                    UpdateStaus = "Update Failed",
                    Exception = ex.ToString()
                }, cancellation: ct);
            }

        }
    }
}

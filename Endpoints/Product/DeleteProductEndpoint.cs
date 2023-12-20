using StoreProdFastEndpoints.Services;
using FastEndpoints;

namespace StoreProdFastEndpoints.Endpoints.Product
{
    public class DeleteProductEndpoint : EndpointWithoutRequest
    {
        private readonly ProductService _service;
        public DeleteProductEndpoint(ProductService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Delete("/api/product/delete/{id}");
            //Policies("AdminsOnly");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                int id = Route<int>("id");
                bool deletionStatus = await _service.DeleteProduct(id, ct);
                await SendAsync(new
                {
                    DeletionStatus = "Deleted Successfully"
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    DeletionStatus = "Deletion Failed",
                    Exception = ex.ToString()
                }, cancellation: ct);
            }

        }
    }
}

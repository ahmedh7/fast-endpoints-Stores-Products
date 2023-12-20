using FastEndpoints;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints.Endpoints.Store
{
    public class DeleteStoreEndpoint : EndpointWithoutRequest
    {
        private readonly StoreService _service;
        public DeleteStoreEndpoint(StoreService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Delete("/api/store/delete/{id}");
            //Policies("AdminsOnly");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                int id = Route<int>("id");
                bool deletionStatus = await _service.DeleteStore(id, ct);
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

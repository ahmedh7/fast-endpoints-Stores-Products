using StoreProdFastEndpoints.DTOs;
using FastEndpoints;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints.Endpoints.Store
{

    public class CreateStoreEndpoint : Endpoint<StoreDTO>
    {
        private readonly StoreService _service;
        public CreateStoreEndpoint(StoreService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Post("/api/store/create");
            //Policies("AdminOnly");
            AllowAnonymous();
        }
        public override async Task HandleAsync(StoreDTO storeDTO , CancellationToken ct)
        {
            try
            {
                var createdStore = await _service.CreateStore(storeDTO, ct);

                await SendAsync(new
                {
                    createdStore,
                    CreationStatus = "Created Successfully",
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    storeDTO,
                    CreationStatus = "Creation Failed",
                    Exception = ex.ToString()
                }, cancellation: ct);
            }

        }
    }
}

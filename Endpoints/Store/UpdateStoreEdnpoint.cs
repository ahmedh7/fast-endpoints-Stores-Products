using StoreProdFastEndpoints.DTOs;
using FastEndpoints;
using System.Runtime.ExceptionServices;
using StoreProdFastEndpoints.Services;

namespace StoreProdFastEndpoints.Endpoints.Store
{
    public class UpdateStoreEdnpoint : Endpoint<StoreDTO>
    {
        private readonly StoreService _service;
        public UpdateStoreEdnpoint(StoreService service)
        {
            _service = service;
        }
        

        public override void Configure()
        {
            Patch("/api/store/update/{id}");
            //Policies("AdminsOnly");
            AllowAnonymous();
        }

        

        public override async Task HandleAsync(StoreDTO storeDTO, CancellationToken ct)
        {
            int id = Route<int>("id");
            try
            {
                var invalidProductIDs = _service.UpdateStore(storeDTO, id, ct);
                await SendAsync(new
                {
                    storeDTO,
                    UpdateStaus = "Updated Successfully",
                    InvalidProductsIDs = invalidProductIDs
                }, cancellation: ct);
            }
            catch (Exception ex)
            {
                await SendAsync(new
                {
                    storeDTO,
                    UpdateStaus = "Update Failed",
                    Exception = ex.ToString()
                }, cancellation: ct);
            }

        }
    }
}

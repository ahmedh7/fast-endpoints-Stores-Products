using M = StoreProdFastEndpoints.Models;

namespace StoreProdFastEndpoints.DTOs
{
    public class CreatedStoreDTO
    {
        public M.Store? Store { get; set; }
        public List<int> InvalidProductIDs { get; set; } = new List<int>();
    }
}

using M = StoreProdFastEndpoints.Models;


namespace StoreProdFastEndpoints.DTOs
{
    public class CreatedProductDTO
    {
        public M.Product? Product { get; set; }
        public List<int> InvalidStoreIDs { get; set; } = new List<int>();
    }
}

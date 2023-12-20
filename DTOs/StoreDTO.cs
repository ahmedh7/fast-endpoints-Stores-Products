namespace StoreProdFastEndpoints.DTOs
{
    public class StoreDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<int> ProductIDs { get; set; } = new List<int>();
    }
}

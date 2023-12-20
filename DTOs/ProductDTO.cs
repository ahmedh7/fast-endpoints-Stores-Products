using System.ComponentModel.DataAnnotations;

namespace StoreProdFastEndpoints.DTOs
{
    public class ProductDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<int> StoreIDs { get; set; } = new();

    }
}

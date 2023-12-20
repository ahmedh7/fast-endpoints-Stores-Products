namespace StoreProdFastEndpoints.DTOs
{
    public class PaginationDTO
    {
        public bool GetAll { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 2;
        public string Filter { get; set; } = string.Empty;
        public bool Ascending { get; set; } = true;
    }
}

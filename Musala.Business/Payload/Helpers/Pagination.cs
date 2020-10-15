namespace Musala.Business.Payload.Helpers
{
    public class Pagination
    {
        public int Skip { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
    }

    public class PaginationLinks
    {
        public string Next { get; set; }
        public string Previous { get; set; }
    }
}

namespace Customer_And_Orders.DAL.Data.Entities
{
    public class QueryParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool? SortByDate { get; set; }
        public string? Status { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
    }
}

namespace Customer_And_Orders.DAL.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}

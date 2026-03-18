namespace UserAPIJWT.Models
{
    public class MergeCartRequest
    {
        public int UserId { get; set; }

        public List<CartItem>? Items { get; set; }
    }
    public class CartItem
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}

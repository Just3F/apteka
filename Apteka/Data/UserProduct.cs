namespace Apteka.Data
{
    public class UserProduct
    {
        public int Count { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

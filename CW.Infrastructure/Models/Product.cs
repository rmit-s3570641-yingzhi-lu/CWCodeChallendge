namespace CW.Infrastructure.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public ProductTypes Type { get; set; }
        public bool IsActive { get; set; }
    }
}

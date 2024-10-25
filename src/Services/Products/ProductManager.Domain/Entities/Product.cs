namespace ProductManager.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string? Description { get; set; } 

        public decimal Price { get; set; } 

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdate { get; set; } 

        public bool Deleted { get; set; } = false;
    }

}

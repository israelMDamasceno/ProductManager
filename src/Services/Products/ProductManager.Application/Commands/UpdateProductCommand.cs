using MediatR;

namespace ProductManager.Application.Commands
{
    public class UpdateProductCommand : IRequest<string>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } 
        public decimal Price { get; set; }
        public DateTime DateUpdate {  get; set; }
    }
}

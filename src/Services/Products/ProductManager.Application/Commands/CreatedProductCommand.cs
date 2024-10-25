using MediatR;

namespace ProductManager.Application.Commands
{
    public class CreatedProductCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}

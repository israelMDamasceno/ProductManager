using MediatR;

namespace ProductManager.Application.Commands
{
    public class DeletedProductCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}

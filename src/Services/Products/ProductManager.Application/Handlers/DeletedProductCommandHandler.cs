using MediatR;
using ProductManager.Application.Commands;
using ProductManager.Application.Notifications;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

public class DeletedProductCommandHandler : IRequestHandler<DeletedProductCommand, string>
{
    private readonly IMediator _mediator;
    private readonly IRepository<Product> _repository;

    public DeletedProductCommandHandler(IMediator mediator, IRepository<Product> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<string> Handle(DeletedProductCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var product = await _repository.Get(request.Id);
            if (product == null)
            {
                return "Product not found";
            }

            product.Deleted = true;
            await _repository.Edit(product);


            await _mediator.Publish(new DeletedProductNotification
            {
                Id = request.Id,
                IsConfirmed = true
            });

            return "Product deleted successfully";
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new DeletedProductNotification
            {
                Id = request.Id,
                IsConfirmed = false
            });


            await _mediator.Publish(new ErrorNotification
            {
                Exception = ex.Message,
                StackTrace = ex.StackTrace
            });

            return "An error occurred while deleting the product";
        }
    }
}

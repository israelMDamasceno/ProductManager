using MediatR;
using ProductManager.Application.Commands;
using ProductManager.Application.Notifications;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

public class UpdatedProductCommandHandler : IRequestHandler<UpdateProductCommand, string>
{
    private readonly IMediator _mediator;
    private readonly IRepository<Product> _repository;

    public UpdatedProductCommandHandler(IMediator mediator, IRepository<Product> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DateUpdate = DateTime.Now 
        };

        try
        {
            await _repository.Edit(product); 
            await _mediator.Publish(new UpdatedProductNotification
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                IsConfirmed = true
            });

            return "Product updated successfully";
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new UpdatedProductNotification
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                IsConfirmed = false
            });

            await _mediator.Publish(new ErrorNotification
            {
                Exception = ex.Message,
                StackTrace = ex.StackTrace
            });

            return "An error occurred while updating the product";
        }
    }
}

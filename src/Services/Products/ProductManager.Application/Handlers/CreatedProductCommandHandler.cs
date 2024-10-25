using MediatR;
using ProductManager.Application.Commands;
using ProductManager.Application.Notifications;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

public class CreatedProductCommandHandler : IRequestHandler<CreatedProductCommand, string>
{
    private readonly IMediator _mediator;
    private readonly IRepository<Product> _repository;

    public CreatedProductCommandHandler(IMediator mediator, IRepository<Product> repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public async Task<string> Handle(CreatedProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DateCreated = DateTime.Now,
            DateUpdate = DateTime.Now,
            Deleted = false
        };

        try
        {
            product = await _repository.Add(product);

            await _mediator.Publish(new CreatedProductNotification
            {
                Name = product.Name,
                Price = product.Price
            });

            return "Product created successfully";
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ErrorNotification
            {
                Exception = ex.Message,
                StackTrace = ex.StackTrace
            });

            return "An error occurred while creating the product";
        }
    }
}

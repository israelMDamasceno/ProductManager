using MediatR;

public class CreatedProductNotification : INotification
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime DateCreated { get; set; }
}

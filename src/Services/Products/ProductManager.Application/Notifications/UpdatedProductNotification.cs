using MediatR;

public class UpdatedProductNotification : INotification
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateUpdate { get; set; }

    public bool IsConfirmed { get; set; }
}
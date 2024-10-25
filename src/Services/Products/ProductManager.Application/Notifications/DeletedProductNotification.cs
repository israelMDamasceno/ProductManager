using MediatR;

public class DeletedProductNotification : INotification
{
    public int Id { get; set; }
    public bool IsConfirmed { get; set; }
}

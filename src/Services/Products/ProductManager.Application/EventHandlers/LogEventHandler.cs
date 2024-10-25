using MediatR;
using ProductManager.Application.Notifications;

namespace ProductManager.Application.EventHandlers
{
    public class LogEventHandler :
                                INotificationHandler<CreatedProductNotification>,
                                INotificationHandler<UpdatedProductNotification>,
                                INotificationHandler<DeletedProductNotification>,
                                INotificationHandler<ErrorNotification>
    {
        public Task Handle(CreatedProductNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"CREATION: '{notification.Name} - {notification.Description} -  {notification.Price}'");
            });
        }

        public Task Handle(UpdatedProductNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"UPDATE: '{notification.Id} - {notification.Name} - {notification.Price} - {notification.IsConfirmed}'");
            });
        }

        public Task Handle(DeletedProductNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"DELETION: '{notification.Id} - {notification.IsConfirmed}'");
            });
        }

        public Task Handle(ErrorNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"ERROR: '{notification.Exception} \n {notification.StackTrace}'");
            });
        }
    }
}

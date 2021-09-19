using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Bootstrapper.Sample01
{

    public class Ping : INotification
    {
        public Ping(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class Pong : INotificationHandler<Ping>
    {
        Task INotificationHandler<Ping>.Handle(Ping notification, CancellationToken cancellationToken)
        {
           Console.WriteLine(notification.Message);
           return Task.CompletedTask;
        }
    }
}

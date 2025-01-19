using Microsoft.AspNetCore.SignalR;
namespace Presentations.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            string name = connection.User.Identity.Name;
            Console.WriteLine(name);
            return name;
        }
    }
}

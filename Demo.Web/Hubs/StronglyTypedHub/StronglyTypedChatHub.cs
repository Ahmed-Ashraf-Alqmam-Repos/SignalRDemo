using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Demo.Web.Hubs.StronglyTypedHub
{
    public class StronglyTypedChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }

        public async Task SendMessageToCaller(string user, string message)
        {
            await Clients.Caller.ReceiveMessage(user, message);
        }

        public async Task SendMessageToGroup(string user, string message)
        {
            await Clients.Group("SignalR Group").ReceiveMessage(user, message);
        }
    }
}

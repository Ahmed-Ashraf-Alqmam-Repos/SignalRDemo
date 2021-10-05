using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Demo.Web.Hubs
{
    /// <summary>
    /// * Don't store state in a property on the hub class. 
    ///   Every hub method call is executed on a new hub instance.
    /// * Use await when calling asynchronous methods that depend on the hub staying alive.
    ///   a method such as Clients.All.SendAsync(...) can fail if it's called without await 
    ///   and the hub method completes before SendAsync finishes.
    ///   
    /// * Security warning: Exposing ConnectionId can lead to malicious impersonation 
    ///   if the SignalR server or client version is ASP.NET Core 2.2 or earlier.
    /// </summary>

    public class ChatHub : Hub
    {
        #region HubMethods
        public async Task SendMessage(string user, string message)
        {
            // ReceiveMessage is a callback method defined in the client side
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToCaller(string user, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string user, string message)
        {
            await Clients.Group("SignalR Group").SendAsync("ReceiveMessage", user, message);
        }
        #endregion

        #region HubMethodName
        // The client Must use this name instead of DirectMessage
        [HubMethodName("SendMessageToUser")]
        public async Task DirectMessage(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", userId, message);
        }
        #endregion

        #region ThrowHubException
        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }
        #endregion

        #region HubCallbackMethods
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Group");
            await base.OnConnectedAsync();
        }


        // - by calling connection.stop(), the exception parameter will be null.
        // - if the client is disconnected due to an error (such as a network failure),
        //   the exception parameter will contain an exception describing the failure.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Group");
            await base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}

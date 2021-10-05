using System.Threading.Tasks;

namespace Demo.Web.Hubs.StronglyTypedHub
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}

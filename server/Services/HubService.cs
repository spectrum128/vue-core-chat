using Microsoft.AspNetCore.SignalR;
using VueChat.Hubs;

namespace VueChat.Services
{

    public interface IHubService
    {
        
        void SendContactRequest(string toUserId, string fromName);
        void AcceptContactRequest(string accepterid, string accepterName, string requester);
        
    }


    public class HubService : IHubService
    {

        private IHubContext<ChatHub> _hubcontext;
        public HubService(IHubContext<ChatHub> hubcontext)
        {
            _hubcontext = hubcontext;
        }


        public void SendContactRequest(string toUserId, string fromName)
        {
            _hubcontext.Clients.User(toUserId).SendAsync("ContactRequest", fromName);
        }

        public void AcceptContactRequest(string accepterid, string accepterName, string requester)
        {
            _hubcontext.Clients.User(requester).SendAsync("ContactRequestAccepted", accepterName);
        }
    }
}
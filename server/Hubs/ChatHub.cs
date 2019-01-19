using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VueChat.Dtos;
using VueChat.Services;

namespace VueChat.Hubs
{

    [Authorize]
    public class ChatHub : Hub
    {
        private IUserService _userService;
        private IContactService _contactService;

        public ChatHub(IUserService userService, IContactService contactService) : base()
        {
            _userService = userService;
            _contactService = contactService;
        }

        public override async Task OnConnectedAsync()
        {
            var userid = Context.User.Identity.Name;
            _userService.SetOnlineState(userid, true);

            await InformContactsOfStatus(true);
            await base.OnConnectedAsync();
            
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userid = Context.User.Identity.Name;
            _userService.SetOnlineState(userid, false);
            await InformContactsOfStatus(false);
            await base.OnDisconnectedAsync(exception);
        }

        private Entities.User GetUser(string userid)
        {
            var id = Convert.ToInt32(userid);
            return _userService.GetById(id);
        }

        private Entities.User GetCurrentUser()
        {
            var usrId = Context.User.Identity.Name;
            return GetUser(usrId);
        }

        public async Task InformContactsOfStatus(bool online)
        {
            
            var usr = GetCurrentUser();
            var contacts = _contactService.GetMyContacts(usr);

            List<string> userids = new List<string>();

            foreach (var co in contacts)
            {
                userids.Add(co.Id.ToString());   
            }

            Console.WriteLine("User " + usr.FullName + (online ? " connected" : " disconnected"));
            await Clients.Users(userids).SendAsync("ContactStatusUpdate", usr.Id.ToString(), online);
        }
        public async Task Send(string message)
        {
            var usr = GetCurrentUser();

            await Clients.All.SendAsync("Send", usr.FirstName, message);
        }

        public async Task SendToUser(ContactMessageDto message)
        {
            var usr = GetCurrentUser();

            message.sentOn = DateTime.Now;
            var x = _contactService.AddMessageToConversation(message);
            message.Id = x.Id;
            
            await Clients.User(message.toId).SendAsync("SendToUser", message);
        }

        public async Task ContactRequest(string userid)
        {
            var usr = GetUser(userid);
            var currentUser = GetCurrentUser();
            
            Console.WriteLine("Sending contact request to user id " + userid + " from " + Context.UserIdentifier);
            await Clients.User(userid).SendAsync("ContactRequest", currentUser.FirstName + " " + currentUser.LastName);
        }

        
    }
}
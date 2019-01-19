
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using VueChat.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using VueChat.Services;
using VueChat.Dtos;
using VueChat.Entities;
using Microsoft.AspNetCore.SignalR;
using VueChat.Hubs;

namespace VueChat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactController : BaseController
    {

        private IContactService _contactService;
        private IMapper _mapper;
        private IHubService _hubservice;
        private readonly AppSettings _appSettings;

        public ContactController(
            IUserService userService,
            IContactService contactService,
            IMapper mapper,
            IHubService hubservice,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _contactService = contactService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _hubservice = hubservice;
        }


        [HttpGet("getguid")]
        public IActionResult GetGuid()
        {
            var gu = Guid.NewGuid();

            return Ok(gu);
        }

        [HttpGet("search/{name}")]
        public IActionResult Search(string name)
        {
            var users = _userService.SearchByName(name);

            var contactDtos = _mapper.Map<IList<ContactDto>>(users);
            return Ok(contactDtos);

        }

        [HttpPost("createconversation")]
        public IActionResult CreateConversation([FromBody]ConversationDto conv)
        {
            _contactService.CreateConversation(conv);

            return Ok(conv);
        }

        [HttpPost("getconversation")]
        public IActionResult GetConversation([FromBody] string conversationId)
        {
            var conv = _contactService.GetConversation(conversationId);

            var user = GetCurrentUser();

            if(conv.partyId1 == user.Id || conv.partyId2 == user.Id){
                return Ok(conv);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost("getconversationforcontact")]
        public IActionResult GetConversationForContact([FromBody]int contactId)
        {
            var user = GetCurrentUser();

            var dto = _contactService.GetConversationForContact(user.Id, contactId);

            if(dto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dto);
            }
        }

        [HttpPost("acceptrequest")]
        public IActionResult AcceptContactRequest([FromBody]ReceivedContactRequestDto req)
        {
            var user = GetCurrentUser();

            if(user.Id == req.RequesteeId)
            {
                _contactService.AcceptContactRequest(req);

                var ex = _contactService.GetConversationForContact(user.Id, req.RequesterId);
                if(ex == null)
                {
                    var conv = _contactService.CreateConversation(req.RequesterId, req.RequesteeId);
                }

                _hubservice.AcceptContactRequest(user.Id.ToString(), user.FullName, req.RequesterId.ToString());
            }

            return Ok(req);
        }

        [HttpPost("sendrequest")]
        public IActionResult SendContactRequest([FromBody]int id)
        {
            Console.WriteLine("Got contact request for id " + id);
            var currentUserId = Convert.ToInt32(HttpContext.User.Identity.Name);

            var request = new ContactRequest { RequesterId = currentUserId, RequesteeId = id };
            var created = _contactService.Create(request);
            
            if(created)
            {
                var user = GetCurrentUser();

                _hubservice.SendContactRequest(id.ToString(), user.FullName);

                return Ok(request);
            }
            else{
                return Ok();
            }
            
        }

        [HttpGet("receivedrequests")]
        public IActionResult GetReceivedRequests()
        {
            var user = GetCurrentUser();

            var reqs = _contactService.GetReceivedRequests(user);

            return Ok(reqs);
        }

        [HttpGet("mycontacts")]
        public IActionResult GetMyContacts()
        {
            var user = GetCurrentUser();

            var reqs = _contactService.GetMyContacts(user);

            return Ok(reqs);
        }
    }
}
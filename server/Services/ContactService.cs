using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VueChat.Dtos;
using VueChat.Entities;
using VueChat.Helpers;

namespace VueChat.Services
{
    public interface IContactService
    {
        
        bool Create(ContactRequest request);
        void Delete(int id);
        IEnumerable<ReceivedContactRequestDto> GetReceivedRequests(User currentUser);
        IEnumerable<ContactDto> GetMyContacts(User currentUser);
        ReceivedContactRequestDto AcceptContactRequest(ReceivedContactRequestDto req);
        ConversationDto CreateConversation(ConversationDto conv);
        Conversation CreateConversation(int partyId1, int partyId2);
        ConversationDto GetConversation(string conversationId);
        ConversationDto GetConversationForContact(int currentUserId, int conactId);
        ContactMessageDto AddMessageToConversation(ContactMessageDto message);
        IEnumerable<ContactMessageDto> GetMessagesForConversation(string conversationId);
        IEnumerable<ContactDto> GetContactsForUserId(string userid);
        
    }

    public class ContactService : IContactService
    {
        private DataContext _context;
        private IMapper _mapper;

        public ContactService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

       
        
        public bool Create(ContactRequest request)
        {
            var created = false;

            var req = _context.ContactRequests.Where(x => x.RequesterId == request.RequesterId && x.RequesteeId == request.RequesteeId).FirstOrDefault();

            // request doesn't already exist so create it
            if(req == null)
            {
                _context.ContactRequests.Add(request);
                _context.SaveChanges();
                created = true;
            }
            
            return created;
        }

        public Conversation CreateConversation(int partyId1, int partyId2)
        {
            Conversation conv = new Conversation();
            conv.Id = Guid.NewGuid();
            conv.PartyId1 = partyId1;
            conv.PartyId2 = partyId2;

            _context.Add(conv);
            _context.SaveChanges();

            return conv;
        }
        public ConversationDto CreateConversation(ConversationDto conv)
        {
            var id = Guid.Parse(conv.Id);
            var cv = new Conversation();
            cv.Id = id;
            cv.PartyId1 = Convert.ToInt32(conv.partyId1);
            cv.PartyId2 = Convert.ToInt32(conv.partyId2);
            
            _context.Conversations.Add(cv);
            _context.SaveChanges();

            return conv;
        }

        public ConversationDto GetConversation(string conversationId)
        {
            var id = Guid.Parse(conversationId);
            var cv = _context.Conversations.Where(x => x.Id == id).FirstOrDefault();

            ConversationDto conversation = new ConversationDto();
            conversation.Id = cv.Id.ToString();
            conversation.partyId1 = cv.PartyId1;
            conversation.partyId2 = cv.PartyId2;
            conversation.messages = GetMessagesForConversation(conversationId);

            return conversation;
        }

        public IEnumerable<ContactMessageDto> GetMessagesForConversation(string conversationId)
        {
            var convId = Guid.Parse(conversationId);
            var ms = _context.Messages.Where(x => x.ConversationId == convId).ToList();

            var messages = new List<ContactMessageDto>();

            foreach (var m in ms)
            {
                ContactMessageDto cm = new ContactMessageDto();
                cm.conversationId = conversationId;
                cm.fromId = m.FromId.ToString();
                cm.fromName = m.FromName;
                cm.Id = m.Id;
                cm.message = m.MessageText;
                cm.sentOn = m.SentOn;
                cm.toId = m.ToId.ToString();
                cm.toName = m.ToName;
                
                messages.Add(cm);
            }

            return messages;
        }

        public ContactMessageDto AddMessageToConversation(ContactMessageDto message)
        {
            var mess = new Message();
            mess.ConversationId = Guid.Parse(message.conversationId);
            mess.FromId = Convert.ToInt32(message.fromId);
            mess.ToId = Convert.ToInt32(message.toId);
            mess.FromName = message.fromName;
            mess.MessageText = message.message;
            mess.SentOn = DateTime.Now;
            mess.ToName = message.toName;
            
            var x = _context.Messages.Add(mess);
            _context.SaveChanges();

            message.Id = x.Entity.Id;

            return message;
        }

        public ConversationDto GetConversationForContact(int currentUserId, int contactId)
        {
            var cv = _context.Conversations.Where(x => 
                (x.PartyId1 == contactId && x.PartyId2 == currentUserId) ||
                (x.PartyId1 == currentUserId && x.PartyId2 == contactId)
            ).FirstOrDefault();

            ConversationDto dto = null;
            if(cv != null){
                dto = new ConversationDto();
                dto.Id = cv.Id.ToString();
                dto.messages = GetMessagesForConversation(cv.Id.ToString());
                dto.partyId1 = cv.PartyId1;
                dto.partyId2 = cv.PartyId2;
            }

            return dto;
        }

        public ReceivedContactRequestDto AcceptContactRequest(ReceivedContactRequestDto req)
        {
            var rec = _context.ContactRequests.Find(req.Id);

            if(rec != null)
            {
                var newContact1 = new AcceptedContact();
                newContact1.UserId = req.RequesterId;
                newContact1.ContactId = req.RequesteeId;
                
                _context.AcceptedContacts.Add(newContact1);

                var newContact2 = new AcceptedContact();
                newContact2.UserId = req.RequesteeId;
                newContact2.ContactId = req.RequesterId;

                _context.AcceptedContacts.Add(newContact2);

                _context.ContactRequests.Remove(rec);

                _context.SaveChanges();
                
            }

            return req;
        }

        public IEnumerable<ContactDto> GetContactsForUserId(string userid)
        {
            int id = Convert.ToInt32(userid);
            var user = _context.ChatUsers.Find(id);
            var contacts = GetMyContacts(user);

            return contacts;
        }

        public IEnumerable<ContactDto> GetMyContacts(User currentUser)
        {
            var accepted = _context.AcceptedContacts.Where(x => x.UserId == currentUser.Id).ToList();

            var contacts = new List<ContactDto>();

            foreach (var ac in accepted)
            {
                var user = _context.ChatUsers.Find(ac.ContactId);

                if(user != null){
                    var con = _mapper.Map<ContactDto>(user);

                    contacts.Add(con);
                }
            }

            return contacts;
        }
        
        public IEnumerable<ReceivedContactRequestDto> GetReceivedRequests(User currentUser)
        {

            var reqs = _context.ContactRequests.Where(x => x.RequesteeId == currentUser.Id).ToList();

            var receivedRequests = new List<ReceivedContactRequestDto>();

            foreach (var r in reqs)
            {
                var user = _context.ChatUsers.Find(r.RequesterId);

                ReceivedContactRequestDto rr = null;

                if(user != null)
                {
                    rr = new ReceivedContactRequestDto
                    {
                        Id = r.Id,
                        RequesteeId = r.RequesteeId,
                        RequesterId = r.RequesterId,
                        FullName = user.FullName,
                        ProfileImgUrl = user.ProfileImgUrl
                    };
                }

                if(rr != null)
                {
                    receivedRequests.Add(rr);
                }
                
            }


            return receivedRequests;
        }

        public void Delete(int id)
        {
            var req = _context.ContactRequests.Find(id);
            if (req != null)
            {
                _context.ContactRequests.Remove(req);
                _context.SaveChanges();
            }
        }

       
       
    }
}
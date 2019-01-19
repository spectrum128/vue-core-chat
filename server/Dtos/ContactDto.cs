
using System;
using System.Collections.Generic;

namespace StChat.Dtos
{

    public class ContactDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ProfileImgUrl { get; set; }
        public bool Online { get; set; }
        public string Status { get; set; }

    }

    public class ContactRequestDto
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int RequesteeId { get; set; }

    }

    public class ReceivedContactRequestDto : ContactRequestDto
    {
        public string FullName { get; set; } 
        public string ProfileImgUrl { get; set; }        
    }

    public class ContactMessageDto
    {
        public int Id { get; set; }
        public string conversationId { get; set; }
        public string toName { get; set; }
        public string toId { get; set; }
        public string message { get; set; }
        public string fromName { get; set; }
        public string fromId { get; set; }
        public DateTime? sentOn { get; set; }

    }

    public class ConversationDto
    {
        public string Id { get; set; }
        public int partyId1 { get; set; }
        public int partyId2 { get; set; }
        public IEnumerable<ContactMessageDto> messages { get; set; }
    }

    
}
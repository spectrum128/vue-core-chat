using System;

namespace StChat.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ProfileImgUrl { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool Online { get; set; }
        public string Status { get; set; }

    }

    public class ContactRequest
    {
        public int Id { get; set; } 
        public int RequesterId { get; set; }
        public int RequesteeId { get; set; }

    }

    public class AcceptedContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactId { get; set; }

    }

    public class Conversation
    {
        public Guid Id { get; set; }
        public int PartyId1 { get; set; }
        public int PartyId2 { get; set; }

    }

    public class Message
    {
        public int Id { get; set; }
        public Guid ConversationId { get; set; }
        public string ToName { get; set; }
        public int ToId { get; set; }
        public string FromName { get; set; }
        public int FromId { get; set; }
        public string MessageText { get; set; }
        public DateTime? SentOn {get; set;}

    }

    
}
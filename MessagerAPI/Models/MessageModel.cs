using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MessagerAPI.Models

{
public class MessageModel
        {
            [Key]
            public int Id { get; set; }

            public string Content { get; set; }

            public DateTime SentAt { get; set; }

            // Foreign key for Sender
            [ForeignKey("Sender")]
            public string SenderEmail { get; set; }
            public IdentityUser Sender { get; set; }

            // Foreign key for Receiver
            [ForeignKey("Receiver")]
            public string ReceiverEmail { get; set; }
            public IdentityUser Receiver { get; set; }

            [ForeignKey("ConversationId")]
            public int ConversationId{ get; set; }
            public Conversation Conversation { get; set; }
}

  }






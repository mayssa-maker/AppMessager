using System;
using System.ComponentModel.DataAnnotations;

namespace MessagerAPI.Models
{

    public class MessageModel
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime? SentAt { get; set; }

        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }}






using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MessagerAPI.Models
{
  public class Conversation
{    [Key]
    public int ConversationId { get; set; }
    public ICollection<MessageModel> Messages { get; set; }
}
}
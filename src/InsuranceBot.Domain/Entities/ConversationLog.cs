using System;

namespace InsuranceBot.Domain.Entities;

public class ConversationLog
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public DateTime Timestamp { get; set; }
}
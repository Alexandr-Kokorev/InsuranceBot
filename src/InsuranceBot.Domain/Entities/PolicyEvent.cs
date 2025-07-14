using System;

namespace InsuranceBot.Domain.Entities;

public class PolicyEvent
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; }
    public string EventType { get; set; }
    public DateTime Timestamp { get; set; }
}
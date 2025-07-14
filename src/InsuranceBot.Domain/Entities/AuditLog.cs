using System;

namespace InsuranceBot.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string Action { get; set; }
    public string State { get; set; }
    public DateTime Timestamp { get; set; }
}
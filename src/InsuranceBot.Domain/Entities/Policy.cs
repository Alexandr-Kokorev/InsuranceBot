using System;

namespace InsuranceBot.Domain.Entities;

public class Policy
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string FilePath { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string Status { get; set; }
}
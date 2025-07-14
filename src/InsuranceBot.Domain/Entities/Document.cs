using System;

namespace InsuranceBot.Domain.Entities;

public class Document
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string Type { get; set; }
    public string Path { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Hash { get; set; }
    public Guid SessionId { get; set; }
}
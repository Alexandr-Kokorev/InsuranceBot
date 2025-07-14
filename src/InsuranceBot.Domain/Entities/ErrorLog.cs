using System;

namespace InsuranceBot.Domain.Entities;

public class ErrorLog
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public DateTime Timestamp { get; set; }
}
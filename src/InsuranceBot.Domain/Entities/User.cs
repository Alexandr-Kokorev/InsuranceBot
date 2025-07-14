using System;

namespace InsuranceBot.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public long TelegramUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string CurrentState { get; set; }
    public int UploadAttempts { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsAdmin { get; set; }
}
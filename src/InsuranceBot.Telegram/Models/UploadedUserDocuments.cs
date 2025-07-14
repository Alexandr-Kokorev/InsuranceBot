using InsuranceBot.Domain.Enums;

namespace InsuranceBot.Telegram.Models;

public class UploadedUserDocuments
{
    public int Count { get; set; }
    public DocumentType Type { get; set; }
    public bool IsConfirmed { get; set; }
    public Guid SessionUuid { get; set; }
}
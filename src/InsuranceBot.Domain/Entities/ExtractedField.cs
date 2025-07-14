using System;

namespace InsuranceBot.Domain.Entities;

public class ExtractedField
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string FieldName { get; set; }
    public string FieldValue { get; set; }
}
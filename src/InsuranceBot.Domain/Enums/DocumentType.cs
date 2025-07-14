using System.ComponentModel;

namespace InsuranceBot.Domain.Enums;

public enum DocumentType
{
    [Description("passport_upload")] Passport,
    [Description("vehicle_docs_upload")] VehiclesLicense
}
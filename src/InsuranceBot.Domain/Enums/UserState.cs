using System.ComponentModel;

namespace InsuranceBot.Domain.Enums;

public enum UserState
{
    [Description("AwaitingDocumentConfirmation")] AwaitingDocumentConfirmation,
    [Description("AwaitingDocumentUpload")] AwaitingDocumentUpload,
    [Description("AwaitingPriceConfirmation")] AwaitingPriceConfirmation,
    [Description("PolicyApprovalPending")] PolicyApprovalPending,
    [Description("PolicyGeneratingPending")] PolicyGeneratingPending
}
using MediatR;

namespace InsuranceBot.Application.Commands;

public record ConfirmExtractedDataCommand(long TelegramUserId, bool PartlyConfirmed, bool Confirmed) : IRequest;
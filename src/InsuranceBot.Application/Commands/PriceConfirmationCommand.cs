using MediatR;

namespace InsuranceBot.Application.Commands;

public record PriceConfirmationCommand(long TelegramUserId, bool Confirmed) : IRequest;
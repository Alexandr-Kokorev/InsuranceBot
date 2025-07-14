using MediatR;

namespace InsuranceBot.Application.Commands;

public record ResendPolicyCommand(long TelegramUserId) : IRequest;
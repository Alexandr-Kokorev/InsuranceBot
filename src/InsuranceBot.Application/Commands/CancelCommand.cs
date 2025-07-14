using MediatR;

namespace InsuranceBot.Application.Commands;

public record CancelCommand(long TelegramUserId) : IRequest;
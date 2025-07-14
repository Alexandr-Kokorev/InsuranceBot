using MediatR;

namespace InsuranceBot.Application.Commands;

public record StartCommand(long TelegramUserId) : IRequest;
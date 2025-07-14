using MediatR;

namespace InsuranceBot.Application.Commands;

public record AdminSummaryCommand(long TelegramUserId) : IRequest;
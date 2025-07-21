using MediatR;

namespace InsuranceBot.Application.Commands;

public record GenerateAiResponseCommand(long UserId, string Message) : IRequest;
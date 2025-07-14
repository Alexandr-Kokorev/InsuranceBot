using System;
using MediatR;

namespace InsuranceBot.Application.Commands;

public record GeneratePolicyCommand(long TelegramUserId, Guid SessionUuid) : IRequest;
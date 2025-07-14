using System;
using System.IO;
using InsuranceBot.Domain.Enums;
using MediatR;

namespace InsuranceBot.Application.Commands;

public record UploadDocumentCommand(
    long TelegramUserId,
    Stream FileStream,
    string Type,
    DocumentType DocumentType,
    Guid SessionUuid) : IRequest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Repositories;

public interface IDocumentRepository
{
    Task<bool> ExistsDuplicateAsync(long userId, string hash);
    Task SaveExtractedFieldsAsync(long userId, Guid sessionUuid, string documentPath, Dictionary<string, string> fields, string hash);
    Task<Dictionary<string, string>> GetExtractedFieldsAsync(long userId, Guid sessionUuid);
}
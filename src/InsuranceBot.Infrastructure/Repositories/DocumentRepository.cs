using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBot.Domain.Entities;
using InsuranceBot.Domain.Interfaces.Repositories;
using InsuranceBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InsuranceBot.Infrastructure.Repositories;

public class DocumentRepository(AppDbContext db) : IDocumentRepository
{
    public async Task<bool> ExistsDuplicateAsync(long userId, string hash)
    {
        return await db.Documents.AnyAsync(d => d.UserId == userId && d.Hash == hash);
    }

    public async Task SaveExtractedFieldsAsync(long userId, Guid sessionUuid, string documentPath, Dictionary<string, string> fields,
        string hash)
    {
        // Save document record (if not exists)
        Document doc = new Document
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = Path.GetExtension(documentPath).ToLower() == ".pdf" ? "Policy" : "Upload",
            Path = documentPath,
            UploadedAt = DateTime.UtcNow,
            Hash = hash,
            SessionId = sessionUuid
        };
        db.Documents.Add(doc);

        // Save each extracted field (OCR)
        foreach (KeyValuePair<string, string> field in fields)
        {
            db.ExtractedFields.Add(new ExtractedField
            {
                Id = Guid.NewGuid(),
                DocumentId = doc.Id,
                FieldName = field.Key,
                FieldValue = field.Value
            });
        }

        await db.SaveChangesAsync();
    }

    public async Task<Dictionary<string, string>> GetExtractedFieldsAsync(long userId, Guid sessionUuid)
    {
        List<Document> doc =
            await db.Documents.Where(d => d.UserId == userId && d.SessionId == sessionUuid).ToListAsync();
        if (doc.Count == 0)
            return new Dictionary<string, string>();

        return await db.ExtractedFields
            .Where(f => doc.Select(d => d.Id).Contains(f.DocumentId))
            .ToDictionaryAsync(f => f.FieldName, f => f.FieldValue);
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using InsuranceBot.Domain.Interfaces.Services;

namespace InsuranceBot.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _docRoot = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    private readonly string _policyRoot = Path.Combine(Directory.GetCurrentDirectory(), "policies");

    public FileStorageService()
    {
        Directory.CreateDirectory(_docRoot);
        Directory.CreateDirectory(_policyRoot);
    }

    public async Task<string> SaveDocumentAsync(long userId, Stream file, string type)
    {
        string fileName = $"{userId}_{type}_{Guid.NewGuid()}.jpg";
        string path = Path.Combine(_docRoot, fileName);
        await using FileStream fs = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fs);
        return path;
    }

    public async Task<Stream> GetDocumentAsync(string path)
        => File.Exists(path) ? new FileStream(path, FileMode.Open, FileAccess.Read) : Stream.Null;

    public string GetDocumentName(string path)
        => Path.GetFileName(path);

    public async Task<string> SavePolicyPdfAsync(long userId, byte[] pdfBytes)
    {
        string fileName = $"{userId}_policy_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
        string path = Path.Combine(_policyRoot, fileName);
        await File.WriteAllBytesAsync(path, pdfBytes);
        return path;
    }
}
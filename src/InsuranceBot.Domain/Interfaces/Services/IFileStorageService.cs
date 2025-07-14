using System.IO;
using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> SaveDocumentAsync(long userId, Stream file, string type);
    Task<Stream> GetDocumentAsync(string path);
    string GetDocumentName(string path);
    Task<string> SavePolicyPdfAsync(long userId, byte[] pdfBytes);
}
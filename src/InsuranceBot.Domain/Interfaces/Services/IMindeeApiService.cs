using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IMindeeApiService
{
    Task<Dictionary<string, string>> ExtractPassportFieldsAsync(string filePath);
    Task<Dictionary<string, string>> ExtractVehicleCertificateFieldsAsync(string filePath);
}
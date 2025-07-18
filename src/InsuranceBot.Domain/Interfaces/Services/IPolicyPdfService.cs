using System.Collections.Generic;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IPolicyPdfService
{
    byte[] GeneratePolicyPdfAsync(Dictionary<string, string> userData);
}
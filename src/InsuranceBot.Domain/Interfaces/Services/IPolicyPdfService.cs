using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IPolicyPdfService
{
    Task<byte[]> GeneratePolicyPdfAsync(Dictionary<string, string> userData, DateTime expiry);
}
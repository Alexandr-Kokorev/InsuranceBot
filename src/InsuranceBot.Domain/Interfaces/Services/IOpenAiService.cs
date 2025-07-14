using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceBot.Domain.Interfaces.Services;

public interface IOpenAiService
{
    Task<string> GetSmartReplyAsync(string prompt, string userInputFallback);
    Task<string> GeneratePolicyTextAsync(Dictionary<string, string> userData);
}
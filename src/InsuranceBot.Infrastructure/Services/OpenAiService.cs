using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Infrastructure.Models;
using Microsoft.Extensions.Configuration;

namespace InsuranceBot.Infrastructure.Services;

public class OpenAiService(IConfiguration config, HttpClient client) : IOpenAiService
{
    private readonly string _apiKey = config["OPENAI_API_KEY"];

    public async Task<string> GetSmartReplyAsync(string prompt, string userInputFallback)
    {
        var body = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        HttpResponseMessage response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", body);
       
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            //throw new Exception($"Error while accessing OpenAI API: {error}");
        }
        
        OpenAiResponse result = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
        return result?.Choices?.FirstOrDefault()?.Message?.Content ?? userInputFallback;
    }

    public async Task<string> GeneratePolicyTextAsync(Dictionary<string, string> userData)
    {
        string prompt =
            $"Generate personalized car insurance policy text for: {string.Join(", ", userData.Select(x => $"{x.Key}: {x.Value}"))}. Include expiry in 7 days.";
        
        string userInputFallback = "Please provide a policy text.";
        
        return await GetSmartReplyAsync(prompt, userInputFallback);
    }
}
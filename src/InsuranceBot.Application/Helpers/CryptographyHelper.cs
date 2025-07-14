using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace InsuranceBot.Application.Helpers;

public static class CryptographyHelper
{
    public static async Task<string> ComputeSha256Async(Stream stream)
    {
        using SHA256 sha = SHA256.Create();
        stream.Position = 0;
        byte[] hash = await sha.ComputeHashAsync(stream);
        stream.Position = 0;
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
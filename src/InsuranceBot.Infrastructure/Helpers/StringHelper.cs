using System;
using System.IO;

namespace InsuranceBot.Infrastructure.Helpers;

public static class StringHelper
{
    public static string StreamToBase64(this Stream imageStream)
    {
        ArgumentNullException.ThrowIfNull(imageStream);

        if (imageStream.CanSeek)
            imageStream.Position = 0;

        using MemoryStream memoryStream = new MemoryStream();
        imageStream.CopyTo(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }
}
using System.Collections.Generic;
using System.IO;
using InsuranceBot.Domain.Interfaces.Services;
using iTextSharp.text.pdf;

namespace InsuranceBot.Infrastructure.Services;

public class PolicyPdfService : IPolicyPdfService
{
    private readonly string _templatePath = Path.Combine("templates", "policy_template.pdf");

    public byte[] GeneratePolicyPdfAsync(Dictionary<string, string> userData)
    {
        using MemoryStream ms = new MemoryStream();
        using PdfReader reader = new PdfReader(_templatePath);
        using PdfStamper stamper = new PdfStamper(reader, ms);
        
        AcroFields form = stamper.AcroFields;
        
        foreach (KeyValuePair<string, string> field in userData)
        {
            form.SetField(field.Key, field.Value ?? "");
        }

        stamper.FormFlattening = true;
        stamper.Close();
        
        return ms.ToArray();
    }
}
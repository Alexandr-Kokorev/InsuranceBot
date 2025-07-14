using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InsuranceBot.Domain.Interfaces.Services;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;

namespace InsuranceBot.Infrastructure.Services;

public class PolicyPdfService : IPolicyPdfService
{
    private readonly string _templatePath = Path.Combine("templates", "policy_template.pdf");

    public async Task<byte[]> GeneratePolicyPdfAsync(Dictionary<string, string> userData, DateTime expiryDate)
    {
        await using FileStream templateStream = File.OpenRead(_templatePath);
        using PdfDocument pdfDoc = PdfReader.Open(templateStream, PdfDocumentOpenMode.Modify);
        PdfAcroForm form = pdfDoc.AcroForm;
        if (form != null)
        {
            form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            SetField(form, "BirthDate", userData.GetValueOrDefault("BirthDate"));
            SetField(form, "IdNumber", userData.GetValueOrDefault("IdNumber"));
            SetField(form, "Name", userData.GetValueOrDefault("Name"));
            SetField(form, "LicensePlate", userData.GetValueOrDefault("LicensePlate"));
            SetField(form, "Model", userData.GetValueOrDefault("Model"));
            SetField(form, "Vin", userData.GetValueOrDefault("Vin"));
            SetField(form, "Year", userData.GetValueOrDefault("Year"));
            SetField(form, "ExpiryDate", expiryDate.ToString("yyyy-MM-dd"));
        }
        using MemoryStream ms = new MemoryStream();
        pdfDoc.Save(ms, false);
        return ms.ToArray();
    }

    private void SetField(PdfAcroForm form, string fieldName, string? value)
    {
        if (form.Fields[fieldName] is PdfTextField field)
        {
            field.Value = new PdfString(value ?? String.Empty);
        }
        else
        {
            // For development: throw an error if the PDF field is missing (optional)
            // throw new InvalidOperationException($"PDF form field '{fieldName}' not found.");
        }
    }
}
using InsuranceBot.Domain.Enums;
using InsuranceBot.Telegram.Models;

namespace InsuranceBot.Telegram.Helper;

// Document counter
public static class UploadDocumentManagerHelper
{
    public static void UpdateUserDocsUploadList(this List<UploadedUserDocuments> uploadedDocuments, Guid sessionUuid)
    {
        switch (uploadedDocuments.Count)
        {
            case 0:
                uploadedDocuments.AddPassportDocument(sessionUuid);
                break;
            case 1:
                uploadedDocuments.AddVehicleLicenseDocument(sessionUuid);
                break;
        }
    }

    public static void MarkAsConfirmed(this List<UploadedUserDocuments> uploadedDocuments)
    {
        UploadedUserDocuments? existingPassportDoc =
            uploadedDocuments.FirstOrDefault(x => x is { Type: DocumentType.Passport, IsConfirmed: false });
        if (existingPassportDoc is { Count: 1 })
        {
            existingPassportDoc.IsConfirmed = true;
        }

        UploadedUserDocuments? existingVehicleDoc =
            uploadedDocuments.FirstOrDefault(x => x is { Type: DocumentType.VehiclesLicense, IsConfirmed: false });
        if (existingVehicleDoc is { Count: 2 })
        {
            existingVehicleDoc.IsConfirmed = true;
        }
    }
    
    public static UploadedUserDocuments GetDocumentTypeToUpload(this List<UploadedUserDocuments> uploadedDocuments, Guid sessionUuid)
    {
        if (uploadedDocuments.Count == 2 && uploadedDocuments.FirstOrDefault() is { IsConfirmed: true })
            return uploadedDocuments.FirstOrDefault(x => x.Type == DocumentType.VehiclesLicense)!;
        if (uploadedDocuments.FirstOrDefault(x => x.Type == DocumentType.Passport) == null)
            uploadedDocuments.AddPassportDocument(sessionUuid);
        return uploadedDocuments.FirstOrDefault(x => x.Type == DocumentType.Passport)!;
    }

    public static void AddPassportDocument(this List<UploadedUserDocuments> uploadedDocuments, Guid sessionUuid)
    {
        UploadedUserDocuments? existingDoc =
            uploadedDocuments.FirstOrDefault(x => x.Type == DocumentType.Passport);

        if (existingDoc is { Count: 1 })
        {
            return;
        }

        uploadedDocuments.Add(new UploadedUserDocuments
        {
            Count = 1,
            Type = DocumentType.Passport,
            IsConfirmed = false,
            SessionUuid = sessionUuid
        });
    }

    public static void AddVehicleLicenseDocument(this List<UploadedUserDocuments> uploadedDocuments, Guid sessionUuid)
    {
        UploadedUserDocuments? existingDoc =
            uploadedDocuments.FirstOrDefault(x => x.Type == DocumentType.VehiclesLicense);

        if (existingDoc is { Count: 2 })
        {
            return;
        }

        uploadedDocuments.Add(new UploadedUserDocuments
        {
            Count = 2,
            Type = DocumentType.VehiclesLicense,
            IsConfirmed = false,
            SessionUuid = sessionUuid
        });
    }

    public static void CleanUploadedDocumentsList(this List<UploadedUserDocuments> uploadedDocuments)
    {
        uploadedDocuments.Clear();
    }
}
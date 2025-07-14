using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceBot.Domain.Interfaces.Services;
using InsuranceBot.Infrastructure.Helpers;
using Mindee;
using Mindee.Input;
using Mindee.Parsing.Common;
using Mindee.Product.Passport;

namespace InsuranceBot.Infrastructure.Services;

public class MindeeApiService(MindeeClient client) : IMindeeApiService
{
    public async Task<Dictionary<string, string>> ExtractPassportFieldsAsync(string filePath)
    {
        LocalInputSource inputSource = new LocalInputSource(filePath);
        PredictResponse<PassportV1> response = await client
            .ParseAsync<PassportV1>(inputSource);
        
        PassportV1Document responseDoc = response.Document.Inference.Prediction;

        Dictionary<string, string> fields = new Dictionary<string, string>
        {
            { "IdNumber", responseDoc.IdNumber.Value },
            { "Name", $"{String.Join(" ", responseDoc.GivenNames.Select(s => s.Value))}" },
            { "BirthDate", responseDoc.BirthDate.Value }
        };

        return fields;
    }

    public async Task<Dictionary<string, string>> ExtractVehicleCertificateFieldsAsync(string filePath)
    {
        /*LocalInputSource inputSource = new LocalInputSource(filePath);
        PredictResponse<PassportV1> response = await client
            .ParseAsync<PassportV1>(inputSource);
        
        PassportV1Document responseDoc = response.Document.Inference.Prediction;*/

        // Mock data for vehicle certificate due to Mindee API limitations
        Dictionary<string, string> fields = new Dictionary<string, string>
        {
            { "LicensePlate", VehicleDataMockHelper.GetRandomLicensePlate() },
            { "Model", VehicleDataMockHelper.GetRandomCarModel() },
            { "Year", VehicleDataMockHelper.GetRandomProductionYear() },
            { "Vin", VehicleDataMockHelper.GetRandomVinCode() }
        };

        return fields;
    }
}
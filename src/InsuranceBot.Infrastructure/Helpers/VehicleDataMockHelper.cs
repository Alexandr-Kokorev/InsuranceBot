using System;

namespace InsuranceBot.Infrastructure.Helpers;

public static class VehicleDataMockHelper
{
    private static readonly string[] LicensePlateFormats =
    [
        "AAA-####", // Example: ABC-1234
        "##-AAA-##", // Example: 12-XYZ-34
        "A## AAA"    // Example: B12 XYZ
    ];
    
    private static readonly string[] CarModels =
    [
        "Toyota Corolla",
        "Volkswagen Golf",
        "Ford Focus",
        "Honda Civic",
        "BMW 3 Series",
        "Mercedes-Benz C-Class",
        "Audi A4",
        "Hyundai Elantra",
        "Kia Sportage",
        "Mazda CX-5"
    ];

    private static readonly Random _random = new Random();
    
    public static string GetRandomLicensePlate()
    {
        var format = LicensePlateFormats[_random.Next(LicensePlateFormats.Length)];
        var plate = "";
        foreach (var c in format)
        {
            if (c == '#')
                plate += _random.Next(10);
            else if (c == 'A')
                plate += (char)('A' + _random.Next(26));
            else
                plate += c;
        }
        return plate;
    }

    public static string GetRandomCarModel()
    {
        return CarModels[_random.Next(CarModels.Length)];
    }

    public static string GetRandomProductionYear(int minYear = 1995, int maxYear = 2024)
    {
        return _random.Next(minYear, maxYear + 1).ToString();
    }

    public static string GetRandomVinCode()
    {
        const string chars = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789"; // VIN does not use I, O, Q
        var vin = new char[17];
        for (int i = 0; i < 17; i++)
        {
            vin[i] = chars[_random.Next(chars.Length)];
        }
        return new string(vin);
    }
}
using System;
using System.ComponentModel;
using System.Reflection;

namespace InsuranceBot.Application.Helpers;

public static class EnumHelper
{
    public static T GetValueFromDescription<T>(this string description) where T : struct, System.Enum
    {
        foreach (FieldInfo field in typeof(T).GetFields())
        {
            if (!IsMatchFound(field, description)) continue;
                
            return (T)field.GetValue(null)!;
        }

        return default;
    }
    
    private static bool IsMatchFound(FieldInfo field, string description)
    {
        DescriptionAttribute descAttr = field.GetCustomAttribute<DescriptionAttribute>();
        bool match = descAttr?.Description.Equals(description, StringComparison.OrdinalIgnoreCase)
                     ?? field.Name.Equals(description, StringComparison.OrdinalIgnoreCase);

        return match;
    }
}
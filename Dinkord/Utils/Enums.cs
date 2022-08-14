using System;
using System.ComponentModel;

namespace Dinkord.Utils;

public static class Enums
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        var keyAttributes =
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return keyAttributes.Length > 0 ? keyAttributes[0].Description : enumValue.ToString();
    }

    public static string GetKeyName(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        var keyAttributes =
            (KeyAttribute[])fieldInfo.GetCustomAttributes(typeof(KeyAttribute), false);

        return keyAttributes.Length > 0 ? keyAttributes[0].KeyName : enumValue.ToString();
    }

    public static string GetKeyDescription(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        var keyAttributes =
            (KeyAttribute[])fieldInfo.GetCustomAttributes(typeof(KeyAttribute), false);

        return keyAttributes.Length > 0 ? keyAttributes[0].Description : enumValue.ToString();
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class KeyAttribute : Attribute
{
    public KeyAttribute(string keyName, string description)
    {
        KeyName = keyName;
        Description = description;
    }

    public string KeyName { get; }
    public string Description { get; }
}
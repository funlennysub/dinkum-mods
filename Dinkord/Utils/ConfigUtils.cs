using System.Text;
using Dinkord.Discord;

namespace Dinkord.Utils;

public static class ConfigUtils
{
    public static string GenerateConfigDescription(params LargeKeys[] keys)
    {
        var sb = new StringBuilder();

        foreach (var key in keys)
        {
            sb.Append(key.ToString());
            sb.Append(": ");
            sb.AppendLine(key.GetKeyDescription());
        }

        return sb.ToString();
    }

    public static string GenerateConfigDescription(params SmallKeys[] keys)
    {
        var sb = new StringBuilder();

        foreach (var key in keys)
        {
            sb.Append(key.ToString());
            sb.Append(": ");
            sb.AppendLine(key.GetKeyDescription());
        }

        return sb.ToString();
    }

    public static string GenerateConfigDescription(params RichPresence.SmallImageFeatures[] features)
    {
        var sb = new StringBuilder();

        foreach (var key in features)
        {
            sb.Append(key.ToString());
            sb.Append(": ");
            sb.AppendLine(key.GetEnumDescription());
        }

        return sb.ToString();
    }
}
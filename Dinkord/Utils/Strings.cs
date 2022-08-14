using System.Globalization;

namespace Dinkord.Utils;

public static class Strings
{
    public static string ToTitleCase(this string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
    }
}
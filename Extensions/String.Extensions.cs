using System.Text.RegularExpressions;

namespace AzureBlobStorageTest.Extensions;

public static class StringExtensioes
{
    public static string RemoveWhiteSpace(this string value)
    {
        return new string(value.Where(v => !Char.IsWhiteSpace(v)).ToArray());
    }

    public static string RemoveAllWhiteSpace(this string value)
    {
        return Regex.Replace(value, @"\s+", "");
    }
}

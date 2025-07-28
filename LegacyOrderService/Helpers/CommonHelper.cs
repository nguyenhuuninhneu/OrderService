using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LegacyOrderService.Helpers;

public static class CommonHelper
{
    public static bool IsEmpty(this string someString)
    {
        return string.IsNullOrEmpty(someString) || string.IsNullOrWhiteSpace(someString);
    }
    
    public static bool HasValue(this string someString)
    {
        return !someString.IsEmpty();
    }

    public static void ThenThrow(this bool value, Exception ex)
    {
        if (value)
        {
            throw ex;
        }
    }
    

    public static string ToSerializedString(this object obj)
    {
        if (obj == default)
        {
            return string.Empty;
        }
        
        return JsonSerializer.Serialize(obj);
    }

    public static string ToMd5Hash(this string someString)
    {
        using var md5 = MD5.Create();
        var result = md5.ComputeHash(Encoding.UTF8.GetBytes(someString));
        return string.Concat(result.Select(b => b.ToString("X2")));
    }
}
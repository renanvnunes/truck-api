using System.Buffers.Text;
using System.Security.Cryptography;

namespace TruckApi.Shared.Utils;

public static class TokenGenerator
{
    public static string New() => Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(32));
}

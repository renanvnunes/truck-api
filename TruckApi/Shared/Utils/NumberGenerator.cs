using System.Security.Cryptography;
using System.Text;

namespace TruckApi.Shared.Utils;

public static class NumberGenerator
{
    public static string New(int length)
    {
        StringBuilder sb = new(length);

        for (int i = 0; i < length; i++)
        {
            int digito = RandomNumberGenerator.GetInt32(0, 10);
            sb.Append(digito);
        }

        return sb.ToString();
    }
}

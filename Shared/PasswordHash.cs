using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace TruckApi.Shared;

public static class PasswordHash
{
    // Parâmetros recomendados pela OWASP para um bom equilíbrio entre segurança e performance
    private const int DegreeOfParallelism = 8; // Número de threads (núcleos) a serem usados
    private const int MemorySizeInKb = 65536; // 64 MB de RAM
    private const int Iterations = 4; // Número de passagens na memória
    private const int SaltSize = 16; // 128 bits de Salt
    private const int HashSize = 32; // 256 bits de Hash final

    public static string Hash(string password)
    {
        // 1. Gera um Salt único e aleatório para cada senha
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            MemorySize = MemorySizeInKb,
            Iterations = Iterations,
        };

        var hash = argon2.GetBytes(HashSize);

        Console.WriteLine($"Password: {password}");

        // 2. Concatena o Salt e o Hash para poder salvar no banco em uma única string formatada
        // Formato padrão de mercado: $argon2id$v=19$m=65536,t=4,p=8$base64(salt)$base64(hash)
        var saltBase64 = Convert.ToBase64String(salt);
        var hashBase64 = Convert.ToBase64String(hash);

        return $"$argon2id$m={MemorySizeInKb},t={Iterations},p={DegreeOfParallelism}${saltBase64}${hashBase64}";
    }

    public static bool Verify(string password, string hashedPassword)
    {
        try
        {
            // 1. Faz o parse da string do banco para extrair os parâmetros, salt e hash original
            var parts = hashedPassword.Split('$');
            if (parts.Length < 5)
            {
                return false;
            }

            // Extrai as configurações usadas no momento do hash (evita quebrar se você mudar os parâmetros globais no futuro)
            var config = parts[2].Split(',');
            var memory = int.Parse(config[0].Split('=')[1]);
            var iterations = int.Parse(config[1].Split('=')[1]);
            var parallelism = int.Parse(config[2].Split('=')[1]);

            var salt = Convert.FromBase64String(parts[3]);
            var originalHash = Convert.FromBase64String(parts[4]);

            // 2. Computa o hash da senha enviada usando o MESMO salt e parâmetros extraídos
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = parallelism,
                MemorySize = memory,
                Iterations = iterations,
            };

            var newHash = argon2.GetBytes(originalHash.Length);

            // 3. Comparação em tempo constante (Cryptographic safe) para evitar Timing Attacks
            return CryptographicOperations.FixedTimeEquals(originalHash, newHash);
        }
        catch
        {
            // Qualquer erro de parse ou formato inválido resulta em falha de verificação
            return false;
        }
    }
}

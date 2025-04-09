using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using NanoidDotNet;

namespace LicenseTokenTester
{
    public class AvaClientLicense
    {
        public string Id { get; set; } = Nanoid.Generate(
            alphabet: Nanoid.Alphabets.UppercaseLettersAndDigits, 
            size: 32
        );
        public string ClientID { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string AppId { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
        public decimal SpendThreshold { get; set; }
        public string IssuedBy { get; set; } = string.Empty;
        public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
    }

    public class LicensePayload
    {
        public required string Id { get; set; }
        public required string ClientID { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public static class LicenseTokenHelper
    {
        private static readonly string secretKey = "super-secret-key-change-this";

        public static string GenerateToken(AvaClientLicense license)
        {
            var payload = new LicensePayload
            {
                Id = license.Id,
                ClientID = license.ClientID,
                ExpiryDate = license.ExpiryDate
            };

            var json = JsonSerializer.Serialize(payload);
            var payloadBytes = Encoding.UTF8.GetBytes(json);
            var payloadBase64 = Convert.ToBase64String(payloadBytes);

            var signature = ComputeHmacSha256(payloadBase64, secretKey);
            var signatureBase64 = Convert.ToBase64String(signature);

            return $"{payloadBase64}.{signatureBase64}";
        }

        public static bool TryValidate(string token, out LicensePayload? payload)
        {
            payload = null;
            var parts = token.Split('.');
            if (parts.Length != 2) return false;

            var payloadBase64 = parts[0];
            var signatureBase64 = parts[1];

            var expectedSignature = Convert.ToBase64String(ComputeHmacSha256(payloadBase64, secretKey));

            if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expectedSignature),
                Encoding.UTF8.GetBytes(signatureBase64)))
            {
                return false;
            }

            var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(payloadBase64));
            payload = JsonSerializer.Deserialize<LicensePayload>(payloadJson);

            if (payload == null || payload.ExpiryDate < DateTime.UtcNow)
                return false;

            return true;
        }

        private static byte[] ComputeHmacSha256(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  generate --clientid <string> --expiry <int> --appid <string> --spendthreshold <int> --issuedby <string>");
                Console.WriteLine("  validate <token>");
                return;
            }

            var command = args[0].ToLower();
            if (command == "generate")
            {
                string? clientId = null, appId = null, issuedBy = null;
                int expiryDays = 0;
                decimal spendThreshold = 0;

                for (int i = 1; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--clientid": clientId = args[++i]; break;
                        case "--expiry": expiryDays = int.Parse(args[++i]); break;
                        case "--appid": appId = args[++i]; break;
                        case "--spendthreshold": spendThreshold = decimal.Parse(args[++i]); break;
                        case "--issuedby": issuedBy = args[++i]; break;
                    }
                }

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(issuedBy))
                {
                    Console.WriteLine("Missing required parameters.");
                    return;
                }

                var license = new AvaClientLicense
                {
                    ClientID = clientId,
                    ExpiryDate = DateTime.UtcNow.AddDays(expiryDays),
                    AppId = appId,
                    SpendThreshold = spendThreshold,
                    IssuedBy = issuedBy
                };

                var token = LicenseTokenHelper.GenerateToken(license);
                Console.WriteLine("Generated License Token:");
                Console.WriteLine(token);
            }
            else if (command == "validate" && args.Length == 2)
            {
                var token = args[1];
                Console.WriteLine("Validating Token...");
                if (LicenseTokenHelper.TryValidate(token, out var payload))
                {
                    Console.WriteLine("Token is valid.");
                    Console.WriteLine($"Id: {payload.Id}, ClientID: {payload.ClientID}, ExpiryDate: {payload.ExpiryDate}");
                }
                else
                {
                    Console.WriteLine("Token is invalid or expired.");
                }
            }
            else
            {
                Console.WriteLine("Invalid command or parameters.");
            }
        }
    }
}

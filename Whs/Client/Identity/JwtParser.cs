using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Whs.Client.Identity
{
    public class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            ExtractRolesFromJWT(claims, keyValuePairs);
            ExtractSurname(claims, keyValuePairs);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            var v = base64.Length % 4;
            switch (v)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            var result = Convert.FromBase64String(base64);
            return result;
        }

        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            _ = keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                try
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"ExtractRolesFromJWT JsonException: {Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    var parsedRole = roles.ToString();
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }
        }

        private static void ExtractSurname(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Surname, out object surnameObj);
            if (surnameObj != null)
            {
                var surnameBase64 = surnameObj.ToString();
                var surnameData = Convert.FromBase64String(surnameBase64);
                var surname = System.Text.Encoding.UTF8.GetString(surnameData);

                Claim claimForRemove = claims.Find(e => e.Type == ClaimTypes.Surname);
                claims.Remove(claimForRemove);
                Claim newClaim = new Claim(ClaimTypes.Surname, surname);
                claims.Add(newClaim);
            }
        }
    }
}

using System.Net;
using System.Net.Sockets;

namespace RiskMap.App;

/// <summary>
/// Application — responsable : Ilyass.
/// Vérifie la cible saisie avant de lancer un scan : une adresse IPv4 (« 192.168.56.101 »)
/// ou un réseau en notation CIDR (« 192.168.56.0/24 »).
/// </summary>
public static class ScanTarget
{
    /// <summary>Plus grand réseau accepté : /16 = 65 536 adresses. Au-delà, le scan prendrait des heures.</summary>
    public const int MinPrefix = 16;

    public static bool TryValidate(string text, out string error)
    {
        error = "";
        var parts = text.Trim().Split('/');

        if (parts.Length > 2 || !IPAddress.TryParse(parts[0], out var ip) || ip.AddressFamily != AddressFamily.InterNetwork
            || parts[0].Count(c => c == '.') != 3) // « 10.1 » est accepté par TryParse : on exige 4 nombres
        {
            error = $"Cible invalide : « {text} ». Exemples : 192.168.56.0/24 ou 192.168.56.101.";
            return false;
        }

        if (parts.Length == 2)
        {
            if (!int.TryParse(parts[1], out var prefix) || prefix < 0 || prefix > 32)
            {
                error = $"Masque invalide : « /{parts[1]} ». Il doit être entre /{MinPrefix} et /32 (ex. /24).";
                return false;
            }
            if (prefix < MinPrefix)
            {
                error = $"Réseau trop grand (/{prefix}) : limitez-vous à /{MinPrefix} ou plus petit (ex. /24 = 256 adresses).";
                return false;
            }
        }
        return true;
    }
}

/// <summary>Trie les adresses IP dans l'ordre numérique (192.168.56.2 avant 192.168.56.10).</summary>
public class IpComparer : IComparer<string>
{
    public static readonly IpComparer Instance = new();

    public int Compare(string? x, string? y)
    {
        if (IPAddress.TryParse(x, out var a) && IPAddress.TryParse(y, out var b))
            return ((ReadOnlySpan<byte>)a.GetAddressBytes()).SequenceCompareTo(b.GetAddressBytes());
        return string.Compare(x, y, StringComparison.Ordinal);
    }
}

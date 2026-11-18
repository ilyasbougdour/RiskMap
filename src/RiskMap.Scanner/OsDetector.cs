using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Scanner;

/// <summary>
/// Module Scanner — responsable : Ilyass.
/// Devine le système d'exploitation en croisant les indices déjà collectés.
/// </summary>
public class OsDetector : IOsDetector
{
    public void Detect(Host host)
    {
        // TODO (Ilyass) : additionner des indices, chacun avec un poids, et garder le système le plus probable.
        //   - TTL (host.Ttl) : ≤ 64 → Linux/macOS, ≤ 128 → Windows, ≤ 255 → équipement réseau / imprimante.
        //   - Bannières brutes : « Ubuntu », « Debian », « Microsoft-IIS », « Win64 »... nomment le système.
        //   - Combinaisons de ports : 135 + 445 (+ 3389) → Windows ; 9100 / 631 → imprimante.
        //   - Fabricant MAC (host.Vendor) : « Hewlett Packard », « Cisco »...
        //
        // Remplir host.Os :
        //   State      = Identified si au moins un indice, sinon Undetermined (jamais Hidden : un système ne se cache pas).
        //   Name       = ex. « Linux (Ubuntu) »
        //   Confidence = 0 à 100, selon le nombre et la force des indices
        //   Clues      = la liste des indices utilisés, affichée dans le rapport (ex. « TTL 64 »)
        throw new NotImplementedException("Détection du système pas encore écrite (OsDetector).");
    }
}

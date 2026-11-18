using RiskMap.Core.Models;

namespace RiskMap.Analysis;

/// <summary>Transforme une bannière brute en logiciel + version.</summary>
public class BannerParser
{
    /// <summary>Remplit service.VersionState, service.Product et service.Version à partir de service.RawBanner.</summary>
    public void Identify(Service service)
    {
        // TODO : une règle (expression régulière) par logiciel connu. Exemples à reconnaître :
        //   « 220 (vsFTPd 2.3.4) »                       → vsftpd 2.3.4            Identified
        //   « SSH-2.0-OpenSSH_8.9p1 Ubuntu-3ubuntu0.10 »  → OpenSSH 8.9p1           Identified
        //   « Apache/2.2.8 (Ubuntu) DAV/2 »               → Apache httpd 2.2.8      Identified
        //   « 5.0.51a-3ubuntu5 » sur le port 3306         → MySQL 5.0.51a           Identified
        //   « nginx » (sans version)                      → nginx, pas de version   Hidden
        //   null, ou texte non reconnu                    → rien                    Undetermined
        //
        // Hidden = le logiciel répond mais cache sa version (choix de l'administrateur).
        // Undetermined = on n'a pas réussi à lire l'information (limite de notre outil).
        //
        // Les noms de produits doivent correspondre à ceux de la NVD pour que la recherche de CVE fonctionne
        // (ex. « openssh », « http_server » chez « apache »). Prévoir une table de correspondance.
        throw new NotImplementedException("Lecture des bannières pas encore écrite (BannerParser).");
    }
}

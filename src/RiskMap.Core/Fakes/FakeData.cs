using RiskMap.Core.Models;
using RiskMap.Core.Storage;

namespace RiskMap.Core.Fakes;

/// <summary>
/// Réseau d'exemple : un poste Windows, une machine Metasploitable 2, un serveur web Ubuntu, une imprimante.
/// C'est la seule source des données d'exemple : les fichiers du dossier fixtures/ sont générés à partir d'ici
/// (commande « dotnet run --project src/RiskMap.Cli -- fixtures »).
///
/// Les CVE sont de vraies CVE connues pour ces versions, mais la liste est volontairement incomplète :
/// ce sont des données de test, pas un vrai résultat NVD.
/// </summary>
public static class FakeData
{
    public const string Target = "192.168.56.0/24";

    /// <summary>Étape 1 : ce que la découverte renvoie (IP, TTL, MAC).</summary>
    public static List<Host> Discovered()
    {
        var hosts = Analyzed().Hosts;
        foreach (var h in hosts)
        {
            h.Services = new();
            h.Os = new();
            h.Risk = null;
            h.AiSummary = null;
        }
        return hosts;
    }

    /// <summary>Étape 2 : après le scanner (ports, bannières brutes, système d'exploitation).</summary>
    public static List<Host> Scanned()
    {
        var hosts = Analyzed().Hosts;
        foreach (var h in hosts)
        {
            h.Risk = null;
            h.AiSummary = null;
            foreach (var s in h.Services)
            {
                s.VersionState = InfoState.Undetermined;
                s.Product = null;
                s.Version = null;
                s.Vulnerabilities = new();
            }
        }
        return hosts;
    }

    /// <summary>Étape 3 : résultat final, après l'analyse. C'est ce que l'interface et la carte affichent.</summary>
    public static ScanResult Analyzed() => JsonStore.Clone(_analyzed);

    private static readonly ScanResult _analyzed = new()
    {
        Target = Target,
        StartedAt = new DateTime(2026, 11, 18, 14, 0, 0),
        FinishedAt = new DateTime(2026, 11, 18, 14, 2, 30),
        Hosts =
        {
            // --- Poste Windows ----------------------------------------------------------
            new Host
            {
                IpAddress = "192.168.56.1",
                Hostname = "PC-ADMIN",
                MacAddress = "0A:00:27:00:00:11",
                Ttl = 128,
                Services =
                {
                    new Service { Port = 135, Name = "msrpc" },
                    new Service { Port = 139, Name = "netbios-ssn" },
                    new Service { Port = 445, Name = "smb" },
                },
                Os = new OsGuess
                {
                    State = InfoState.Identified, Name = "Windows", Confidence = 80,
                    Clues = { "TTL 128", "ports 135 + 445 ouverts" },
                },
                Risk = new RiskAssessment
                {
                    Score = 25, Level = RiskLevel.Medium,
                    Factors = { new RiskFactor { Source = "port", Points = 25, Reason = "Port 445 (SMB) : faille connue (ex. WannaCry)" } },
                },
                AiSummary =
                    "Poste Windows avec le partage de fichiers (SMB, port 445) exposé. Aucune version n'a pu être lue, " +
                    "donc aucune CVE n'a été recherchée : le risque réel est inconnu. Priorité : vérifier que les mises à jour " +
                    "Windows sont appliquées (MS17-010, faille utilisée par WannaCry) et limiter l'accès au port 445 par le pare-feu.",
            },

            // --- Metasploitable 2 (machine volontairement vulnérable) -------------------
            new Host
            {
                IpAddress = "192.168.56.101",
                Hostname = "metasploitable",
                MacAddress = "08:00:27:3A:5B:01",
                Vendor = "PCS Systemtechnik (VirtualBox)",
                Ttl = 64,
                Services =
                {
                    new Service
                    {
                        Port = 21, Name = "ftp", RawBanner = "220 (vsFTPd 2.3.4)",
                        VersionState = InfoState.Identified, Product = "vsftpd", Version = "2.3.4",
                        Vulnerabilities =
                        {
                            new Vulnerability
                            {
                                CveId = "CVE-2011-2523", CvssScore = 9.8, Severity = "CRITICAL",
                                Description = "Porte dérobée ajoutée à l'archive de vsftpd 2.3.4 : un nom d'utilisateur contenant « :) » ouvre un shell sur le port 6200.",
                            },
                        },
                    },
                    new Service
                    {
                        Port = 22, Name = "ssh", RawBanner = "SSH-2.0-OpenSSH_4.7p1 Debian-8ubuntu1",
                        VersionState = InfoState.Identified, Product = "OpenSSH", Version = "4.7p1",
                        Vulnerabilities =
                        {
                            new Vulnerability
                            {
                                CveId = "CVE-2010-4478", CvssScore = 7.5, Severity = "HIGH",
                                Description = "L'implémentation J-PAKE d'OpenSSH avant 5.7 ne vérifie pas certains paramètres, ce qui peut permettre de contourner l'authentification.",
                            },
                        },
                    },
                    new Service { Port = 23, Name = "telnet", RawBanner = "metasploitable login: " },
                    new Service
                    {
                        Port = 80, Name = "http", RawBanner = "Apache/2.2.8 (Ubuntu) DAV/2",
                        VersionState = InfoState.Identified, Product = "Apache httpd", Version = "2.2.8",
                        Vulnerabilities =
                        {
                            new Vulnerability
                            {
                                CveId = "CVE-2011-3192", CvssScore = 7.8, Severity = "HIGH",
                                Description = "« Apache Killer » : un en-tête Range forgé épuise la mémoire du serveur (déni de service).",
                            },
                        },
                    },
                    new Service { Port = 445, Name = "smb" },
                    new Service
                    {
                        Port = 3306, Name = "mysql", RawBanner = "5.0.51a-3ubuntu5",
                        VersionState = InfoState.Identified, Product = "MySQL", Version = "5.0.51a",
                    },
                },
                Os = new OsGuess
                {
                    State = InfoState.Identified, Name = "Linux (Ubuntu)", Confidence = 90,
                    Clues = { "TTL 64", "bannière SSH « Debian-8ubuntu1 »", "bannière HTTP « (Ubuntu) »" },
                },
                Risk = new RiskAssessment
                {
                    Score = 185, Level = RiskLevel.Critical,
                    Factors =
                    {
                        new RiskFactor { Source = "port", Points = 20, Reason = "Port 21 (FTP) : transfert non chiffré" },
                        new RiskFactor { Source = "port", Points = 0, Reason = "Port 22 (SSH) : protocole sécurisé" },
                        new RiskFactor { Source = "port", Points = 30, Reason = "Port 23 (Telnet) : mots de passe envoyés en clair" },
                        new RiskFactor { Source = "port", Points = 5, Reason = "Port 80 (HTTP) : trafic web non chiffré" },
                        new RiskFactor { Source = "port", Points = 25, Reason = "Port 445 (SMB) : faille connue (ex. WannaCry)" },
                        new RiskFactor { Source = "port", Points = 15, Reason = "Port 3306 (MySQL) : base de données exposée sur le réseau" },
                        new RiskFactor { Source = "cve", Points = 40, Reason = "CVE-2011-2523 (critique) sur vsftpd 2.3.4" },
                        new RiskFactor { Source = "cve", Points = 25, Reason = "CVE-2010-4478 (élevée) sur OpenSSH 4.7p1" },
                        new RiskFactor { Source = "cve", Points = 25, Reason = "CVE-2011-3192 (élevée) sur Apache httpd 2.2.8" },
                    },
                },
                AiSummary =
                    "Machine Linux (Ubuntu) très exposée : six services ouverts, dont plusieurs non chiffrés (FTP, Telnet, HTTP). " +
                    "Le plus urgent est le serveur FTP vsftpd 2.3.4, touché par CVE-2011-2523 (porte dérobée, gravité critique) : " +
                    "désactiver ce service ou installer une version saine. Ensuite : fermer Telnet au profit de SSH, mettre à jour " +
                    "OpenSSH 4.7p1 et Apache 2.2.8, et ne plus exposer MySQL (port 3306) sur le réseau.",
            },

            // --- Serveur web Ubuntu, plutôt bien configuré ------------------------------
            new Host
            {
                IpAddress = "192.168.56.102",
                Hostname = "srv-web",
                MacAddress = "08:00:27:3A:5B:02",
                Vendor = "PCS Systemtechnik (VirtualBox)",
                Ttl = 64,
                Services =
                {
                    new Service
                    {
                        Port = 22, Name = "ssh", RawBanner = "SSH-2.0-OpenSSH_8.9p1 Ubuntu-3ubuntu0.10",
                        VersionState = InfoState.Identified, Product = "OpenSSH", Version = "8.9p1",
                        Vulnerabilities =
                        {
                            new Vulnerability
                            {
                                CveId = "CVE-2024-6387", CvssScore = 8.1, Severity = "HIGH",
                                Description = "« regreSSHion » : condition de concurrence dans le gestionnaire de signaux de sshd, pouvant permettre une exécution de code à distance sans authentification.",
                            },
                        },
                    },
                    new Service { Port = 80, Name = "http", RawBanner = "nginx", VersionState = InfoState.Hidden, Product = "nginx" },
                    new Service { Port = 443, Name = "https", RawBanner = "nginx", VersionState = InfoState.Hidden, Product = "nginx" },
                },
                Os = new OsGuess
                {
                    State = InfoState.Identified, Name = "Linux (Ubuntu)", Confidence = 85,
                    Clues = { "TTL 64", "bannière SSH « Ubuntu-3ubuntu0.10 »" },
                },
                Risk = new RiskAssessment
                {
                    Score = 30, Level = RiskLevel.Medium,
                    Factors =
                    {
                        new RiskFactor { Source = "port", Points = 0, Reason = "Port 22 (SSH) : protocole sécurisé" },
                        new RiskFactor { Source = "port", Points = 5, Reason = "Port 80 (HTTP) : trafic web non chiffré" },
                        new RiskFactor { Source = "port", Points = 0, Reason = "Port 443 (HTTPS) : trafic chiffré" },
                        new RiskFactor { Source = "cve", Points = 25, Reason = "CVE-2024-6387 (élevée) sur OpenSSH 8.9p1" },
                    },
                },
                AiSummary =
                    "Serveur web Linux (Ubuntu) plutôt bien configuré : nginx masque sa version, ce qui est une bonne pratique, " +
                    "mais ne prouve pas qu'il est à jour. OpenSSH 8.9p1 est potentiellement concerné par CVE-2024-6387 (regreSSHion). " +
                    "Attention, Ubuntu corrige souvent une faille sans changer le numéro de version : le suffixe « 3ubuntu0.10 » peut " +
                    "déjà inclure le correctif. À vérifier sur le serveur avec « apt list --installed openssh-server ».",
            },

            // --- Imprimante -------------------------------------------------------------
            new Host
            {
                IpAddress = "192.168.56.103",
                MacAddress = "3C:D9:2B:10:20:30",
                Vendor = "Hewlett Packard",
                Ttl = 255,
                Services =
                {
                    new Service { Port = 80, Name = "http", RawBanner = "HP HTTP Server; HP LaserJet Pro", VersionState = InfoState.Hidden, Product = "HP HTTP Server" },
                    new Service { Port = 631, Name = "ipp" },
                    new Service { Port = 9100, Name = "jetdirect" },
                },
                Os = new OsGuess
                {
                    State = InfoState.Identified, Name = "Imprimante (HP)", Confidence = 70,
                    Clues = { "TTL 255", "fabricant MAC Hewlett Packard", "port 9100 (impression)" },
                },
                Risk = new RiskAssessment
                {
                    Score = 5, Level = RiskLevel.Low,
                    Factors = { new RiskFactor { Source = "port", Points = 5, Reason = "Port 80 (HTTP) : trafic web non chiffré" } },
                },
                AiSummary =
                    "Imprimante HP. Aucun service à risque connu, mais l'interface web (port 80) n'est pas chiffrée et les imprimantes " +
                    "sont rarement mises à jour : vérifier le firmware et changer le mot de passe administrateur par défaut.",
            },
        },
    };
}

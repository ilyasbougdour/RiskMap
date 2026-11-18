using System.Net;
using System.Text;
using RiskMap.Analysis;
using RiskMap.Core.Fakes;
using RiskMap.Core.Models;
using RiskMap.Core.Pipeline;
using RiskMap.Core.Storage;
using RiskMap.Discovery;
using RiskMap.Scanner;

// Outil en ligne de commande : chacun teste SA partie sans attendre les autres.
// Toutes les commandes écrivent un ScanResult en JSON (à l'écran, ou dans un fichier avec --out).

Console.OutputEncoding = Encoding.UTF8;

if (args.Length == 0 || args[0] is "-h" or "--help" or "help")
{
    PrintHelp();
    return 0;
}

var options = Options.Parse(args.Skip(1).ToArray());
var progress = new ConsoleProgress();
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

try
{
    ScanResult result;
    switch (args[0])
    {
        case "fixtures":
            WriteFixtures(options.Positional ?? "fixtures");
            return 0;

        case "discover": // Découverte (Aida)
        {
            var target = options.Positional ?? throw new ArgumentException("Indiquez un réseau, ex. 192.168.56.0/24");
            result = new ScanResult { Target = target, StartedAt = DateTime.Now };
            result.Hosts = await new PingSweepDiscovery().DiscoverAsync(target, progress, cts.Token);
            break;
        }

        case "scan": // Scanner (Ilyass) : une adresse IP, ou un fichier JSON de machines
        {
            result = LoadOrSingleHost(options.Positional ?? Path.Combine("fixtures", "1-discovered.json"));
            var scanner = new TcpPortScanner();
            var os = new OsDetector();
            foreach (var host in result.Hosts)
            {
                Console.Error.WriteLine($"  Scan de {host.IpAddress}...");
                await scanner.ScanAsync(host, cts.Token);
                os.Detect(host);
            }
            break;
        }

        case "analyze": // Analyse & IA : part d'un fichier de machines déjà scannées
        {
            result = JsonStore.Load<ScanResult>(options.Positional ?? Path.Combine("fixtures", "2-scanned.json"));
            var analyzer = HostAnalyzer.CreateDefault();
            foreach (var host in result.Hosts)
            {
                Console.Error.WriteLine($"  Analyse de {host.IpAddress}...");
                await analyzer.AnalyzeAsync(host, cts.Token);
            }
            break;
        }

        case "run": // Tout le pipeline. Par défaut tout est en démo ; --real choisit les vrais modules.
        {
            var target = options.Positional ?? FakeData.Target;
            Console.Error.WriteLine($"  Modules réels : {(options.Real.Count == 0 ? "aucun (démo)" : string.Join(", ", options.Real))}");
            var stages = Stages.Create(
                realDiscovery: options.Real.Contains("discovery"),
                realScanner: options.Real.Contains("scanner"),
                realAnalysis: options.Real.Contains("analysis"));
            result = await new ScanPipeline(stages).RunAsync(target, progress, cts.Token);
            break;
        }

        default:
            Console.Error.WriteLine($"Commande inconnue : {args[0]}");
            PrintHelp();
            return 1;
    }

    result.FinishedAt ??= DateTime.Now;
    if (options.Out != null)
    {
        JsonStore.Save(options.Out, result);
        Console.Error.WriteLine($"  Résultat enregistré dans {options.Out}");
    }
    else
    {
        Console.WriteLine(JsonStore.Serialize(result));
    }
    return 0;
}
catch (NotImplementedException ex)
{
    Console.Error.WriteLine($"Pas encore écrit : {ex.Message}");
    return 2;
}
catch (Exception ex) when (ex is ArgumentException or FileNotFoundException or InvalidDataException)
{
    Console.Error.WriteLine($"Erreur : {ex.Message}");
    return 1;
}

static ScanResult LoadOrSingleHost(string input)
{
    if (IPAddress.TryParse(input, out _))
        return new ScanResult { Target = input, StartedAt = DateTime.Now, Hosts = { new Host { IpAddress = input } } };
    return JsonStore.Load<ScanResult>(input);
}

static void WriteFixtures(string dir)
{
    var analyzed = FakeData.Analyzed();
    var discovered = new ScanResult { Target = analyzed.Target, StartedAt = analyzed.StartedAt, FinishedAt = analyzed.FinishedAt, Hosts = FakeData.Discovered() };
    var scanned = new ScanResult { Target = analyzed.Target, StartedAt = analyzed.StartedAt, FinishedAt = analyzed.FinishedAt, Hosts = FakeData.Scanned() };

    JsonStore.Save(Path.Combine(dir, "1-discovered.json"), discovered);
    JsonStore.Save(Path.Combine(dir, "2-scanned.json"), scanned);
    JsonStore.Save(Path.Combine(dir, "3-analyzed.json"), analyzed);
    Console.Error.WriteLine($"  Données d'exemple écrites dans {Path.GetFullPath(dir)}");
}

static void PrintHelp() => Console.WriteLine("""
    RiskMap — outil en ligne de commande

      discover <réseau>          Découverte seule (ex. 192.168.56.0/24)
      scan [ip | fichier.json]   Scanner seul. Par défaut : fixtures/1-discovered.json
      analyze [fichier.json]     Analyse seule. Par défaut : fixtures/2-scanned.json
      run [réseau] [--real discovery,scanner,analysis]
                                 Pipeline complet. Sans --real, tout est en démo.
      fixtures [dossier]         Régénère les données d'exemple (par défaut : fixtures/)

    Option commune : --out fichier.json   enregistre le résultat au lieu de l'afficher.

    Exemple : dotnet run --project src/RiskMap.Cli -- run --real scanner --out resultat.json
    """);

/// <summary>Affiche la progression immédiatement, dans l'ordre (Progress<T> l'afficherait en différé).</summary>
class ConsoleProgress : IProgress<string>
{
    public void Report(string value) => Console.Error.WriteLine($"  {value}");
}

class Options
{
    public string? Positional { get; private set; }
    public string? Out { get; private set; }
    public HashSet<string> Real { get; } = new(StringComparer.OrdinalIgnoreCase);

    public static Options Parse(string[] args)
    {
        var o = new Options();
        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--out" when i + 1 < args.Length:
                    o.Out = args[++i];
                    break;
                case "--real" when i + 1 < args.Length:
                    foreach (var s in args[++i].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                        o.Real.Add(s);
                    break;
                default:
                    o.Positional ??= args[i];
                    break;
            }
        }
        return o;
    }
}

using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Core.Pipeline;

/// <summary>Les quatre étapes à brancher. Vraies ou fausses implémentations, au choix.</summary>
public record PipelineStages(
    IHostDiscovery Discovery,
    IPortScanner Scanner,
    IOsDetector OsDetector,
    IHostAnalyzer Analyzer);

/// <summary>
/// Enchaîne les étapes : découverte, scan des ports, détection du système, analyse.
/// Responsable : module Scanner / intégration.
/// </summary>
public class ScanPipeline
{
    private readonly PipelineStages _stages;

    /// <summary>Nombre de machines scannées en même temps.</summary>
    public int MaxParallelHosts { get; set; } = 16;

    public ScanPipeline(PipelineStages stages) => _stages = stages;

    public async Task<ScanResult> RunAsync(string target, IProgress<string>? progress = null, CancellationToken ct = default)
    {
        var result = new ScanResult { Target = target, StartedAt = DateTime.Now };

        progress?.Report($"Découverte des machines sur {target}...");
        result.Hosts = await _stages.Discovery.DiscoverAsync(target, progress, ct);
        progress?.Report($"{result.Hosts.Count} machine(s) trouvée(s).");

        progress?.Report("Scan des ports...");
        using (var limit = new SemaphoreSlim(MaxParallelHosts))
        {
            await Task.WhenAll(result.Hosts.Select(async host =>
            {
                await limit.WaitAsync(ct);
                try
                {
                    await _stages.Scanner.ScanAsync(host, ct);
                    _stages.OsDetector.Detect(host);
                    progress?.Report($"{host.IpAddress} : {host.Services.Count} port(s) ouvert(s).");
                }
                finally
                {
                    limit.Release();
                }
            }));
        }

        // L'analyse est faite machine par machine : la base NVD limite le nombre de requêtes.
        foreach (var host in result.Hosts)
        {
            progress?.Report($"Analyse de {host.IpAddress}...");
            await _stages.Analyzer.AnalyzeAsync(host, ct);
        }

        result.FinishedAt = DateTime.Now;
        progress?.Report("Scan terminé.");
        return result;
    }
}

using RiskMap.Analysis.Ai;
using RiskMap.Analysis.Cve;
using RiskMap.Analysis.Scoring;
using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Analysis;

/// <summary>
/// Module Analyse & IA : point d'entrée appelé par le pipeline pour chaque machine.
/// Enchaîne : bannière → produit/version → CVE → score → synthèse IA.
/// </summary>
public class HostAnalyzer : IHostAnalyzer
{
    private readonly BannerParser _parser;
    private readonly IVulnerabilitySource _vulnerabilities;
    private readonly RiskScorer _scorer;
    private readonly ISummaryProvider? _summary;

    public HostAnalyzer(BannerParser parser, IVulnerabilitySource vulnerabilities, RiskScorer scorer, ISummaryProvider? summary)
    {
        _parser = parser;
        _vulnerabilities = vulnerabilities;
        _scorer = scorer;
        _summary = summary;
    }

    /// <summary>Configuration par défaut : NVD avec cache local, règles du fichier rules/port-rules.json, IA locale puis API.</summary>
    public static HostAnalyzer CreateDefault()
    {
        var rules = RiskRules.Load(Path.Combine(AppContext.BaseDirectory, "rules", "port-rules.json"));
        var cacheDir = Path.Combine(AppContext.BaseDirectory, "cache", "nvd");
        return new HostAnalyzer(
            new BannerParser(),
            new CveCache(new NvdClient(), cacheDir),
            new RiskScorer(rules),
            new FallbackSummaryProvider(new OllamaSummaryProvider(), new ApiSummaryProvider()));
    }

    public async Task AnalyzeAsync(Host host, CancellationToken ct)
    {
        foreach (var service in host.Services)
        {
            _parser.Identify(service);

            if (service.VersionState == InfoState.Identified && service.Product != null && service.Version != null)
                service.Vulnerabilities = await _vulnerabilities.FindAsync(service.Product, service.Version, ct);
        }

        host.Risk = _scorer.Score(host);

        if (_summary == null) return;
        try
        {
            host.AiSummary = await _summary.SummarizeAsync(host, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Une IA indisponible ne doit pas faire échouer tout le scan.
            host.AiSummary = $"Synthèse IA indisponible : {ex.Message}";
        }
    }
}

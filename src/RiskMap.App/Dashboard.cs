using System.Windows.Media;
using RiskMap.Core.Models;

namespace RiskMap.App;

/// <summary>
/// Application — responsable : Ilyass.
/// Chiffres du tableau de bord, calculés à partir d'un résultat de scan. L'onglet « Tableau de bord » s'y lie.
/// </summary>
public class DashboardSummary
{
    public string Target { get; init; } = "";
    public string Period { get; init; } = "";

    public int HostCount { get; init; }
    public int OpenPortCount { get; init; }
    public int CveCount { get; init; }
    public int CriticalCveCount { get; init; }

    /// <summary>Une tuile par niveau de risque (Critique, Élevé, Moyen, Faible, Non analysé).</summary>
    public List<LevelTile> Levels { get; init; } = new();

    /// <summary>Les machines les plus à risque, de la plus à la moins risquée.</summary>
    public List<Host> TopHosts { get; init; } = new();

    /// <summary>Les ports les plus souvent ouverts sur le réseau.</summary>
    public List<PortCount> TopPorts { get; init; } = new();

    /// <summary>Versions masquées et non déterminées : le risque réel peut être plus élevé qu'affiché.</summary>
    public int UnknownVersionCount { get; init; }

    public static DashboardSummary From(ScanResult result)
    {
        var services = result.Hosts.SelectMany(h => h.Services).ToList();
        var cves = services.SelectMany(s => s.Vulnerabilities).ToList();

        var levels = new RiskLevel?[] { RiskLevel.Critical, RiskLevel.High, RiskLevel.Medium, RiskLevel.Low, null }
            .Select(level => new LevelTile(
                RiskDisplay.TextFor(level),
                result.Hosts.Count(h => h.Risk?.Level == level),
                RiskDisplay.BrushFor(level)))
            .Where(t => t.Count > 0 || t.Label != RiskDisplay.TextFor(null)) // « Non analysé » seulement s'il y en a
            .ToList();

        return new DashboardSummary
        {
            Target = result.Target,
            Period = result.FinishedAt is DateTime end
                ? $"{result.StartedAt:dd/MM/yyyy HH:mm} — durée {(end - result.StartedAt).TotalSeconds:0} s"
                : $"{result.StartedAt:dd/MM/yyyy HH:mm}",
            HostCount = result.Hosts.Count,
            OpenPortCount = services.Count,
            CveCount = cves.Select(v => v.CveId).Distinct().Count(),
            CriticalCveCount = cves.Where(v => v.Severity == "CRITICAL").Select(v => v.CveId).Distinct().Count(),
            Levels = levels,
            TopHosts = result.Hosts
                .Where(h => h.Risk != null)
                .OrderByDescending(h => h.Risk!.Score)
                .Take(5)
                .ToList(),
            TopPorts = services
                .GroupBy(s => (s.Port, s.Name))
                .Select(g => new PortCount($"{g.Key.Port} ({g.Key.Name ?? "?"})", g.Count()))
                .OrderByDescending(p => p.Count).ThenBy(p => p.Label)
                .Take(6)
                .ToList(),
            UnknownVersionCount = services.Count(s => s.VersionState != InfoState.Identified),
        };
    }
}

public record LevelTile(string Label, int Count, Brush Color);

public record PortCount(string Label, int Count);

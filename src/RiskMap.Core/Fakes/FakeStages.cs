using RiskMap.Core.Contracts;
using RiskMap.Core.Models;

namespace RiskMap.Core.Fakes;

// Fausses implémentations : elles renvoient les données de FakeData.
// Elles servent à faire tourner l'application complète avant que les vrais modules soient prêts.

public class FakeHostDiscovery : IHostDiscovery
{
    public async Task<List<Host>> DiscoverAsync(string target, IProgress<string>? progress, CancellationToken ct)
    {
        await Task.Delay(300, ct); // simule un peu d'attente pour voir la progression dans l'interface
        return FakeData.Discovered();
    }
}

public class FakePortScanner : IPortScanner
{
    public async Task ScanAsync(Host host, CancellationToken ct)
    {
        await Task.Delay(200, ct);
        var sample = FakeData.Scanned().FirstOrDefault(h => h.IpAddress == host.IpAddress);
        host.Services = sample?.Services ?? new();
    }
}

public class FakeOsDetector : IOsDetector
{
    public void Detect(Host host)
    {
        var sample = FakeData.Scanned().FirstOrDefault(h => h.IpAddress == host.IpAddress);
        host.Os = sample?.Os ?? new OsGuess();
    }
}

public class FakeHostAnalyzer : IHostAnalyzer
{
    public async Task AnalyzeAsync(Host host, CancellationToken ct)
    {
        await Task.Delay(200, ct);
        var sample = FakeData.Analyzed().Hosts.FirstOrDefault(h => h.IpAddress == host.IpAddress);
        if (sample == null) return;

        foreach (var service in host.Services)
        {
            var s = sample.Services.FirstOrDefault(x => x.Port == service.Port);
            if (s == null) continue;
            service.VersionState = s.VersionState;
            service.Product = s.Product;
            service.Version = s.Version;
            service.Vulnerabilities = s.Vulnerabilities;
        }
        host.Risk = sample.Risk;
        host.AiSummary = sample.AiSummary;
    }
}

using RiskMap.Analysis;
using RiskMap.Core.Fakes;
using RiskMap.Core.Pipeline;
using RiskMap.Discovery;
using RiskMap.Scanner;

namespace RiskMap.App;

/// <summary>Choisit, pour chaque étape, la vraie implémentation ou la fausse (démo).</summary>
static class Stages
{
    public static PipelineStages Create(bool realDiscovery, bool realScanner, bool realAnalysis) => new(
        realDiscovery ? new PingSweepDiscovery() : new FakeHostDiscovery(),
        realScanner ? new TcpPortScanner() : new FakePortScanner(),
        realScanner ? new OsDetector() : new FakeOsDetector(),
        realAnalysis ? HostAnalyzer.CreateDefault() : new FakeHostAnalyzer());
}

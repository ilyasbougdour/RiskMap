using RiskMap.Core.Models;

namespace RiskMap.Analysis.Ai;

/// <summary>Rédige la synthèse d'une machine. Deux implémentations : modèle local ou API externe.</summary>
public interface ISummaryProvider
{
    /// <summary>Nom affiché dans l'interface, ex. « Local (Ollama) ».</summary>
    string Name { get; }

    /// <summary>true si les données du scan quittent l'ordinateur (l'interface doit le signaler).</summary>
    bool SendsDataOutside { get; }

    Task<string> SummarizeAsync(Host host, CancellationToken ct);
}

/// <summary>Modèle local via Ollama (https://ollama.com) : les données ne quittent pas la machine.</summary>
public class OllamaSummaryProvider : ISummaryProvider
{
    public string Name => "Local (Ollama)";
    public bool SendsDataOutside => false;

    public string Endpoint { get; set; } = "http://localhost:11434";
    public string Model { get; set; } = "llama3.2";

    public Task<string> SummarizeAsync(Host host, CancellationToken ct)
    {
        // TODO : POST {Endpoint}/api/generate avec { model, prompt = PromptBuilder.Build(host), stream = false },
        // puis lire le champ « response ». Mesurer le temps de réponse sur la machine de démo dès la semaine 1.
        throw new NotImplementedException("Modèle local pas encore branché (OllamaSummaryProvider).");
    }
}

/// <summary>Solution de secours : un modèle en ligne, si la machine est trop limitée pour le modèle local.</summary>
public class ApiSummaryProvider : ISummaryProvider
{
    public string Name => "API externe";
    public bool SendsDataOutside => true;

    /// <summary>Lue dans la variable d'environnement RISKMAP_API_KEY. Ne jamais l'écrire dans le code ni la pousser sur GitHub.</summary>
    public string? ApiKey { get; set; } = Environment.GetEnvironmentVariable("RISKMAP_API_KEY");

    public Task<string> SummarizeAsync(Host host, CancellationToken ct)
    {
        // TODO : appeler l'API choisie avec PromptBuilder.Build(host).
        throw new NotImplementedException("API externe pas encore branchée (ApiSummaryProvider).");
    }
}

/// <summary>Essaie le modèle local, et passe à l'API s'il ne répond pas.</summary>
public class FallbackSummaryProvider : ISummaryProvider
{
    private readonly ISummaryProvider _primary;
    private readonly ISummaryProvider _fallback;

    public FallbackSummaryProvider(ISummaryProvider primary, ISummaryProvider fallback)
    {
        _primary = primary;
        _fallback = fallback;
    }

    public string Name => $"{_primary.Name}, sinon {_fallback.Name}";
    public bool SendsDataOutside => _fallback.SendsDataOutside;

    public async Task<string> SummarizeAsync(Host host, CancellationToken ct)
    {
        try
        {
            return await _primary.SummarizeAsync(host, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return await _fallback.SummarizeAsync(host, ct);
        }
    }
}

/// <summary>Construit le texte envoyé à l'IA.</summary>
public static class PromptBuilder
{
    public static string Build(Host host)
    {
        // TODO : envoyer les résultats en JSON (JsonStore.Serialize(host)) avec ces consignes :
        //   - répondre en français, en quelques phrases, pour quelqu'un qui n'est pas expert ;
        //   - ne citer QUE les CVE présentes dans les données, ne jamais en inventer ;
        //   - « Hidden » = bonne pratique à signaler, mais le risque reste inconnu, pas faible ;
        //   - « Undetermined » = limite de l'outil, ne rien en conclure sur la machine ;
        //   - parler de « potentiellement vulnérable » (un système peut corriger sans changer de version) ;
        //   - finir par les correctifs, dans l'ordre de priorité.
        throw new NotImplementedException();
    }
}

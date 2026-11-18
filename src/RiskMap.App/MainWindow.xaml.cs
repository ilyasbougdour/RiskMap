using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RiskMap.Core.Models;
using RiskMap.Core.Pipeline;
using RiskMap.Core.Storage;

namespace RiskMap.App;

/// <summary>
/// Fenêtre principale — responsable : Ilyass.
/// Liste des machines à gauche ; carte et détail à droite.
/// </summary>
public partial class MainWindow : Window
{
    private ScanResult? _result;
    private CancellationTokenSource? _cts;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Scan_Click(object sender, RoutedEventArgs e)
    {
        var target = TargetBox.Text.Trim();
        if (!ScanTarget.TryValidate(target, out var error))
        {
            StatusText.Text = error;
            TargetBox.Focus();
            return;
        }

        bool realDiscovery = RealDiscovery.IsChecked == true, realScanner = RealScanner.IsChecked == true;

        // Cadre légal : un vrai scan envoie des paquets sur le réseau. On demande une confirmation explicite.
        if (realDiscovery || realScanner)
        {
            var answer = MessageBox.Show(this,
                $"Ce scan va réellement interroger les machines de {target}.\n\n" +
                "Confirmez-vous que ce réseau vous appartient ou que vous avez l'autorisation de le scanner ?\n" +
                "(Laboratoire isolé uniquement : jamais le réseau de l'école ni un réseau public.)",
                "Autorisation de scan", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (answer != MessageBoxResult.Yes)
            {
                StatusText.Text = "Scan annulé : autorisation non confirmée.";
                return;
            }
        }

        var stages = Stages.Create(realDiscovery, realScanner, RealAnalysis.IsChecked == true);

        _cts = new CancellationTokenSource();
        SetBusy(true);
        try
        {
            var progress = new Progress<string>(m => StatusText.Text = m); // revient sur le thread de l'interface
            var result = await new ScanPipeline(stages).RunAsync(target, progress, _cts.Token);
            ShowResult(result);
        }
        catch (OperationCanceledException)
        {
            StatusText.Text = "Scan arrêté.";
        }
        catch (NotImplementedException ex)
        {
            StatusText.Text = $"Module pas encore écrit : {ex.Message} Décochez-le pour utiliser les données d'exemple.";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Erreur : {ex.Message}";
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e) => _cts?.Cancel();

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog { Filter = "Résultat RiskMap (*.json)|*.json" };
        if (dialog.ShowDialog(this) != true) return;
        try
        {
            ShowResult(JsonStore.Load<ScanResult>(dialog.FileName));
            StatusText.Text = $"Ouvert : {dialog.FileName}";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Impossible d'ouvrir le fichier : {ex.Message}";
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (_result == null)
        {
            StatusText.Text = "Rien à enregistrer : lancez d'abord un scan.";
            return;
        }
        var dialog = new SaveFileDialog { Filter = "Résultat RiskMap (*.json)|*.json", FileName = $"riskmap-{DateTime.Now:yyyy-MM-dd-HHmm}.json" };
        if (dialog.ShowDialog(this) != true) return;
        JsonStore.Save(dialog.FileName, _result);
        StatusText.Text = $"Enregistré : {dialog.FileName}";
    }

    private void ShowResult(ScanResult result)
    {
        _result = result;
        // Les machines les plus à risque en haut ; à score égal, par adresse.
        HostsGrid.ItemsSource = result.Hosts
            .OrderByDescending(h => h.Risk?.Score ?? -1)
            .ThenBy(h => h.IpAddress, IpComparer.Instance)
            .ToList();
        Map.ShowResult(result);
        ShowDashboard(result);
        ShowDetail(null);
        Tabs.SelectedItem = DashboardTab;
    }

    private void ShowDashboard(ScanResult? result)
    {
        DashboardContent.DataContext = result == null ? null : DashboardSummary.From(result);
        NoResultText.Visibility = result == null ? Visibility.Visible : Visibility.Collapsed;
        DashboardContent.Visibility = result == null ? Visibility.Collapsed : Visibility.Visible;
    }

    private void TopHost_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.Tag is Host host)
            Map_HostSelected(this, host); // même comportement qu'un clic sur la carte : sélection + onglet détail
    }

    private void HostsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var host = HostsGrid.SelectedItem as Host;
        ShowDetail(host);
        Map.Highlight(host);
    }

    private void Map_HostSelected(object? sender, Host host)
    {
        HostsGrid.SelectedItem = host; // déclenche HostsGrid_SelectionChanged
        Tabs.SelectedItem = DetailTab;
    }

    private void ShowDetail(Host? host)
    {
        DetailPanel.DataContext = host;
        NoSelectionText.Visibility = host == null ? Visibility.Visible : Visibility.Collapsed;
        DetailContent.Visibility = host == null ? Visibility.Collapsed : Visibility.Visible;

        // Toutes les CVE de la machine dans un seul tableau, avec le logiciel concerné.
        CveGrid.ItemsSource = host?.Services
            .SelectMany(s => s.Vulnerabilities.Select(v => new
            {
                v.CveId,
                v.CvssScore,
                v.Severity,
                Software = $"{s.Product} {s.Version} (port {s.Port})",
                v.Description,
            }))
            .OrderByDescending(v => v.CvssScore)
            .ToList();
    }

    private void SetBusy(bool busy)
    {
        ScanButton.IsEnabled = !busy;
        CancelButton.IsEnabled = busy;
        BusyBar.Visibility = busy ? Visibility.Visible : Visibility.Collapsed;
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using RiskMap.Core.Models;

namespace RiskMap.App;

/// <summary>
/// Carte du réseau — responsable : Aida.
/// Version de départ : le réseau au centre, les machines en cercle autour, une couleur par niveau de risque.
/// Idées d'amélioration : regrouper par type d'appareil, icônes par système, légende, zoom, infobulles...
/// Pour travailler dessus sans scanner : bouton « Ouvrir » → fixtures/3-analyzed.json.
/// </summary>
public partial class NetworkMapControl : UserControl
{
    private const double NodeSize = 46;
    private const double CenterSize = 84;

    private ScanResult? _result;
    private Host? _selected;

    /// <summary>Déclenché quand on clique sur une machine.</summary>
    public event EventHandler<Host>? HostSelected;

    public NetworkMapControl()
    {
        InitializeComponent();
        SizeChanged += (_, _) => Redraw();
    }

    public void ShowResult(ScanResult? result)
    {
        _result = result;
        _selected = null;
        Redraw();
    }

    public void Highlight(Host? host)
    {
        _selected = host;
        Redraw();
    }

    private void Redraw()
    {
        MapCanvas.Children.Clear();
        var hosts = _result?.Hosts ?? new List<Host>();
        EmptyText.Visibility = hosts.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        if (hosts.Count == 0 || ActualWidth < 50 || ActualHeight < 50) return;

        var center = new Point(ActualWidth / 2, ActualHeight / 2);
        var radius = Math.Max(90, Math.Min(ActualWidth, ActualHeight) / 2 - 80);

        var positions = hosts
            .Select((_, i) => 2 * Math.PI * i / hosts.Count - Math.PI / 2)
            .Select(a => new Point(center.X + radius * Math.Cos(a), center.Y + radius * Math.Sin(a)))
            .ToList();

        // Liens d'abord, pour qu'ils passent sous les cercles.
        foreach (var p in positions)
        {
            MapCanvas.Children.Add(new Line
            {
                X1 = center.X, Y1 = center.Y, X2 = p.X, Y2 = p.Y,
                Stroke = Brushes.LightGray, StrokeThickness = 2,
            });
        }

        DrawNode(center, CenterSize, new SolidColorBrush(Color.FromRgb(0x37, 0x47, 0x4F)), "Réseau", _result!.Target, null);

        for (var i = 0; i < hosts.Count; i++)
        {
            var host = hosts[i];
            var label = host.IpAddress + (host.Os.Name != null ? "\n" + host.Os.Name : "");
            DrawNode(positions[i], NodeSize, RiskDisplay.BrushFor(host.Risk?.Level), host.Risk?.Score.ToString() ?? "?", label, host);
        }
    }

    private void DrawNode(Point at, double size, Brush fill, string inner, string label, Host? host)
    {
        var isSelected = host != null && host == _selected;
        var circle = new Ellipse
        {
            Width = size, Height = size, Fill = fill,
            Stroke = isSelected ? Brushes.Black : Brushes.White,
            StrokeThickness = isSelected ? 4 : 2,
        };
        Place(circle, at.X - size / 2, at.Y - size / 2);

        var innerText = new TextBlock
        {
            Text = inner, Foreground = Brushes.White, FontWeight = FontWeights.Bold,
            Width = size, TextAlignment = TextAlignment.Center, IsHitTestVisible = false,
        };
        Place(innerText, at.X - size / 2, at.Y - 9);

        var labelText = new TextBlock
        {
            Text = label, Width = 160, TextAlignment = TextAlignment.Center,
            FontSize = 12, Foreground = Brushes.Black, IsHitTestVisible = false,
        };
        Place(labelText, at.X - 80, at.Y + size / 2 + 4);

        if (host != null)
        {
            circle.Cursor = Cursors.Hand;
            circle.ToolTip = $"{host.IpAddress} — risque {RiskDisplay.TextFor(host.Risk?.Level).ToLower()}";
            circle.MouseLeftButtonUp += (_, _) => HostSelected?.Invoke(this, host);
        }
    }

    private void Place(UIElement element, double left, double top)
    {
        Canvas.SetLeft(element, left);
        Canvas.SetTop(element, top);
        MapCanvas.Children.Add(element);
    }
}

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using RiskMap.Core.Models;

namespace RiskMap.App;

/// <summary>Couleurs et libellés communs au tableau, au détail et à la carte.</summary>
public static class RiskDisplay
{
    public static Brush BrushFor(RiskLevel? level) => level switch
    {
        RiskLevel.Low => new SolidColorBrush(Color.FromRgb(0x2E, 0x7D, 0x32)),      // vert
        RiskLevel.Medium => new SolidColorBrush(Color.FromRgb(0xC7, 0x8A, 0x00)),   // jaune foncé
        RiskLevel.High => new SolidColorBrush(Color.FromRgb(0xE6, 0x5C, 0x00)),     // orange
        RiskLevel.Critical => new SolidColorBrush(Color.FromRgb(0xC6, 0x28, 0x28)), // rouge
        _ => new SolidColorBrush(Color.FromRgb(0x75, 0x75, 0x75)),                  // gris : pas encore analysé
    };

    public static string TextFor(RiskLevel? level) => level switch
    {
        RiskLevel.Low => "Faible",
        RiskLevel.Medium => "Moyen",
        RiskLevel.High => "Élevé",
        RiskLevel.Critical => "Critique",
        _ => "Non analysé",
    };

    public static string TextFor(InfoState state) => state switch
    {
        InfoState.Identified => "Identifié",
        InfoState.Hidden => "Masqué",
        _ => "Non déterminé",
    };
}

public class RiskLevelToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        RiskDisplay.BrushFor(value as RiskLevel?);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

public class RiskLevelToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        RiskDisplay.TextFor(value as RiskLevel?);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

public class InfoStateToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is InfoState s ? RiskDisplay.TextFor(s) : "";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

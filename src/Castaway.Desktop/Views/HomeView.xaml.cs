using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Castaway.Services;

namespace Castaway.Views;

/// <summary>
/// The Home preview from the Castaway mockup. Every title here is made up; the real library
/// arrives with Castaway Server.
/// </summary>
public partial class HomeView : UserControl
{
    private static readonly CultureInfo Us = CultureInfo.GetCultureInfo("en-US");

    private sealed record SampleTitle(string Title, string Year, string Background, string Foreground, string Tag, bool Unwatched);
    private sealed record SampleProgress(string Title, string Detail, string Background, string Foreground, double Watched);

    private static readonly SampleProgress[] Continue =
    [
        new("Glass Harbor", "38 min left", "#2C2F6B", "#F2E6C9", 0.62),
        new("The Lighthouse Years", "S2 · E5 · 21 min left", "#1F3A4D", "#E9F1F5", 0.45),
        new("Low Orbit", "1 hr 4 min left", "#D9E2E8", "#1A2430", 0.28),
        new("Kitchen Confidential Club", "S1 · E3 · 9 min left", "#8A3B23", "#FBE3CF", 0.81),
    ];

    private static readonly SampleTitle[] Movies =
    [
        new("Northern Static", "2024", "#1F3A4D", "#E9F1F5", "A FILM", true),
        new("The Salt Road", "2023", "#C9A26B", "#2A1D10", "DRAMA", false),
        new("Glass Harbor", "2025", "#2C2F6B", "#F2E6C9", "THRILLER", true),
        new("Midnight Ferry", "2022", "#111418", "#F2B33D", "NOIR", true),
        new("Low Orbit", "2024", "#D9E2E8", "#1A2430", "SCI-FI", false),
        new("Copper Valley", "2021", "#8A3B23", "#FBE3CF", "WESTERN", true),
        new("Paper Lanterns", "2023", "#F0D9A8", "#5A2A1C", "FAMILY", true),
        new("The Long Quiet", "2020", "#3D4A3A", "#E7EADF", "DRAMA", false),
        new("Wild Signal", "2025", "#E8503A", "#FFF3E8", "ACTION", true),
    ];

    private static readonly SampleTitle[] Shows =
    [
        new("The Lighthouse Years", "3 seasons", "#1F3A4D", "#F2E6C9", "SERIES", false),
        new("Hollow Pines", "2 seasons", "#1C2B22", "#B9D8A8", "HORROR", true),
        new("Neon Tide", "1 season", "#5B1E5E", "#FFD0F0", "MYSTERY", true),
        new("Kitchen Confidential Club", "4 seasons", "#8A3B23", "#FBE3CF", "COMEDY", false),
        new("Atlas Falls", "2 seasons", "#2D5A7A", "#F6E7C1", "ADVENTURE", true),
        new("Moon Garden", "1 season", "#F0D9A8", "#2C2F6B", "ANIME", true),
        new("Field Notes", "5 seasons", "#3D4A3A", "#E7EADF", "NATURE", false),
        new("Night Shift", "2 seasons", "#111418", "#7FD3A0", "DRAMA", true),
        new("Sunday Drivers", "1 season", "#C9A26B", "#2A1D10", "COMEDY", true),
    ];

    public HomeView()
    {
        InitializeComponent();

        var now = DateTime.Now;
        DateText.Text = now.ToString("dddd, MMMM d", Us);
        GreetingText.Text = now.Hour switch
        {
            < 12 => "Good morning",
            < 18 => "Good afternoon",
            _ => "Good evening",
        };

        foreach (var c in Continue) ContinueRow.Children.Add(ContinueTile(c));
        foreach (var m in Movies) MoviesRow.Children.Add(PosterTile(m));
        foreach (var s in Shows) ShowsRow.Children.Add(PosterTile(s));
    }

    // ----- Poster tile: artwork, title, year -----

    private UIElement PosterTile(SampleTitle t)
    {
        var poster = new Grid { Height = 222, ClipToBounds = true };
        poster.Children.Add(new Border { Background = Hex(t.Background), CornerRadius = new CornerRadius(4) });

        var art = new DockPanel { Margin = new Thickness(13), LastChildFill = false };
        var tag = new TextBlock
        {
            Text = t.Tag, FontSize = 10, FontWeight = FontWeights.SemiBold,
            Foreground = Hex(t.Foreground), Opacity = 0.75,
        };
        DockPanel.SetDock(tag, Dock.Top);
        art.Children.Add(tag);
        var big = new TextBlock
        {
            Text = t.Title.ToUpperInvariant(), FontSize = 20, FontWeight = FontWeights.Black,
            Foreground = Hex(t.Foreground), TextWrapping = TextWrapping.Wrap, LineHeight = 21,
            LineStackingStrategy = LineStackingStrategy.BlockLineHeight,
        };
        big.SetResourceReference(TextBlock.FontFamilyProperty, "DisplayFont");
        DockPanel.SetDock(big, Dock.Bottom);
        art.Children.Add(big);
        poster.Children.Add(art);

        if (t.Unwatched)
        {
            // Unwatched marker: the accent-colored corner from the mockup.
            var corner = new Polygon
            {
                Points = new PointCollection { new Point(0, 0), new Point(22, 0), new Point(22, 22) },
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                ToolTip = "Unwatched",
            };
            corner.SetResourceReference(Shape.FillProperty, "Brush.Accent");
            poster.Children.Add(corner);
        }

        var body = new StackPanel();
        body.Children.Add(new Border
        {
            Child = poster,
            CornerRadius = new CornerRadius(4),
            Effect = new System.Windows.Media.Effects.DropShadowEffect { BlurRadius = 14, ShadowDepth = 3, Opacity = 0.3 },
        });
        body.Children.Add(Text(t.Title, FontWeights.SemiBold, false, new Thickness(0, 9, 0, 0)));
        body.Children.Add(Text(t.Year, FontWeights.Normal, true, new Thickness(0, 2, 0, 0)));

        return TileButton(body, 148, t.Title);
    }

    // ----- Continue Watching tile: landscape artwork with a progress bar -----

    private UIElement ContinueTile(SampleProgress p)
    {
        var art = new Grid { Height = 146 };
        art.Children.Add(new Border { Background = Hex(p.Background), CornerRadius = new CornerRadius(4) });
        var title = new TextBlock
        {
            Text = p.Title.ToUpperInvariant(), FontSize = 19, FontWeight = FontWeights.Black,
            Foreground = Hex(p.Foreground), TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(14, 12, 14, 18), VerticalAlignment = VerticalAlignment.Bottom,
        };
        title.SetResourceReference(TextBlock.FontFamilyProperty, "DisplayFont");
        art.Children.Add(title);

        var play = new Border
        {
            Width = 42, Height = 42, CornerRadius = new CornerRadius(21),
            Background = new SolidColorBrush(Color.FromArgb(0x99, 0, 0, 0)),
            HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
            Child = new TextBlock
            {
                Text = "", FontSize = 16, Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(3, 0, 0, 0),
            },
        };
        ((TextBlock)play.Child).SetResourceReference(TextBlock.FontFamilyProperty, "IconFont");
        art.Children.Add(play);

        // Progress bar along the bottom edge.
        var progress = new Grid { Height = 4, VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(0, 0, 0, 0) };
        progress.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(p.Watched, GridUnitType.Star) });
        progress.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1 - p.Watched, GridUnitType.Star) });
        var track = new Border { Background = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0)) };
        Grid.SetColumnSpan(track, 2);
        progress.Children.Add(track);
        var fill = new Border();
        fill.SetResourceReference(Border.BackgroundProperty, "Brush.Accent");
        progress.Children.Add(fill);
        art.Children.Add(progress);

        var body = new StackPanel();
        body.Children.Add(new Border { Child = art, CornerRadius = new CornerRadius(4), ClipToBounds = true });
        body.Children.Add(Text(p.Detail, FontWeights.Normal, true, new Thickness(0, 9, 0, 0)));

        return TileButton(body, 260, p.Title);
    }

    // ----- Helpers -----

    private Button TileButton(UIElement content, double width, string title)
    {
        var button = new Button
        {
            Content = content,
            Width = width,
            Margin = new Thickness(0, 0, 20, 0),
            Tag = "Playing movies and shows",
            ToolTip = title,
            VerticalAlignment = VerticalAlignment.Top,
        };
        button.SetResourceReference(StyleProperty, "CardButton");
        System.Windows.Automation.AutomationProperties.SetName(button, title);
        button.Click += Soon_Click;
        return button;
    }

    private static TextBlock Text(string text, FontWeight weight, bool muted, Thickness margin)
    {
        var tb = new TextBlock
        {
            Text = text, FontWeight = weight, Margin = margin,
            TextTrimming = TextTrimming.CharacterEllipsis,
            FontSize = muted ? 12.5 : 13.5,
        };
        tb.SetResourceReference(TextBlock.ForegroundProperty, muted ? "Brush.TextMuted" : "Brush.Text");
        return tb;
    }

    private static SolidColorBrush Hex(string hex) =>
        new((Color)ColorConverter.ConvertFromString(hex));

    private void Soon_Click(object sender, RoutedEventArgs e) => Toaster.Soon(sender);

    private void Server_Click(object sender, RoutedEventArgs e) => MainWindow.Instance?.GoTo("server");

    private void GoTo_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: string key }) MainWindow.Instance?.GoTo(key);
    }
}

// https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/label?view=net-maui-8.0#create-a-reusable-hyperlink-class
using CommunityToolkit.Maui;

namespace MauiTotpApp.Shared {
    public class HyperlinkSpan : Span
    {
        public static readonly BindableProperty UrlProperty =
            BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public HyperlinkSpan()
        {
            TextDecorations = TextDecorations.Underline;
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                // Launcher.OpenAsync is provided by Essentials.
                Command = new Command(async () => await Launcher.OpenAsync(Url))
            });
        }
    }
}
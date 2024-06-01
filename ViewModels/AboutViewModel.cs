namespace MauiTotpApp.ViewModels;

internal class AboutViewModel
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string Author => "Skiletro";
}

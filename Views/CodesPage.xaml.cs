using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiTotpApp.Shared;
using MauiTotpApp.ViewModels;
using SQLite;

namespace MauiTotpApp.Views;

public partial class CodesPage : ContentPage
{
    public CodesPage()
	{
        InitializeComponent();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdderPage());
    }

    private async void CopyToClipboard_Tapped(object sender, EventArgs e)
    {
        // Assuming the sender is StackLayout and it's in the context of the bound item
        if (sender is StackLayout stackLayout && stackLayout.BindingContext is TotpData item)
        {
            string service = item.Name;
            string code = item.Code.ToString();
            // Copy the 'Code' property value to clipboard
            await Clipboard.SetTextAsync(code);

            // We can also vibrate the device to let the user know further that it has copied successfully.
            // This won't do anything on Windows because there is no vibration motor, but that is okay.
#if ANDROID
            Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.25));
#endif

            // Alert the user that it has been copied succesfully.
            await Application.Current.MainPage.DisplayAlert("Copied to Clipboard", $"Copied code for \"{service}\" to the clipboard", "Okay");
        }
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        // Check if the user has scrolled to the top
        if (e.ScrollY <= 0)
        {
            // Execute the refresh command
            if (BindingContext is CodesViewModel viewModel)
            {
                viewModel.RefreshCodesButton_Command.Execute(null);
            }
        }
    }
}
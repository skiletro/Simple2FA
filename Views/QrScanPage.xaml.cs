using MauiTotpApp.Models;
using ZXing.Net.Maui;

namespace MauiTotpApp.Views;

public partial class QrScanPage : ContentPage
{
	public QrScanPage()
	{
		InitializeComponent();
		qrReader.Options = new BarcodeReaderOptions
        {
			Formats = BarcodeFormat.QrCode,
		};
        qrReader.BarcodesDetected += qrReader_BarcodesDetected;
	}

    private void qrReader_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {

        BarcodeResult? results = e.Results?.FirstOrDefault();

		if (results == null)
			return;

        Dispatcher.DispatchAsync(() =>
        {
            try
            {
                // Make a Uri, if this fails then it's not a Uri and it'll be caught by the catch
                Uri totpUri = new Uri(results.Value);

                // Checks if the Uri is of type otpauth (aka a 2FA uri)
                if (totpUri.Scheme != "otpauth")
                    return;

                Dictionary<string, string> query = totpUri.Query
                    .TrimStart('?')
                    .Split('&')
                    .Select(part => part.Split("=")) // splits up the key and value
                    .ToDictionary(split => split[0], split => split[1]);

                // If the uri doesn't contain a secret, then there is nothing we can do with it
                if (!query.ContainsKey("secret"))
                    return;

                DatabaseHelper databaseHelper = new DatabaseHelper();
                string serviceName = totpUri.LocalPath.TrimStart('/');
                string totpKey = query["secret"];

                // Unsubscribe from the event so it doesn't fire multiple times
                qrReader.BarcodesDetected -= qrReader_BarcodesDetected;

                // We can also vibrate the device to let the user know further that it has scanned successfully.
                // This won't do anything on Windows because there is no vibration motor, however this page won't even be
                // visible on Windows, so it should be fine.
                Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.25));

                // Finally add the data to the database
                databaseHelper.AddKeyToDatabase(serviceName, totpKey);

                // Alert the user
                Application.Current.MainPage.DisplayAlert("Key Successfully Added", $"New TOTP Code with name \"{serviceName}\" and key \"{totpKey}\" succesfully added.", "Okay");
                Shell.Current.Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                qrReader.BarcodesDetected -= qrReader_BarcodesDetected;
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Okay");
                Shell.Current.Navigation.PopToRootAsync();
            }
        });

    }

    private void FlipCameraButton_Clicked(object sender, EventArgs e)
    {
        FlipCamera();
    }

    private void ToggleFlashlightButton_Clicked(object sender, EventArgs e)
    {
        ToggleFlashlight();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        FlipCamera();
    }

    private void ToggleFlashlight()
    {
        qrReader.IsTorchOn = !qrReader.IsTorchOn;
    }

    private void FlipCamera()
    {
        qrReader.CameraLocation = (qrReader.CameraLocation == CameraLocation.Rear) ? CameraLocation.Front : CameraLocation.Rear;
    }
}
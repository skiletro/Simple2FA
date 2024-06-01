using MauiTotpApp.Views;
using System.Windows.Input;
using OtpNet;
using MauiTotpApp.Models;

namespace MauiTotpApp.ViewModels
{
    public class AdderViewModel
    {
        private DatabaseHelper databaseHelper;
        public string ServiceName { get; set; }
        public string TotpKey { get; set; }
        public ICommand AddNewService_Commmand { get; }
        public ICommand OpenQRScanner_Command { get; }
        public ICommand Back_Command { get; }

        public AdderViewModel()
        {
            ServiceName = "";
            TotpKey = "";
            databaseHelper = new DatabaseHelper();
            AddNewService_Commmand = new Command(AddNewService_Handler);
            OpenQRScanner_Command = new Command(OpenQRScanner_Handler);
            Back_Command = new Command(Back_Handler);
        }

        private async void AddNewService_Handler()
        {
            try
            {
                // Try and encode the key into a byte array, if this errors out then we can catch it before it gets submitted into the database
                _ = Base32Encoding.ToBytes(TotpKey);

                // Error out if the length of either is too long
                if (ServiceName.Length > 64 || TotpKey.Length > 64)
                {
                    throw new ArgumentException("Service Name or Key is greater than 128.");
                }

                // If the code gets this far, we can assume the key is fine!
                databaseHelper.AddKeyToDatabase(ServiceName, TotpKey);

                // Let the user know and move on...
                await Application.Current.MainPage.DisplayAlert("Key Successfully Added", $"New TOTP Code with name \"{ServiceName}\" and key \"{TotpKey}\" succesfully added.", "Okay");
                await Shell.Current.GoToAsync("..");
            }
            catch (ArgumentException arEx)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "The key you submitted is not valid.", "Okay");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Unknown Error Occured", ex.Message, "Okay");
            }
        }

        public async void OpenQRScanner_Handler()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new QrScanPage());
        }

        public async void Back_Handler()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}

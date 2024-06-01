using OtpNet;
using MauiTotpApp.Shared;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiTotpApp.Views;
using MauiTotpApp.Models;

namespace MauiTotpApp.ViewModels
{
    public class CodesViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<TotpData> Data { get; set; } = new ObservableCollection<TotpData>();
        
        private IDispatcherTimer timer;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private DatabaseHelper databaseHelper;
        public ICommand PageAppearing_Command { get; }
        public ICommand CopyCodeToClipboard_Command { get; }
        public ICommand AddNewCodeButton_Command { get; }
        public ICommand RefreshCodesButton_Command { get; }
        public ICommand OpenHelpButton_Command { get; }

        public CodesViewModel()
        {
            databaseHelper = new DatabaseHelper();
            databaseHelper.InitializeDatabase();
            LoadKeys();
            InitializeTimer();

            // Implement command handling logic
            PageAppearing_Command = new Command(PageAppearing_Handler);
            CopyCodeToClipboard_Command = new Command(CopyCodeToClipboard_Handler);
            AddNewCodeButton_Command = new Command(AddNewCodeButton_Handler);
            RefreshCodesButton_Command = new Command(LoadKeys);
            OpenHelpButton_Command = new Command(OpenHelpButton_Handler);
        }

        private void PageAppearing_Handler(object obj)
        {
            // Reload the keys when going back to the page (stops the user from having to manually refresh)
            LoadKeys();
        }

        private async void CopyCodeToClipboard_Handler(object obj)
        {
            if (obj is StackLayout stackLayout && stackLayout.BindingContext is TotpData item) {
                string code = item.Code.ToString();
                // Copy the 'Code' property value to clipboard
                await Clipboard.SetTextAsync(code);

                // Alert the user that it has been copied succesfully.
                SemanticScreenReader.Default.Announce($"The code {code} has been copied to the clipboard.");
                await Application.Current.MainPage.DisplayAlert("Copied to Clipboard", $"Copied 2FA Code \"{code}\" to the clipboard.", "Okay");

            }
        }

        private void AddNewCodeButton_Handler()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AdderPage());
        }

        private void OpenHelpButton_Handler()
        {
            Application.Current.MainPage.Navigation.PushAsync(new HelpPage());
        }

        private void LoadKeys()
        {
            Data.Clear();

            List<TotpKeys> databaseInformation = databaseHelper.FetchKeysFromDatabase();
            foreach (TotpKeys entry in databaseInformation)
            {
                Data.Add(new TotpData(entry.Name, new Totp(Base32Encoding.ToBytes(entry.Key))));
            }
        }

        private void InitializeTimer()
        {
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => UpdateStuffs();
            timer.Start();
        }

        private void UpdateStuffs()
        {

            for (int i = 0; i < Data.Count; i++)
            {

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Data[i].RemainingTime = Data[i].Totp.RemainingSeconds(); ;
                    Data[i].RemainingTimeNormalised = 1 - ((float)Data[i].RemainingTime / 30);

                    // Only recomputes the code if the time hit 0 and reset back to the beginning.
                    if (Data[i].RemainingTime == 30 || Data[i].Code == null)
                        Data[i].Code = Data[i].Totp.ComputeTotp();
                });
            }
        }
    }
}

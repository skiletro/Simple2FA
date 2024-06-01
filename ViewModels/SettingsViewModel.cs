using System.Windows.Input;
using System.Text;
using CommunityToolkit.Maui.Storage;
using System.Text.Json;
using MauiTotpApp.Models;

namespace MauiTotpApp.ViewModels
{
    public class SettingsViewModel
    {
        private DatabaseHelper databaseHelper = new DatabaseHelper();

        public ICommand ResetDatabase_Command { get; }
        public ICommand ImportSampleCodes_Command { get; }
        public ICommand ImportSampleCodesLarge_Command { get; }

        public ICommand ExportToJSON_Command { get; }

        public SettingsViewModel()
        {
            ResetDatabase_Command = new Command(ResetDatabase_Handler);
            ImportSampleCodes_Command = new Command(ImportSampleCodes_Handler);
            ImportSampleCodesLarge_Command = new Command(ImportSampleCodesLarge_Handler);

            ExportToJSON_Command = new Command(ExportToJSON_Handler);
        }

        private async void ResetDatabase_Handler()
        {
            bool result = await Application.Current.MainPage.DisplayAlert(
                "Caution",
                "Resetting the database is IRREVERSIBLE. Are you sure you want to proceed?",
                "Yes", "No, take me back to safety");
            if (result == true) // If the user confirms the action...
            {
                databaseHelper.ResetDatabase();
            }
        }

        private async void ImportSampleCodes_Handler(object obj)
        {
            bool result = await Application.Current.MainPage.DisplayAlert(
                "Caution",
                "This action resets the database in order to fill it with sample data. This action is IRREVERSIBLE. Are you sure you want to proceed?",
                "Yes", "No, take me back to safety");
            if (result == true) // If the user confirms the action...
            {
                // What totp codes are being used in the sample isn't important.
                databaseHelper.AddKeyToDatabase("Google", "I65VU7K5ZQL7WB4A");
                databaseHelper.AddKeyToDatabase("Facebook", "I65VU7K5ZQL7WB4B");
                databaseHelper.AddKeyToDatabase("Twitch", "I65VU7K5ZQL7WB4C");
                databaseHelper.AddKeyToDatabase("Twitter", "I65VU7K5ZQL7WB4D");
                databaseHelper.AddKeyToDatabase("Discord", "I65VU7K5ZQL7WE4E");
                databaseHelper.AddKeyToDatabase("Mastodon", "I65VU7K5ZQL7WB4F");
                databaseHelper.AddKeyToDatabase("Matrix", "I65VU7K5ZQL7WB4G");
            }
        }

        private async void ImportSampleCodesLarge_Handler(object obj)
        {
            bool result = await Application.Current.MainPage.DisplayAlert(
                "Caution",
                "This action resets the database in order to fill it with sample data: much of the sample data being repeats as to pad out the page. This action is IRREVERSIBLE. Are you sure you want to proceed?",
                "Yes", "No, take me back to safety");
            if (result == true) // If the user confirms the action...
            {
                databaseHelper.AddKeyToDatabase("Google", "I65VU7K5ZQL7WB4A");
                databaseHelper.AddKeyToDatabase("Facebook", "I65VU7K5ZQL7WB4B");
                databaseHelper.AddKeyToDatabase("Twitch", "I65VU7K5ZQL7WB4C");
                databaseHelper.AddKeyToDatabase("Twitter", "I65VU7K5ZQL7WB4D");
                databaseHelper.AddKeyToDatabase("Discord", "I65VU7K5ZQL7WE4E");
                databaseHelper.AddKeyToDatabase("Mastodon", "I65VU7K5ZQL7WB4F");
                databaseHelper.AddKeyToDatabase("Matrix", "I65VU7K5ZQL7WB4G");
                databaseHelper.AddKeyToDatabase("Google", "I65VU7K5ZQL7WB4A");
                databaseHelper.AddKeyToDatabase("Facebook", "I65VU7K5ZQL7WB4B");
                databaseHelper.AddKeyToDatabase("Twitch", "I65VU7K5ZQL7WB4C");
                databaseHelper.AddKeyToDatabase("Twitter", "I65VU7K5ZQL7WB4D");
                databaseHelper.AddKeyToDatabase("Discord", "I65VU7K5ZQL7WE4E");
                databaseHelper.AddKeyToDatabase("Mastodon", "I65VU7K5ZQL7WB4F");
                databaseHelper.AddKeyToDatabase("Matrix", "I65VU7K5ZQL7WB4G");
                databaseHelper.AddKeyToDatabase("Google", "I65VU7K5ZQL7WB4A");
                databaseHelper.AddKeyToDatabase("Facebook", "I65VU7K5ZQL7WB4B");
                databaseHelper.AddKeyToDatabase("Twitch", "I65VU7K5ZQL7WB4C");
                databaseHelper.AddKeyToDatabase("Twitter", "I65VU7K5ZQL7WB4D");
                databaseHelper.AddKeyToDatabase("Discord", "I65VU7K5ZQL7WE4E");
                databaseHelper.AddKeyToDatabase("Mastodon", "I65VU7K5ZQL7WB4F");
                databaseHelper.AddKeyToDatabase("Matrix", "I65VU7K5ZQL7WB4G");
            }
        }

        private async void ExportToJSON_Handler()
        {
            bool result = await Application.Current.MainPage.DisplayAlert(
                "Caution",
                "This action involves exporting potentially sensitive data in PLAIN TEXT. Proceeding with this action is at your own risk. Would you like to proceed?",
                "Yes", "No, take me back to safety");
            if (result == true)
            {
                JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                string jsonData = JsonSerializer.Serialize(databaseHelper.FetchKeysFromDatabase(), jsonSerializerSettings);

                using MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(jsonData));
                CancellationToken cancellationToken = new CancellationToken();
                FileSaverResult fileSaverResult = await FileSaver.Default.SaveAsync("totp_data.json", memoryStream, cancellationToken);
                if (fileSaverResult.IsSuccessful)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "The file has been saved.", "Okay");
                }
            }
        }

    }
}

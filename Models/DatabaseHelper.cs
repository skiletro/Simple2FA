using SQLite;
using MauiTotpApp.Shared;

namespace MauiTotpApp.Models
{
    internal class DatabaseHelper
    {
        private SQLiteConnection connection;

        public DatabaseHelper()
        {
            string databaseName = "totpkeys.db";
            string databaseDirectory = Path.Combine(FileSystem.Current.AppDataDirectory, databaseName);
            connection = new SQLiteConnection(databaseDirectory);
        }

        public void InitializeDatabase()
        {
            // If the table doesn't already exist, create it.
            if (!connection.GetTableInfo("TotpKeys").Any(table => table.Name == "TotpKeys"))
            {
                connection.CreateTable<TotpKeys>();
            }
        }

        public void ResetDatabase()
        {
            connection.DropTable<TotpKeys>();
            connection.CreateTable<TotpKeys>();
        }

        public void AddKeyToDatabase(string serviceName, string totpKey)
        {
            TotpKeys newKey = new TotpKeys();
            newKey.Name = serviceName;
            newKey.Key = totpKey;

            connection.Insert(newKey);
        }

        public List<TotpKeys> FetchKeysFromDatabase()
        {
            return connection.Table<TotpKeys>().ToList();
        }
    }
}

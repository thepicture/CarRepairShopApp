using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;

namespace CarRepairShopApp.ViewModel
{
    class DatabaseBackupSaver
    {
        /// <summary>
        /// Saves the .bak file to the selected filepath.
        /// </summary>
        public static void Save(string filePath)
        {
            // Reads the connection string from the config file.
            var connectionString = ConfigurationManager.ConnectionStrings["BackupString"].ConnectionString;

            // Reads the backup folder from the config file.
            var backupFolder = filePath;

            var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

            var backupFileName = string.Format("{0}{1}-{2}.bak",
                backupFolder + "\\", sqlConStrBuilder.InitialCatalog,
                DateTime.Now.ToString("yyyy-MM-dd_hh-mm"));
            new Thread(() =>
            {
                using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                {
                    var query = string.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                        sqlConStrBuilder.InitialCatalog, backupFileName);

                    using (var command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Создание резервной копии неуспешно!" +
                                "\nПожалуйста, проверьте подключение к сети Интернет.",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                }
            }).Start();
        }
    }
}

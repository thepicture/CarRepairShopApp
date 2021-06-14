using System.Windows;

namespace CarRepairShopApp.ViewModel
{
    class ReportExceptionHandler
    {
        public static void ShowErrorInformationAboutMSOffice()
        {
            MessageBox.Show("Сохранение неуспешно. Убедитесь, что в вашей системе установлен пакет Microsoft Office.",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}

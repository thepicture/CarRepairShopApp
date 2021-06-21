using System.Windows;

namespace CarRepairShopApp.ViewModel
{
    class FolderGetter
    {
        public static string GetSelectedPath()
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "Укажите папку сохранения файла:";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("Операция была отменена!",
                        "Внимание",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return null;
                }
            }
        }
    }
}

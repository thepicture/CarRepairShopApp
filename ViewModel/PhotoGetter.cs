using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarRepairShopApp.ViewModel
{
    class PhotoGetter
    {
        private static readonly OpenFileDialog dialog = new OpenFileDialog();
        public static BitmapImage Image { get; set; }
        public static byte[] ImageInBytes { get; set; }

        /// <summary>
        /// Returns a message that the operations are successfully done.
        /// </summary>
        public static void SayAllOk()
        {
            MessageBox.Show("Изображение успешно обновлено!",
                       "Успешно!",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information);
        }

        /// <summary>
        /// Attempt to open the image.
        /// </summary>
        public static bool OpenDialog()
        {
            if ((bool)dialog.ShowDialog())
            {
                ImageInBytes = File.ReadAllBytes(dialog.FileName);
                try
                {
                    Image = new BitmapImage(new Uri(dialog.FileName));
                    return true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйста, выберите изображение для прикрепления.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

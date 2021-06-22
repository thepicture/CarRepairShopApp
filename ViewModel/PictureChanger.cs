using CarRepairShopApp.Model;
using System;
using System.Windows;

namespace CarRepairShopApp.ViewModel
{
    class PictureChanger
    {
        public static void ChangePicture()
        {
            if (PhotoGetter.OpenDialog())
            {
                Manager.CurrentUser.USER_PHOTO = PhotoGetter.ImageInBytes;
                try
                {
                    Manager.Context.SaveChanges();
                    MessageBox.Show("Аватар успешно изменён!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("При прикреплении аватара произошла ошибка!" +
                        "\nПожалуйста, попробуйте прикрепить фото ещё раз.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        public static void DeletePicture()
        {
            if (MessageBox.Show("Точно удалить текущий аватар?",
               "Внимание",
               MessageBoxButton.YesNo,
               MessageBoxImage.Question)
               == MessageBoxResult.Yes)
            {
                Manager.CurrentUser.USER_PHOTO = null;
                Manager.Context.SaveChanges();
            }
        }
    }
}

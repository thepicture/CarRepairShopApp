using CarRepairShopApp.Model;
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
                MessageBox.Show("Аватар успешно изменён!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
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

using CarRepairShopApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Word = Microsoft.Office.Interop.Word;

namespace CarRepairShopApp.View
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            InitializeUserPhoto();
            UserNameBlock.Text = Manager.CurrentUser.USER_NAME;
            UserRoleBlock.Text = "Роль: " + Manager.CurrentUser.Role.NAME;
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList();
            MastersGrid.ItemsSource = Manager.Context.Master.ToList();
        }

        private void InitializeUserPhoto()
        {
            if (Manager.CurrentUser.USER_PHOTO != null)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(Manager.CurrentUser.USER_PHOTO);
                image.EndInit();
                UserImage.Source = image;
                DeletePictureItem.IsEnabled = true;
            }
            else
            {
                UserImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/user_picture.png"));
                DeletePictureItem.IsEnabled = false;
            }
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow(null)
            {
                Title = "Добавление новой услуги"
            };
            addEditServiceWindow.ShowDialog();
            UpdateEntries();
        }

        /// <summary>
        /// Shows hints in the StatusBar.
        /// </summary>
        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Source is Button button)
            {
                ButtonName.Text = button.Content.ToString();
            }
            if (e.Source is MenuItem menuItem)
            {
                ButtonName.Text = menuItem.Header.ToString();
            }
        }

        /// <summary>
        /// Make the StatusBar empty.
        /// </summary>
        private void MainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonName.Text = null;
        }

        private void ServiceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceGrid.SelectedItems.Count > 1)
            {
                BtnEditService.IsEnabled = ModifyServiceItem.IsEnabled = false;
            }
            else
            {
                BtnEditService.IsEnabled = ModifyServiceItem.IsEnabled = true;
            }
            BtnDeleteService.IsEnabled = DeleteServiceItem.IsEnabled = true;
        }

        private void BtnEditService_Click(object sender, RoutedEventArgs e)
        {
            Service service = ServiceGrid.SelectedItem as Service;
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow(service)
            {
                Title = "Редактирование услуги " + service.SE_NAME
            };
            addEditServiceWindow.ShowDialog();
        }

        private void UpdateEntries()
        {
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList();
            MastersGrid.ItemsSource = Manager.Context.Master.ToList();
        }

        private void BtnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            List<Service> services = ServiceGrid.SelectedItems.Cast<Service>().ToList();
            if (services.Count() > 0)
            {
                if (MessageBox.Show("Внимание! Будет удалено следующее число услуг: "
                    + services.Count() + $".\n" +
                    $"Нажмите \"Да\", если вы действительно хотите безвозвратно удалить выбранные услуги.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    services.ForEach(s => Manager.Context.PhotoOfService.RemoveRange(s.PhotoOfService.ToList()));
                    Manager.Context.Service.RemoveRange(services);
                    try
                    {
                        Manager.Context.SaveChanges();
                        UpdateEntries();
                        BtnEditService.IsEnabled = BtnDeleteService.IsEnabled = false;
                        ModifyServiceItem.IsEnabled = DeleteServiceItem.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Удаление неуспешно! Пожалуйста, попробуйте ещё раз." +
                            "\nОшибка: "
                            + ex.Message, "Ошибка удаления",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ChangePictureItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    Manager.CurrentUser.USER_PHOTO = File.ReadAllBytes(dialog.FileName);
                    Manager.Context.SaveChanges();
                    InitializeUserPhoto();
                    MessageBox.Show("Аватар успешно изменен!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Пожалуйста, прикрепите изображение.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void DeletePictureItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно удалить текущий аватар?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                Manager.CurrentUser.USER_PHOTO = null;
                Manager.Context.SaveChanges();
                InitializeUserPhoto();
            }
        }

        private void MainAdminWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Точно завершить текущую сессию пользователя "
                + Manager.CurrentUser.USER_NAME
                + "?",
                "Внимание",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                Manager.MainLoginRegisterWindow.Show();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddServiceItem_Click(object sender, RoutedEventArgs e)
        {
            BtnAddService_Click(null, null);
        }

        private void ModifyServiceItem_Click(object sender, RoutedEventArgs e)
        {
            BtnEditService_Click(null, null);
        }

        private void DeleteServiceItem_Click(object sender, RoutedEventArgs e)
        {
            BtnDeleteService_Click(null, null);
        }

        private void MastersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MastersGrid.SelectedItems.Count > 1)
            {
                BtnEditMaster.IsEnabled = EditMasterItem.IsEnabled = false;
            }
            else
            {
                BtnEditMaster.IsEnabled = EditMasterItem.IsEnabled = true;
            }
            BtnDeleteMaster.IsEnabled = DeleteMasterItem.IsEnabled = true;
        }

        private void BtnDeleteMaster_Click(object sender, RoutedEventArgs e)
        {
            List<Master> masters = MastersGrid.SelectedItems.Cast<Master>().ToList();
            if (MessageBox.Show("Действительно удалить следующее число мастеров: "
               + masters.Count
               + "?"
               + "\nЭто действие отменить нельзя!",
               "Внимание",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
            {
                Manager.Context.Master.RemoveRange(masters);
                try
                {
                    Manager.Context.SaveChanges();
                    MastersGrid.ItemsSource = Manager.Context.Master.ToList();
                    MessageBox.Show("Выбранные мастеры успешно удалены!",
                        "Успешно!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Данные не были удалены! Пожалуйста, попробуйте ещё раз." +
                        "\nОшибка:"
                        + ex.Message,
                        "Ошибка удаления",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddMaster_Click(object sender, RoutedEventArgs e)
        {
            AddEditMasterWindow masterWindow = new AddEditMasterWindow(null)
            {
                Title = "Добавление нового механика"
            };
            masterWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnEditMaster_Click(object sender, RoutedEventArgs e)
        {
            Master master = MastersGrid.SelectedItem as Master;
            AddEditMasterWindow masterWindow = new AddEditMasterWindow(master)
            {
                Title = "Изменение данных о механике " + master.M_NAME
            };
            masterWindow.ShowDialog();
            UpdateEntries();
        }

        private void AddMasterItem_Click(object sender, RoutedEventArgs e)
        {
            BtnAddMaster_Click(null, null);
        }

        private void EditMasterItem_Click(object sender, RoutedEventArgs e)
        {
            BtnEditMaster_Click(null, null);
        }

        private void DeleteMasterItem_Click(object sender, RoutedEventArgs e)
        {
            BtnDeleteMaster_Click(null, null);
        }

        /// <summary>
        /// Forms the .pdf and .docx documents with order statistic of masters and shows the .docx document after operations.
        /// </summary>
        private void MastersOrderReportForm_Click(object sender, RoutedEventArgs e)
        {
            var allMasters = Manager.Context.Master.ToList();
            var allServices = Manager.Context.Service.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            foreach (var master in allMasters)
            {
                Word.Paragraph paragraph = document.Paragraphs.Add();
                Word.Range masterRange = paragraph.Range;
                masterRange.Text = master.M_NAME;
                masterRange.set_Style("Заголовок 1");
                masterRange.InsertParagraphAfter();

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table ordersOfMasterTable = document.Tables.Add(tableRange, allServices.Count + 1, 2); ;
                ordersOfMasterTable.Borders.InsideLineStyle = ordersOfMasterTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                ordersOfMasterTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = ordersOfMasterTable.Cell(1, 1).Range;
                cellRange.Text = "Услуга";
                cellRange = ordersOfMasterTable.Cell(1, 2).Range;
                cellRange.Text = "Количество продаж";

                ordersOfMasterTable.Rows[1].Range.Bold = 1;
                ordersOfMasterTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                for (int i = 0; i < allServices.Count; i++)
                {
                    var currentService = allServices[i];

                    cellRange = ordersOfMasterTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentService.SE_NAME;
                    cellRange = ordersOfMasterTable.Cell(i + 2, 2).Range;
                    cellRange.Text = master.Order.Where(o => o.Service.Contains(currentService)).Count().ToString();
                }
                if (master != allMasters.LastOrDefault())
                {
                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }
            application.Visible = true;

            document.SaveAs(@"C:\Test.docx");
            document.SaveAs(@"C:\Test.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }
    }
}

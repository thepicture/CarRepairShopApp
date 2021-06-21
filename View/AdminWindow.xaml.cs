using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
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
            UpdateEntries();
            // CreateRandomTypeOfCar();
            // CreateRandomServices();
            // CreateRandomModelsOfServices();
            // AddDummyPhotosToService();
        }

        private void AddDummyPhotosToService()
        {
            foreach (Service service in Manager.Context.Service.ToList())
            {
                for (int i = 0; i < random.Next(0, 5); i++)
                {
                    Manager.Context.Entry(service).Entity.PhotoOfService
                        .Add(new PhotoOfService { PHOTO = Manager.Context.PhotoOfService.ToList().OrderBy(r => random.Next()).First().PHOTO });
                }
                Manager.Context.SaveChanges();
            }
        }

        readonly Random random = new Random();
        /// <summary>
        /// Inserts random types of car into database.
        /// </summary>
        private void CreateRandomTypeOfCar()
        {
            string[] charsPartOne = { "A", "B", "C", "D", "E" };
            string[] charsPartTwo = { "LITE", "PRO", "SUPER", "SUN" };
            foreach (Auto auto in Manager.Context.Auto.ToList())
            {
                for (int i = 0; i < random.Next(1, 10); i++)
                {
                    auto.TypeOfCar.Add(new TypeOfCar
                    {
                        T_NAME = charsPartOne[random.Next(0, charsPartOne.Length)]
                        + random.Next(100, 999).ToString()
                        + charsPartTwo[random.Next(0, charsPartTwo.Length)]
                    });
                }
                Manager.Context.Entry(auto).State = System.Data.Entity.EntityState.Modified;
            }
            Manager.Context.SaveChanges();
        }

        /// <summary>
        /// Inserts random models into services from database.
        /// </summary>
        private void CreateRandomModelsOfServices()
        {
            foreach (Service service in Manager.Context.Service.ToList())
            {
                for (int i = 0; i < random.Next(1, 10); i++)
                {
                    Manager.Context.Service.Find(service.SE_ID).ServiceOfModel.Add(new ServiceOfModel
                    {
                        COST = decimal.Parse((random.NextDouble() * 4900 + 100).ToString()),
                        TypeOfCar = Manager.Context.TypeOfCar.ToList().Except(Manager.Context.Service.Find(service.SE_ID).ServiceOfModel.Select(s => s.TypeOfCar)).OrderBy(r => random.Next()).First()
                    });
                    Manager.Context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Creates random services in database.
        /// </summary>
        private void CreateRandomServices()
        {
            string[] servicePartOne = { "Установка", "Замена", "Починка", "Покраска", "Ремонт" };
            string[] servicePartTwo = { "бампера", "окон", "подвески", "шин", "фар", "двигателя" };
            string[] servicePartThree = { "Делюкс", "PRO", "Лайт", "Стандарт", "VIP" };
            string[] descriptions = { "Гарантированное и качественное выполнение услуги ", "Быстрое и дешёвое выполнение услуги" };
            for (int i = 0; i < 300; i++)
            {
                Service service = new Service
                {
                    SE_NAME = servicePartOne[random.Next(0, servicePartOne.Length)]
                    + " "
                    + servicePartTwo[random.Next(0, servicePartThree.Length)]
                    + " "
                    + servicePartThree[random.Next(0, servicePartThree.Length)],
                    SE_DESCRIPTION = descriptions[random.Next(0, descriptions.Length)] + "!"
                };
                if (random.NextDouble() > 0.2)
                {
                    service.PhotoOfService.Add(Manager.Context.PhotoOfService.ToList().OrderBy(r => random.Next()).First());
                }
                Manager.Context.Service.Add(service);
            }
            Manager.Context.SaveChanges();
        }

        /// <summary>
        /// Initializes user photo.
        /// </summary>
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

        /// <summary>
        /// Opens a window for adding a new service.
        /// </summary>
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

        /// <summary>
        /// Makes button enabled if and only if selected items count is greather than 1.
        /// </summary>
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

        /// <summary>
        /// Open a window for editing the service.
        /// </summary>
        private void BtnEditService_Click(object sender, RoutedEventArgs e)
        {
            Service service = ServiceGrid.SelectedItem as Service;
            AddEditServiceWindow addEditServiceWindow = new AddEditServiceWindow(service)
            {
                Title = "Редактирование услуги " + service.SE_NAME
            };
            addEditServiceWindow.ShowDialog();
            UpdateEntries();
        }

        /// <summary>
        /// Updates the datagrids.
        /// </summary>
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
                        MessageBox.Show("Выбранные услуги успешно удалены!",
                            "Успешно!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Удаление неуспешно! Пожалуйста, попробуйте ещё раз." +
                            "\nОшибка: "
                            + ex.Message, "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Changes user picture.
        /// </summary>
        private void ChangePictureItem_Click(object sender, RoutedEventArgs e)
        {
            PictureChanger.ChangePicture();
            InitializeUserPhoto();
        }

        /// <summary>
        /// Deletes user picture.
        /// </summary>
        private void DeletePictureItem_Click(object sender, RoutedEventArgs e)
        {
            PictureChanger.DeletePicture();
            InitializeUserPhoto();
        }

        /// <summary>
        /// Closes current window and goes back to the login window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Closes current window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Checks if the selected items count is greater than one, otherwise the edit buttons are disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Deletes the selected count of masters.
        /// </summary>
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
                foreach (Master master in masters)
                {
                    //master.Phone.Clear();
                    master.Order.Clear();
                    Manager.Context.Entry(master).State = System.Data.Entity.EntityState.Deleted;
                }
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

        /// <summary>
        /// Opens the master add window.
        /// </summary>
        private void BtnAddMaster_Click(object sender, RoutedEventArgs e)
        {
            AddEditMasterWindow masterWindow = new AddEditMasterWindow(null)
            {
                Title = "Добавление нового механика"
            };
            masterWindow.ShowDialog();
            UpdateEntries();
        }

        /// <summary>
        /// Opens the master edit window.
        /// </summary>
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

        /// <summary>
        /// Forms the .pdf and .docx documents with order statistic of masters and shows the .docx document after operations.
        /// </summary>
        private void MastersOrderReportForm_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FolderGetter.GetSelectedPath();
            if (filePath == null)
            {
                return;
            }
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

            string currentDate = DateTime.Now.ToString("dd-MM-yyyy_hh-mm");

            try
            {
                document.SaveAs(filePath + @"\Отчёт-по-механикам_" + currentDate + ".docx");
                document.SaveAs(filePath + @"\Отчёт-по-механикам_" + currentDate + ".pdf", Word.WdExportFormat.wdExportFormatPDF);
            }
            catch (Exception)
            {
                ReportExceptionHandler.ShowErrorInformationAboutMSOffice();
            }
        }

        /// <summary>
        /// Calls the service report method from ServiceReportFormer class.
        /// </summary>
        private void ServicesReportForm_Click(object sender, RoutedEventArgs e)
        {
            ServiceReportFormer.FormReport();
        }

        private string filePath;
        private void BtnCreateBDBackup_Click(object sender, RoutedEventArgs e)
        {
            filePath = FolderGetter.GetSelectedPath();
            if (filePath == null)
            {
                return;
            }
            DatabaseBackupSaver.Save(filePath);
            MessageBox.Show("Резервная копия успешно создана по пути " + filePath,
                         "Успешно!",
                         MessageBoxButton.OK,
                         MessageBoxImage.Information);
            filePath = null;
        }

        private readonly DispatcherTimer timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromHours(3), // Every three hours
        };
        /// <summary>
        /// Creates backup of the database for every three hours.
        /// </summary>
        private void IsAutoBackup_Checked(object sender, RoutedEventArgs e)
        {
            filePath = FolderGetter.GetSelectedPath();
            if (filePath == null)
            {
                return;
            }
            timer.Tick += Timer_Tick;
            timer.Start();
            MessageBox.Show("Резервное копирование успешно включено!" +
                "\nДиректория файла: " + filePath,
                "Успешно!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Creates backup of the database when tick is occurs.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            DatabaseBackupSaver.Save(filePath);
        }

        /// <summary>
        /// Disables autobackup of the database.
        /// </summary>
        private void IsAutoBackup_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Автоматическое резервное копирование успешно выключено!",
                "Успешно!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            timer.Stop();
            filePath = null;
        }

        /// <summary>
        /// Finds services by its name.
        /// </summary>
        private void TBoxServiceSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ServiceGrid.ItemsSource = Manager.Context.Service.ToList().Where(s => s.SE_NAME.ToLower()
            .Contains(TBoxServiceSearch.Text.ToLower()));
        }

        /// <summary>
        /// Finds masters by their name.
        /// </summary>
        private void TBoxMasterSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            MastersGrid.ItemsSource = Manager.Context.Master.ToList().Where(m => m.M_NAME.ToLower()
            .Contains(TBoxMasterSearch.Text.ToLower()));
        }
    }
}

﻿using CarRepairShopApp.Model;
using CarRepairShopApp.ViewModel;
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
    /// Interaction logic for MasterWindow.xaml
    /// </summary>
    public partial class MasterWindow : Window
    {
        public MasterWindow()
        {
            InitializeComponent();
            InitializeUserPhoto();
            UserNameBlock.Text = Manager.CurrentUser.USER_NAME;
            UserRoleBlock.Text = "Роль: " + Manager.CurrentUser.Role.NAME;
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            OrdersGrid.SelectedItems.Clear();
            ContractGrid.ItemsSource = Manager.Context.Contract.ToList();
            OrdersGrid.ItemsSource = Manager.Context.Order.ToList();
            CustomersGrid.ItemsSource = Manager.Context.Client.ToList();
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

        private void ChangePictureItem_Click(object sender, RoutedEventArgs e)
        {
            PictureChanger.ChangePicture();
            InitializeUserPhoto();
        }

        private void DeletePictureItem_Click(object sender, RoutedEventArgs e)
        {
            PictureChanger.DeletePicture();
            InitializeUserPhoto();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CustomersReportForm_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FolderGetter.GetSelectedPath();
            if (filePath == null)
            {
                return;
            }
            var allCustomers = Manager.Context.Client.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            for (int i = 0; i < allCustomers.Count; i++)
            {
                Word.Paragraph paragraph = document.Paragraphs.Add();
                Word.Range customerRange = paragraph.Range;

                customerRange.Text = "Статистика заказов по клиенту " + allCustomers[i].CL_NAME;
                customerRange.set_Style("Заголовок 1");
                customerRange.InsertParagraphAfter();

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table serviceTable = document.Tables.Add(tableRange, 2, 2); ;
                serviceTable.Borders.InsideLineStyle = serviceTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                serviceTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = serviceTable.Cell(1, 1).Range;
                cellRange.Text = "Клиент";
                cellRange = serviceTable.Cell(1, 2).Range;
                cellRange.Text = "Среднее число заказов в месяц";

                serviceTable.Rows[1].Range.Bold = 1;
                serviceTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                var currentCustomer = allCustomers[i];

                cellRange = serviceTable.Cell(2, 1).Range;
                cellRange.Text = currentCustomer.CL_NAME;
                cellRange = serviceTable.Cell(2, 2).Range;
                if (currentCustomer.Order.Count() == 0)
                {
                    cellRange.Text = "Заказов не найдено";
                }
                else if (currentCustomer.Order.Max(o => o.O_CREATEDATE.Year) == currentCustomer.Order.Min(o => o.O_CREATEDATE.Year))
                {
                    cellRange.Text = Math.Round(currentCustomer.Order.Count / 12.0, 2).ToString() + $" шт.";
                }
                else
                {
                    cellRange.Text = Math.Round(currentCustomer.Order.Count / currentCustomer.Order.Max(o => o.O_CREATEDATE.Year) - currentCustomer.Order.Min(o => o.O_CREATEDATE.Year) * 1.0, 2).ToString() + " шт.";
                }
                if (currentCustomer != Manager.Context.Client.ToList().LastOrDefault())
                {
                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }
            application.Visible = true;

            string currentDate = DateTime.Now.ToString("dd-MM-yyyy_hh-mm");

            try
            {
                document.SaveAs(filePath + @"\Отчёт-по-заказам-клиентов_" + currentDate + ".docx");
                document.SaveAs(filePath + @"\Отчёт-по-заказам-клиентов_" + currentDate + ".pdf", Word.WdExportFormat.wdExportFormatPDF);
            }
            catch (Exception)
            {
                ReportExceptionHandler.ShowErrorInformationAboutMSOffice();
            }
        }

        private void BtnAddContract_Click(object sender, RoutedEventArgs e)
        {
            AddEditContractWindow contractWindow = new AddEditContractWindow(null)
            {
                Title = "Добавление нового контракта"
            };
            contractWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnEditContract_Click(object sender, RoutedEventArgs e)
        {
            Contract selectedContract = ContractGrid.SelectedItem as Contract;
            AddEditContractWindow contractWindow = new AddEditContractWindow(selectedContract)
            {
                Title = "Изменение договора от даты " + selectedContract.CO_DATE
            };
            contractWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnDeleteContract_Click(object sender, RoutedEventArgs e)
        {
            List<Contract> contracts = ContractGrid.SelectedItems.Cast<Contract>().ToList();
            if (contracts.Count() > 0)
            {
                if (MessageBox.Show("Внимание! Будет отменено следующее число договоров: "
                    + contracts.Count() + $".\n" +
                    $"Нажмите \"Да\", если вы действительно хотите безвозвратно отменить выбранные договоры.",
                    "Подтверждение отмены",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Manager.Context.Contract.RemoveRange(contracts);
                    try
                    {
                        Manager.Context.SaveChanges();
                        UpdateEntries();
                        BtnEditContract.IsEnabled = EditContractItem.IsEnabled = false;
                        BtnDeleteContract.IsEnabled = DeleteContractItem.IsEnabled = false;
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

        private void ContractGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContractGrid.SelectedItems.Count != 1)
            {
                BtnEditContract.IsEnabled = EditContractItem.IsEnabled = false;
            }
            else
            {
                BtnEditContract.IsEnabled = EditContractItem.IsEnabled = true;
            }
            BtnDeleteContract.IsEnabled = DeleteContractItem.IsEnabled = true;
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            AddEditOrderWindow orderWindow = new AddEditOrderWindow(null)
            {
                Title = "Формирование нового заказа"
            };
            orderWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = OrdersGrid.SelectedItem as Order;
            AddEditOrderWindow orderWindow = new AddEditOrderWindow(order)
            {
                Title = "Обновление информации о заказе даты " + order.O_CREATEDATE
            };
            orderWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            List<Order> orders = OrdersGrid.SelectedItems.Cast<Order>().ToList();
            if (orders.Count() > 0)
            {
                if (MessageBox.Show("Внимание! Будет безвозвратно отменено следующее число заказов: "
                    + orders.Count() + $".\n" +
                    $"Нажмите \"Да\", если вы действительно хотите безвозвратно отменить выбранные заказы.",
                    "Подтверждение отмены",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (Order order in orders)
                    {
                        order.Service.Clear();
                    }
                    Manager.Context.Order.RemoveRange(orders);
                    try
                    {
                        Manager.Context.SaveChanges();
                        UpdateEntries();
                        BtnEditOrder.IsEnabled = ModifyOrderItem.IsEnabled = false;
                        BtnDeleteOrder.IsEnabled = DeleteOrderItem.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Отмена неуспешна! Пожалуйста, попробуйте ещё раз." +
                            "\nОшибка: "
                            + ex.Message, "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItems.Count != 1)
            {
                BtnEditOrder.IsEnabled = ModifyOrderItem.IsEnabled = false;
            }
            else
            {
                BtnEditOrder.IsEnabled = ModifyOrderItem.IsEnabled = true;
            }
            if (OrdersGrid.SelectedItems.Count != 0)
            {
                BtnDeleteOrder.IsEnabled = DeleteOrderItem.IsEnabled = true;
            }
        }

        private void MasterWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
        /// Saves new status to the database.
        /// </summary>
        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItems.Count.Equals(0))
            {
                return;
            }
            try
            {
                Order order = OrdersGrid.SelectedItem as Order;
                Status status = (sender as ComboBox).SelectedItem as Status;
                Manager.Context.Order.Where(o => o.O_ID.Equals(order.O_ID)).First().Status = status;
                Manager.Context.SaveChanges();
                MessageBox.Show("Статус заказа успешно изменён!",
                    "Успешно!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("При изменении статуса произошла ошибка." +
                    "Пожалуйста, попробуйте изменить статус ещё раз.",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddEditCustomerWindow customerWindow = new AddEditCustomerWindow(null)
            {
                Title = "Добавление данных о новом клиенте"
            };
            customerWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            Client client = CustomersGrid.SelectedItem as Client;
            AddEditCustomerWindow customerWindow = new AddEditCustomerWindow(client)
            {
                Title = "Изменение данных о клиенте " + client.CL_NAME
            };
            customerWindow.ShowDialog();
            UpdateEntries();
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            List<Client> clients = CustomersGrid.SelectedItems.Cast<Client>().ToList();
            if (clients.Count() > 0)
            {
                if (MessageBox.Show("Внимание! Будет безвозвратно удалено следующее число данных о клиентах: "
                    + clients.Count() + $".\n" +
                    $"Нажмите \"Да\", если вы действительно хотите безвозвратно удалить выбранные данные о клиентах.",
                    "Подтверждение отмены",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (Client client in clients)
                    {
                        client.Auto.Clear();
                        client.Phone.Clear();
                    }
                    Manager.Context.Client.RemoveRange(clients);
                    try
                    {
                        Manager.Context.SaveChanges();
                        UpdateEntries();
                        BtnEditCustomer.IsEnabled = EditCustomerItem.IsEnabled = false;
                        BtnDeleteCustomer.IsEnabled = DeleteCustomerItem.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Отмена неуспешна! Пожалуйста, попробуйте ещё раз." +
                            "\nОшибка: "
                            + ex.Message, "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CustomersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomersGrid.SelectedItems.Count != 1)
            {
                BtnEditCustomer.IsEnabled = EditCustomerItem.IsEnabled = false;
            }
            else
            {
                BtnEditCustomer.IsEnabled = EditCustomerItem.IsEnabled = true;
            }
            BtnDeleteCustomer.IsEnabled = DeleteCustomerItem.IsEnabled = true;
        }

        private void ServicesReportForm_Click(object sender, RoutedEventArgs e)
        {
            ServiceReportFormer.FormReport();
        }
    }
}

using CarRepairShopApp.Model;
using System;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

namespace CarRepairShopApp.ViewModel
{
    class ServiceReportFormer
    {
        public static void FormReport()
        {
            string filePath = FolderGetter.GetSelectedPath();
            if (filePath == null)
            {
                return;
            }
            var allServices = Manager.Context.Service.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph paragraph = document.Paragraphs.Add();
            Word.Range serviceRange = paragraph.Range;
            serviceRange.Text = "Информация о средней цене каждой услуги за " + DateTime.Now.ToString();
            serviceRange.set_Style("Заголовок 1");
            serviceRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table serviceTable = document.Tables.Add(tableRange, allServices.Count + 1, 2); ;
            serviceTable.Borders.InsideLineStyle = serviceTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            serviceTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = serviceTable.Cell(1, 1).Range;
            cellRange.Text = "Услуга";
            cellRange = serviceTable.Cell(1, 2).Range;
            cellRange.Text = "Средняя цена";

            serviceTable.Rows[1].Range.Bold = 1;
            serviceTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            for (int i = 0; i < allServices.Count; i++)
            {
                var currentService = allServices[i];

                cellRange = serviceTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentService.SE_NAME;
                cellRange = serviceTable.Cell(i + 2, 2).Range;
                if (currentService.ServiceOfModel.Count() == 0)
                {
                    cellRange.Text = "Цен не найдено";
                    continue;
                }
                cellRange.Text = Math.Floor(currentService.ServiceOfModel.Sum(s => s.COST) / currentService.ServiceOfModel.Count()).ToString() + " руб.";
            }
            application.Visible = true;

            string currentDate = DateTime.Now.ToString("dd-MM-yyyy_hh-mm");

            try
            {
                document.SaveAs(filePath + @"\Отчёт-по-услугам_" + currentDate + ".docx");
                document.SaveAs(filePath + @"\Отчёт-по-услугам_" + currentDate + ".pdf", Word.WdExportFormat.wdExportFormatPDF);
            }
            catch (Exception)
            {
                ReportExceptionHandler.ShowErrorInformationAboutMSOffice();
            }
        }
    }
}

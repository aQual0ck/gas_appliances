using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout.Properties;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        private string _filepath;
        private AuxClasses.Appliance appl;
        private AuxClasses.Owners own;
        private AuxClasses.Users user;
        private int catid;
        private int statid;
        public PageAdmin()
        {
            InitializeComponent();
        }

        //private void menuReport_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog sfd = new SaveFileDialog();
        //    sfd.FileName = "Отчет";
        //    sfd.DefaultExt = ".pdf";

        //    PdfFont font = PdfFontFactory.CreateFont($"{Directory.GetParent(Environment.CurrentDirectory).Parent.FullName}\\Assets\\arial.ttf", "Identity-H");

        //    bool? result = sfd.ShowDialog();

        //    if (result == true)
        //    {
        //        _filepath = sfd.FileName;

        //        using (PdfWriter writer = new PdfWriter(_filepath))
        //        {
        //            using (PdfDocument pdf = new PdfDocument(writer))
        //            {
        //                Document doc = new Document(pdf, PageSize.DEFAULT);
        //                doc.SetFont(font);
        //                float[] columnWidths = { 10f, 20f, 20f, 10f, 10f, 30f };
        //                Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

        //                foreach (var column in dgrAppliances.Columns)
        //                {
        //                    table.AddHeaderCell(new Cell().Add(new Paragraph(column.Header.ToString())));
        //                }

        //                foreach (var item in dgrAppliances.Items)
        //                {
        //                    foreach (var column in dgrAppliances.Columns)
        //                    {
        //                        if (column.Header.ToString() != "Категория")
        //                        {
        //                            var cellContent = TypeDescriptor.GetProperties(item)[$"{column.SortMemberPath}"].GetValue(item);
        //                            string cellValue = cellContent != null ? cellContent.ToString() : string.Empty;
        //                            table.AddCell(new Cell().Add(new Paragraph(cellValue)));
        //                        }
        //                        else if (column.Header.ToString() == "Категория")
        //                        {
        //                            object cell = TypeDescriptor.GetProperties(item)["Category"].GetValue(item);
        //                            string cellValue = TypeDescriptor.GetProperties(cell)["CategoryName"].GetValue(cell).ToString();
        //                            table.AddCell(new Cell().Add(new Paragraph(cellValue)));
        //                        }
        //                    }
        //                }

        //                doc.Add(table);
        //                doc.Close();
        //            }
        //        }
        //        MessageBox.Show($"Отчет сохранен по данному пути: {_filepath}");
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using System.IO;
using System.ComponentModel;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageToolCheck.xaml
    /// </summary>
    public partial class PageToolCheck : Page
    {
        public PageToolCheck()
        {
            InitializeComponent();
            dgrToolExam.ItemsSource = AuxClasses.DBClass.entObj.ToolCheck.ToList();

            if (string.IsNullOrEmpty(txbSearchTools.Text))
            {
                txbSearchTools.Text = "по инструментам";
                txbSearchTools.Foreground = Brushes.Gray;
                txbSearchTools.GotFocus += RemoveTextSearchTools;
                txbSearchTools.LostFocus += AddTextSearchTools;
            }

            if (string.IsNullOrEmpty(txbSearchUsers.Text))
            {
                txbSearchUsers.Text = "по проверяющим";
                txbSearchUsers.Foreground = Brushes.Gray;
                txbSearchUsers.GotFocus += RemoveTextSearchUsers;
                txbSearchUsers.LostFocus += AddTextSearchUsers;
            }
        }

        private void menuAddExam_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddToolCheck());
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void txbSearchTools_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void txbSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var query = AuxClasses.DBClass.entObj.ToolCheck.AsQueryable();

            DateTime? cds = dpCheckDateStart.SelectedDate;
            DateTime? cde = dpCheckDateEnd.SelectedDate;

            if (!string.IsNullOrEmpty(txbSearchTools.Text) && txbSearchTools.Text != "по инструментам")
                query = query.Where(x => x.Tool.ModelName.ToLower().Contains(txbSearchTools.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchUsers.Text) && txbSearchUsers.Text != "по проверяющим")
                query = query.Where(x => x.Users.FullName.ToLower().Contains(txbSearchUsers.Text.ToLower()));

            if (dpCheckDateStart.SelectedDate != null)
                query = query.Where(x => x.CheckDate >= cds);

            if (dpCheckDateEnd.SelectedDate != null)
                query = query.Where(x => x.CheckDate < cde);

            dgrToolExam.ItemsSource = query.ToList();
        }

        private void RemoveTextSearchTools(object sender, EventArgs e)
        {
            if (txbSearchTools.Text == "по инструментам")
            {
                txbSearchTools.Text = "";
                txbSearchTools.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchTools(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchTools.Text))
            {
                txbSearchTools.Text = "по инструментам";
                txbSearchTools.Foreground = Brushes.Gray;
            }
        }

        private void RemoveTextSearchUsers(object sender, EventArgs e)
        {
            if (txbSearchUsers.Text == "по проверяющим")
            {
                txbSearchUsers.Text = "";
                txbSearchUsers.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchUsers(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchUsers.Text))
            {
                txbSearchUsers.Text = "по проверяющим";
                txbSearchUsers.Foreground = Brushes.Gray;
            }
        }

        private void dpCheckDateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpCheckDateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private string _filepath;
        private void menuReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "Отчет";
                sfd.DefaultExt = ".pdf";

                PdfFont font = PdfFontFactory.CreateFont($"{Directory.GetParent(Environment.CurrentDirectory).Parent.FullName}\\Assets\\arial.ttf", "Identity-H");

                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    _filepath = sfd.FileName;

                    using (PdfWriter writer = new PdfWriter(_filepath))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            Document doc = new Document(pdf, PageSize.DEFAULT);
                            doc.SetFont(font);
                            float[] columnWidths = { 30f, 30f, 10f };
                            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

                            foreach (var column in dgrToolExam.Columns)
                            {
                                table.AddHeaderCell(new Cell().Add(new Paragraph(column.Header.ToString())));
                            }

                            foreach (var item in dgrToolExam.Items)
                            {
                                foreach (var column in dgrToolExam.Columns)
                                {
                                    if (column.Header.ToString() != "Проверяющий" && column.Header.ToString() != "Инструмент")
                                    {
                                        DateTime? cellContent = (DateTime?)TypeDescriptor.GetProperties(item)[$"{column.SortMemberPath}"].GetValue(item);
                                        string cellValue = cellContent != null ? cellContent?.ToString("dd-MM-yyyy") : string.Empty;
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                    }
                                    else if (column.Header.ToString() == "Проверяющий")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["Users"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["FullName"].GetValue(cell).ToString();
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                    }
                                    else if (column.Header.ToString() == "Инструмент")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["Tool"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["ModelName"].GetValue(cell).ToString();
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                    }
                                }
                            }

                            doc.Add(table);
                            doc.Close();
                        }
                    }
                    MessageBox.Show($"Отчет сохранен по данному пути: {_filepath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message.ToString(), "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

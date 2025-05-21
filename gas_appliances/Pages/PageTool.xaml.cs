using gas_appliances.AuxClasses;
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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Layout.Properties;
using System.IO;
using System.Text.RegularExpressions;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageTool.xaml
    /// </summary>
    public partial class PageTool : Page
    {
        private int catid;
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageTool()
        {
            InitializeComponent();
            dgrTools.ItemsSource = AuxClasses.DBClass.entObj.Tool.ToList();

            cmbCategory.SelectedValuePath = "ToolCategoryName";
            cmbCategory.DisplayMemberPath = "ToolCategoryName";
            var cat = AuxClasses.DBClass.entObj.ToolCategory.ToList();
            cat.Insert(0, new AuxClasses.ToolCategory { Id = 0, ToolCategoryName = "Все категории" });
            cmbCategory.ItemsSource = cat;
            cmbCategory.SelectedIndex = 0;

            if (string.IsNullOrEmpty(txbSearchTools.Text))
            {
                txbSearchTools.Text = "по названию";
                txbSearchTools.Foreground = Brushes.Gray;
                txbSearchTools.GotFocus += RemoveTextSearchTools;
                txbSearchTools.LostFocus += AddTextSearchTools;
            }

            if (string.IsNullOrEmpty(txbSearchToolsSN.Text))
            {
                txbSearchToolsSN.Text = "по серийному номеру";
                txbSearchToolsSN.Foreground = Brushes.Gray;
                txbSearchToolsSN.GotFocus += RemoveTextSearchSN;
                txbSearchToolsSN.LostFocus += AddTextSearchSN;
            }
        }

        private void menuAddTool_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddTool());
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void txbSearchTools_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dgrTools_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditTool(dgrTools.SelectedItem));
        }

        private void ApplyFilters()
        {
            if (cmbCategory.SelectedItem != null)
                catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectedItem)["Id"].GetValue(cmbCategory.SelectedItem));
            else
                catid = 0;

            DateTime? oss = dpOperatingSinceStart.SelectedDate;
            DateTime? ose = dpOperatingSinceEnd.SelectedDate;
            DateTime? nes = dpNextExaminationStart.SelectedDate;
            DateTime? nee = dpNextExaminationEnd.SelectedDate;
            DateTime? dss = dpDecomissionedSinceStart.SelectedDate;
            DateTime? dse = dpDecomissionedSinceEnd.SelectedDate;

            var query = AuxClasses.DBClass.entObj.Tool.AsQueryable();

            if (catid != 0)
                query = query.Where(x => x.CategoryId == catid);

            if (!string.IsNullOrEmpty(txbSearchTools.Text) && txbSearchTools.Text != "по названию")
                query = query.Where(x => x.ModelName.ToLower().Contains(txbSearchTools.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchToolsSN.Text) && txbSearchToolsSN.Text != "по серийному номеру")
                query = query.Where(x => x.SerialNumber.ToLower().Contains(txbSearchToolsSN.Text.ToLower()));

            if (dpOperatingSinceStart.SelectedDate != null)
                query = query.Where(x => x.OperatingSince >= oss);

            if (dpOperatingSinceEnd.SelectedDate != null)
                query = query.Where(x => x.OperatingSince < ose);

            if (dpNextExaminationStart.SelectedDate != null)
                query = query.Where(x => x.NextExamination >= nes);

            if (dpNextExaminationEnd.SelectedDate != null)
                query = query.Where(x => x.NextExamination < nee);

            if (dpDecomissionedSinceStart.SelectedDate != null)
                query = query.Where(x => x.DecomissionedSince >= dss);

            if (dpDecomissionedSinceEnd.SelectedDate != null)
                query = query.Where(x => x.DecomissionedSince < dse);

            dgrTools.ItemsSource = query.ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void txbSearchToolsSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void RemoveTextSearchTools(object sender, EventArgs e)
        {
            if (txbSearchTools.Text == "по названию")
            {
                txbSearchTools.Text = "";
                txbSearchTools.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchTools(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchTools.Text))
            {
                txbSearchTools.Text = "по названию";
                txbSearchTools.Foreground = Brushes.Gray;
            }
        }

        private void RemoveTextSearchSN(object sender, EventArgs e)
        {
            if (txbSearchToolsSN.Text == "по серийному номеру")
            {
                txbSearchToolsSN.Text = "";
                txbSearchToolsSN.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchSN(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchToolsSN.Text))
            {
                txbSearchToolsSN.Text = "по серийному номеру";
                txbSearchToolsSN.Foreground = Brushes.Gray;
            }
        }

        private void dpOperatingSinceStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpOperatingSinceEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpNextExaminationStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpNextExaminationEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpDecomissionedSinceStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpDecomissionedSinceEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private string _filepath;
        private void menuReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = $"Отчет_{DateTime.Now.ToString("dd-MM-yyyy")}";
                sfd.DefaultExt = ".pdf";

                PdfFont font = PdfFontFactory.CreateFont($"{Environment.GetEnvironmentVariable("SystemRoot")}\\Fonts\\times.ttf", "Identity-H");

                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    _filepath = sfd.FileName;

                    using (PdfWriter writer = new PdfWriter(_filepath))
                    {
                        using (PdfDocument pdf = new PdfDocument(writer))
                        {
                            Document doc = new Document(pdf, PageSize.DEFAULT.Rotate());
                            doc.SetFont(font);
                            float[] columnWidths = { 5f, 20f, 20f, 10f, 10f, 10f, 10f, 10f, 5f };
                            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

                            foreach (var column in dgrTools.Columns)
                            {
                                table.AddHeaderCell(new Cell().Add(new Paragraph(column.Header.ToString())));
                            }

                            foreach (var item in dgrTools.Items)
                            {
                                foreach (var column in dgrTools.Columns)
                                {
                                    if (column.Header.ToString() != "Категория" && column.Header.ToString() != "Производитель")
                                    {
                                        if (column.Header.ToString() == "В экспл." || column.Header.ToString() == "След. поверка" || column.Header.ToString() == "Списан")
                                        {
                                            DateTime? cellContent = (DateTime?)TypeDescriptor.GetProperties(item)[$"{column.SortMemberPath}"].GetValue(item);
                                            string cellValue = cellContent != null ? cellContent?.ToString("dd-MM-yyyy") : string.Empty;
                                            table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                        }
                                        else
                                        {
                                            var cellContent = TypeDescriptor.GetProperties(item)[$"{column.SortMemberPath}"].GetValue(item);
                                            string cellValue = cellContent != null ? cellContent.ToString() : string.Empty;
                                            table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                        }
                                    }
                                    else if (column.Header.ToString() == "Категория")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["ToolCategory"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["ToolCategoryName"].GetValue(cell).ToString();
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                    }
                                    else if (column.Header.ToString() == "Производитель")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["ToolManufacturer"].GetValue(item);
                                        object cellAux = TypeDescriptor.GetProperties(cell)["ToolManufacturerType"].GetValue(cell);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["ManufacturerName"].GetValue(cell).ToString();
                                        string cellValueAux = TypeDescriptor.GetProperties(cellAux)["ManufacturerTypeName"].GetValue(cellAux).ToString();
                                        string final = cellValueAux + " " + cellValue;
                                        table.AddCell(new Cell().Add(new Paragraph(final)));
                                    }
                                }
                            }

                            Paragraph paragraph = new Paragraph($"Дата отчета: {DateTime.Now.ToString("dd-MM-yyyy")}");
                            doc.Add(paragraph);
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

        private void dpOperatingSinceStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpOperatingSinceEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpNextExaminationStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpNextExaminationEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpDecomissionedSinceStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpDecomissionedSinceEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

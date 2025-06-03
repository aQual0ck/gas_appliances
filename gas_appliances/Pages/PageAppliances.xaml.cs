using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
    /// Interaction logic for PageAppliances.xaml
    /// </summary>
    public partial class PageAppliances : Page
    {
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageAppliances()
        {
            InitializeComponent();
            dgrAppliances.ItemsSource = AuxClasses.DBClass.entObj.Appliance.ToList();

            cmbCategory.SelectedValuePath = "CategoryName";
            cmbCategory.DisplayMemberPath = "CategoryName";
            var cat = AuxClasses.DBClass.entObj.Category.ToList();
            cat.Insert(0, new AuxClasses.Category { Id = 0, CategoryName = "Все категории" });
            cmbCategory.ItemsSource = cat;
            cmbCategory.SelectedIndex = 0;

            cmbStatus.SelectedValuePath = "StatusName";
            cmbStatus.DisplayMemberPath = "StatusName";
            var stat = AuxClasses.DBClass.entObj.Statuses.ToList();
            stat.Insert(0, new AuxClasses.Statuses { Id = 0, StatusName = "Все статусы" });
            cmbStatus.ItemsSource = stat;
            cmbStatus.SelectedIndex = 0;

            if (string.IsNullOrEmpty(txbSearchAppliances.Text))
            {
                txbSearchAppliances.Text = "по названию";
                txbSearchAppliances.Foreground = Brushes.Gray;
                txbSearchAppliances.GotFocus += RemoveTextSearchAppliances;
                txbSearchAppliances.LostFocus += AddTextSearchAppliances;
            }

            if (string.IsNullOrEmpty(txbSearchAppliancesSN.Text))
            {
                txbSearchAppliancesSN.Text = "по серийному номеру";
                txbSearchAppliancesSN.Foreground = Brushes.Gray;
                txbSearchAppliancesSN.GotFocus += RemoveTextSearchSN;
                txbSearchAppliancesSN.LostFocus += AddTextSearchSN;
            }

            if (string.IsNullOrEmpty(txbSearchOwners.Text))
            {
                txbSearchOwners.Text = "по владельцам";
                txbSearchOwners.Foreground = Brushes.Gray;
                txbSearchOwners.GotFocus += RemoveTextSearchOwners;
                txbSearchOwners.LostFocus += AddTextSearchOwners;
            }
        }

        private int catid;
        private int statid;
        private void ApplyFilters()
        {
            if (cmbCategory.SelectedItem != null)
                catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectedItem)["Id"].GetValue(cmbCategory.SelectedItem));
            else
                catid = 0;

            if (cmbStatus.SelectedItem != null)
                statid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbStatus.SelectedItem)["Id"].GetValue(cmbStatus.SelectedItem));
            else
                statid = 0;

            DateTime? iss = dpInstalledSinceStart.SelectedDate;
            DateTime? ise = dpInstalledSinceEnd.SelectedDate;
            DateTime? nes = dpNextExaminationStart.SelectedDate;
            DateTime? nee = dpNextExaminationEnd.SelectedDate;

            var queryAppl = AuxClasses.DBClass.entObj.Appliance.AsQueryable();

            if (catid != 0)
                queryAppl = queryAppl.Where(x => x.CategoryId == catid);

            if (statid != 0)
                queryAppl = queryAppl.Where(x => x.StatusId == statid);

            if (!string.IsNullOrEmpty(txbSearchAppliances.Text) && txbSearchAppliances.Text != "по названию")
                queryAppl = queryAppl.Where(x => x.ApplianceName.ToLower().Contains(txbSearchAppliances.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchAppliancesSN.Text) && txbSearchAppliancesSN.Text != "по серийному номеру")
                queryAppl = queryAppl.Where(x => x.SerialNumber.ToLower().Contains(txbSearchAppliancesSN.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchOwners.Text) && txbSearchOwners.Text != "по владельцам")
                queryAppl = queryAppl.Where(x => x.Owners.OwnerName.ToLower().Contains(txbSearchOwners.Text.ToLower()));

            if (dpInstalledSinceStart.SelectedDate != null)
                queryAppl = queryAppl.Where(x => x.InstalledSince >= iss);

            if (dpInstalledSinceEnd.SelectedDate != null)
                queryAppl = queryAppl.Where(x => x.InstalledSince < ise);

            if (dpNextExaminationStart.SelectedDate != null)
                queryAppl = queryAppl.Where(x => x.NextExamination >= nes);

            if (dpNextExaminationEnd.SelectedDate != null)
                queryAppl = queryAppl.Where(x => x.NextExamination < nee);

            dgrAppliances.ItemsSource = queryAppl.ToList();
        }

        private void txbSearchAppliances_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void menuAddAppliance_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddAppliance());
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dgrAppliances_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgrAppliances.SelectedItem != null)
            {
                AuxClasses.FrameClass.frmObj.Navigate(new PageEditAppliance(dgrAppliances.SelectedItem));
            }
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void txbSearchAppliancesSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void RemoveTextSearchAppliances(object sender, EventArgs e)
        {
            if (txbSearchAppliances.Text == "по названию")
            {
                txbSearchAppliances.Text = "";
                txbSearchAppliances.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchAppliances(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchAppliances.Text))
            {
                txbSearchAppliances.Text = "по названию";
                txbSearchAppliances.Foreground = Brushes.Gray;
            }
        }

        private void RemoveTextSearchSN(object sender, EventArgs e)
        {
            if (txbSearchAppliancesSN.Text == "по серийному номеру")
            {
                txbSearchAppliancesSN.Text = "";
                txbSearchAppliancesSN.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchSN(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchAppliancesSN.Text))
            {
                txbSearchAppliancesSN.Text = "по серийному номеру";
                txbSearchAppliancesSN.Foreground = Brushes.Gray;
            }
        }

        private void RemoveTextSearchOwners(object sender, EventArgs e)
        {
            if (txbSearchOwners.Text == "по владельцам")
            {
                txbSearchOwners.Text = "";
                txbSearchOwners.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchOwners(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchOwners.Text))
            {
                txbSearchOwners.Text = "по владельцам";
                txbSearchOwners.Foreground = Brushes.Gray;
            }
        }

        private void dpInstalledSinceStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpInstalledSinceEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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
                            float[] columnWidths = { 10f, 20f, 20f, 10f, 10f, 10f, 10f, 10f };
                            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

                            foreach (var column in dgrAppliances.Columns)
                            {
                                table.AddHeaderCell(new Cell().Add(new Paragraph(column.Header.ToString())));
                            }

                            foreach (var item in dgrAppliances.Items)
                            {
                                foreach (var column in dgrAppliances.Columns)
                                {
                                    if (column.Header.ToString() != "Статус" && column.Header.ToString() != "Категория" && column.Header.ToString() != "Владелец")
                                    {
                                        if (column.Header.ToString() == "В экспл." || column.Header.ToString() == "След. поверка")
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
                                    else if (column.Header.ToString() == "Статус")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["Statuses"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["StatusName"].GetValue(cell).ToString();
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                    }
                                    else if (column.Header.ToString() == "Категория")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["Category"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["CategoryName"].GetValue(cell).ToString();
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
                                    }
                                    else if (column.Header.ToString() == "Владелец")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["Owners"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["OwnerName"].GetValue(cell).ToString();
                                        table.AddCell(new Cell().Add(new Paragraph(cellValue)));
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

        private void txbSearchOwners_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dpInstalledSinceStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpInstalledSinceEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }
}

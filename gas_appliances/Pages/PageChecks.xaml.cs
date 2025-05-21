using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Win32;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using iText.Layout;
using System.Text.RegularExpressions;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageChecks.xaml
    /// </summary>
    public partial class PageChecks : Page
    {
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageChecks()
        {
            InitializeComponent();
            dgrExam.ItemsSource = AuxClasses.DBClass.entObj.ApplianceCheck.ToList();

            if (string.IsNullOrEmpty(txbSearchAppliances.Text))
            {
                txbSearchAppliances.Text = "по приборам";
                txbSearchAppliances.Foreground = Brushes.Gray;
                txbSearchAppliances.GotFocus += RemoveTextSearchAppliances;
                txbSearchAppliances.LostFocus += AddTextSearchAppliances;
            }

            if (string.IsNullOrEmpty(txbSearchUsers.Text))
            {
                txbSearchUsers.Text = "по проверяющим";
                txbSearchUsers.Foreground = Brushes.Gray;
                txbSearchUsers.GotFocus += RemoveTextSearchUsers;
                txbSearchUsers.LostFocus += AddTextSearchUsers;
            }

            if (string.IsNullOrEmpty(txbSearchOwners.Text))
            {
                txbSearchOwners.Text = "по владельцам";
                txbSearchOwners.Foreground = Brushes.Gray;
                txbSearchOwners.GotFocus += RemoveTextSearchOwners;
                txbSearchOwners.LostFocus += AddTextSearchOwners;
            }
        }

        private void menuAddExam_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddExam());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgrExam.ItemsSource = AuxClasses.DBClass.entObj.ApplianceCheck.ToList();
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void txbSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void txbSearchAppliances_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var query = AuxClasses.DBClass.entObj.ApplianceCheck.AsQueryable();

            DateTime? cds = dpCheckDateStart.SelectedDate;
            DateTime? cde = dpCheckDateEnd.SelectedDate;

            if (!string.IsNullOrEmpty(txbSearchAppliances.Text) && txbSearchAppliances.Text != "по приборам")
                query = query.Where(x => x.Appliance.ApplianceName.ToLower().Contains(txbSearchAppliances.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchUsers.Text) && txbSearchUsers.Text != "по проверяющим")
                query = query.Where(x => x.Users.FullName.ToLower().Contains(txbSearchUsers.Text.ToLower()));
            
            if (!string.IsNullOrEmpty(txbSearchOwners.Text) && txbSearchOwners.Text != "по владельцам")
                query = query.Where(x => x.Appliance.Owners.OwnerName.ToLower().Contains(txbSearchOwners.Text.ToLower()));

            if (dpCheckDateStart.SelectedDate != null)
                query = query.Where(x => x.CheckDate >= cds);

            if (dpCheckDateEnd.SelectedDate != null)
                query = query.Where(x => x.CheckDate < cde);

            dgrExam.ItemsSource = query.ToList();
        }

        private void RemoveTextSearchAppliances(object sender, EventArgs e)
        {
            if (txbSearchAppliances.Text == "по приборам")
            {
                txbSearchAppliances.Text = "";
                txbSearchAppliances.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchAppliances(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchAppliances.Text))
            {
                txbSearchAppliances.Text = "по приборам";
                txbSearchAppliances.Foreground = Brushes.Gray;
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
                            Document doc = new Document(pdf, PageSize.DEFAULT);
                            doc.SetFont(font);
                            float[] columnWidths = { 30f, 30f, 30f, 10f };
                            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();

                            foreach (var column in dgrExam.Columns)
                            {
                                table.AddHeaderCell(new Cell().Add(new Paragraph(column.Header.ToString())));
                            }

                            foreach (var item in dgrExam.Items)
                            {
                                foreach (var column in dgrExam.Columns)
                                {
                                    if (column.Header.ToString() != "Проверяющий" && column.Header.ToString() != "Прибор")
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
                                    else if (column.Header.ToString() == "Прибор")
                                    {
                                        object cell = TypeDescriptor.GetProperties(item)["Appliance"].GetValue(item);
                                        string cellValue = TypeDescriptor.GetProperties(cell)["ApplianceName"].GetValue(cell).ToString();
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

        private void dpCheckDateEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpCheckDateStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

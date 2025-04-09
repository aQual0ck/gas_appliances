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
        private int roleid;
        public PageAdmin()
        {
            InitializeComponent();
            dgrAppliances.ItemsSource = AuxClasses.DBClass.entObj.Appliance.ToList();
            dgrOwners.ItemsSource = AuxClasses.DBClass.entObj.Owners.ToList();
            dgrUsers.ItemsSource = AuxClasses.DBClass.entObj.Users.ToList();
            dgrExam.ItemsSource = AuxClasses.DBClass.entObj.ApplianceCheck.ToList();

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

            cmbRole.SelectedValuePath = "RoleName";
            cmbRole.DisplayMemberPath = "RoleName";
            var role = AuxClasses.DBClass.entObj.Roles.ToList();
            role.Insert(0, new AuxClasses.Roles { Id = 0, RoleName = "Все роли" });
            cmbRole.ItemsSource = role;
            cmbRole.SelectedIndex = 0;
        }

        private void txbSearchAppliances_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void menuAddOwner_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddOwner());
        }

        private void txbSearchOwners_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dgrOwners_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditOwner(dgrOwners.SelectedItem));
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
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditAppliance(dgrAppliances.SelectedItem));
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

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void menuAddUser_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddUser());
        }

        private void txbSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dgrUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditUser(dgrUsers.SelectedItem));
        }

        private void ApplyFilters()
        {
            if (cmbCategory.SelectedItem != null || cmbCategory.SelectedIndex > 0)
                catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectedItem)["Id"].GetValue(cmbCategory.SelectedItem));
            else
                catid = 0;

            if (cmbStatus.SelectedItem != null)
                statid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbStatus.SelectedItem)["Id"].GetValue(cmbStatus.SelectedItem));
            else
                statid = 0;

            if (cmbRole.SelectedItem != null)
                roleid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbRole.SelectedItem)["Id"].GetValue(cmbRole.SelectedItem));
            else
                roleid = 0;

            var queryAppl = AuxClasses.DBClass.entObj.Appliance.AsQueryable();
            var queryOwner = AuxClasses.DBClass.entObj.Owners.AsQueryable();
            var queryUser = AuxClasses.DBClass.entObj.Users.AsQueryable();

            if (catid != 0)
                queryAppl = queryAppl.Where(x => x.CategoryId == catid);

            if (statid != 0)
                queryAppl = queryAppl.Where(x => x.StatusId == statid);

            if (!string.IsNullOrEmpty(txbSearchAppliances.Text))
                queryAppl = queryAppl.Where(x => x.ApplianceName.ToLower().Contains(txbSearchAppliances.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchOwners.Text))
                queryOwner = queryOwner.Where(x => x.OwnerName.ToLower().Contains(txbSearchOwners.Text.ToLower()));

            if (roleid != 0)
                queryUser = queryUser.Where(x => x.RoleId == roleid);

            if (!string.IsNullOrEmpty(txbSearchUsers.Text))
                queryUser = queryUser.Where(x => x.FullName.ToLower().Contains(txbSearchUsers.Text.ToLower()));

            dgrAppliances.ItemsSource = queryAppl.ToList();
            dgrOwners.ItemsSource = queryOwner.ToList();
            dgrUsers.ItemsSource = queryUser.ToList();
        }

        private void menuAddExam_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddExam());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnRefresh2_Click(object sender, RoutedEventArgs e)
        {
            dgrExam.ItemsSource = AuxClasses.DBClass.entObj.ApplianceCheck.ToList();
        }
    }
}

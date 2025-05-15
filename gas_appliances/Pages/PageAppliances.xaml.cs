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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageAppliances.xaml
    /// </summary>
    public partial class PageAppliances : Page
    {
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

            var queryAppl = AuxClasses.DBClass.entObj.Appliance.AsQueryable();

            if (catid != 0)
                queryAppl = queryAppl.Where(x => x.CategoryId == catid);

            if (statid != 0)
                queryAppl = queryAppl.Where(x => x.StatusId == statid);

            if (!string.IsNullOrEmpty(txbSearchAppliances.Text))
                queryAppl = queryAppl.Where(x => x.ApplianceName.ToLower().Contains(txbSearchAppliances.Text.ToLower()));

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
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditAppliance(dgrAppliances.SelectedItem));
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageManufacturers.xaml
    /// </summary>
    public partial class PageManufacturers : Page
    {
        public PageManufacturers()
        {
            InitializeComponent();
            dgrManufacturers.ItemsSource = AuxClasses.DBClass.entObj.ToolManufacturer.ToList();

            cmbType.SelectedValuePath = "ManufacturerTypeName";
            cmbType.DisplayMemberPath = "ManufacturerTypeName";
            var type = AuxClasses.DBClass.entObj.ToolManufacturerType.ToList();
            type.Insert(0, new AuxClasses.ToolManufacturerType { Id = 0, ManufacturerTypeName = "Все типы" });
            cmbType.ItemsSource = type;
            cmbType.SelectedIndex = 0;
        }

        private void menuAddManufacturer_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddManufacturer());
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void txbSearchManufacturers_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void dgrManufacturers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditManufacturer(dgrManufacturers.SelectedItem));
        }

        private int typeid;
        private void ApplyFilters()
        {
            if (cmbType.SelectedItem != null)
                typeid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbType.SelectedItem)["Id"].GetValue(cmbType.SelectedItem));
            else
                typeid = 0;

            var queryMan = AuxClasses.DBClass.entObj.ToolManufacturer.AsQueryable();

            if (typeid != 0)
                queryMan = queryMan.Where(x => x.ManufacturerTypeId == typeid);
            if (!string.IsNullOrEmpty(txbSearchManufacturers.Text))
                queryMan = queryMan.Where(x => x.ManufacturerName.ToLower().Contains(txbSearchManufacturers.Text.ToLower()));

            dgrManufacturers.ItemsSource = queryMan.ToList();
        }
    }
}

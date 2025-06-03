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

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageOwners.xaml
    /// </summary>
    public partial class PageOwners : Page
    {
        public PageOwners()
        {
            InitializeComponent();
            dgrOwners.ItemsSource = AuxClasses.DBClass.entObj.Owners.ToList();
        }

        private void ApplyFilters()
        {
            var queryOwner = AuxClasses.DBClass.entObj.Owners.AsQueryable();

            if (!string.IsNullOrEmpty(txbSearchOwners.Text))
                queryOwner = queryOwner.Where(x => x.OwnerName.ToLower().Contains(txbSearchOwners.Text.ToLower()));

            dgrOwners.ItemsSource = queryOwner.ToList();
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
            if (dgrOwners.SelectedItem != null)
            {
                AuxClasses.FrameClass.frmObj.Navigate(new PageEditOwner(dgrOwners.SelectedItem));
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
    }
}

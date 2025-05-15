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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageTool.xaml
    /// </summary>
    public partial class PageTool : Page
    {
        private int catid;
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

            var query = AuxClasses.DBClass.entObj.Tool.AsQueryable();

            if (catid != 0)
                query = query.Where(x => x.CategoryId == catid);

            if (!string.IsNullOrEmpty(txbSearchTools.Text))
                query = query.Where(x => x.ModelName.ToLower().Contains(txbSearchTools.Text.ToLower()));

            dgrTools.ItemsSource = query.ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }
    }
}

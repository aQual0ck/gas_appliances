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
    /// Interaction logic for PageChecks.xaml
    /// </summary>
    public partial class PageChecks : Page
    {
        public PageChecks()
        {
            InitializeComponent();
            dgrExam.ItemsSource = AuxClasses.DBClass.entObj.ApplianceCheck.ToList();
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
    }
}

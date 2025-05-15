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
    /// Interaction logic for PageToolCheck.xaml
    /// </summary>
    public partial class PageToolCheck : Page
    {
        public PageToolCheck()
        {
            InitializeComponent();
            dgrToolExam.ItemsSource = AuxClasses.DBClass.entObj.ToolCheck.ToList();
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
            dgrToolExam.ItemsSource = AuxClasses.DBClass.entObj.ToolCheck.ToList();
        }
    }
}

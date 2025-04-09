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
    /// Логика взаимодействия для PageAddOwner.xaml
    /// </summary>
    public partial class PageAddOwner : Page
    {
        private AuxClasses.Owners own;
        public PageAddOwner()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            own = new AuxClasses.Owners()
            {
                OwnerName = txbOwnerName.Text,
                ContactInfo = txbContactInfo.Text
            };
            AuxClasses.DBClass.entObj.Owners.Add(own);
            AuxClasses.DBClass.entObj.SaveChanges();
            MessageBox.Show("Добавлено");
        }
    }
}

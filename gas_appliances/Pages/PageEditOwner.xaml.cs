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
    /// Логика взаимодействия для PageEditOwner.xaml
    /// </summary>
    public partial class PageEditOwner : Page
    {
        private AuxClasses.Owners own;
        public PageEditOwner(object item)
        {
            InitializeComponent();
            DataContext = item;

            var id = TypeDescriptor.GetProperties(DataContext)["Id"].GetValue(DataContext);
            own = AuxClasses.DBClass.entObj.Owners.FirstOrDefault(x => x.Id == (int)id);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление прибора", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.Owners.Remove(own);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            own.OwnerName = txbOwnerName.Text;
            own.ContactInfo = txbContactInfo.Text;

            AuxClasses.DBClass.entObj.SaveChanges();

            MessageBox.Show("Сохранено");
        }
    }
}

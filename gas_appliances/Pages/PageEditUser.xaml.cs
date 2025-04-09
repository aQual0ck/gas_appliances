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
    /// Логика взаимодействия для PageEditUser.xaml
    /// </summary>
    public partial class PageEditUser : Page
    {
        private AuxClasses.Roles role;
        private AuxClasses.Users user;
        public PageEditUser(object item)
        {
            InitializeComponent();
            DataContext = item;

            var id = TypeDescriptor.GetProperties(DataContext)["Id"].GetValue(DataContext);
            user = AuxClasses.DBClass.entObj.Users.FirstOrDefault(x => x.Id == (int)id);

            var roleid = TypeDescriptor.GetProperties(DataContext)["RoleId"].GetValue(DataContext);
            role = AuxClasses.DBClass.entObj.Roles.FirstOrDefault(x => x.Id == (int)roleid);

            cmbRole.SelectedValuePath = "RoleName";
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.ItemsSource = AuxClasses.DBClass.entObj.Roles.ToList();
            cmbRole.SelectedValue = role.RoleName;
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.Users.Remove(user);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int roleid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbRole.SelectionBoxItem)["Id"].GetValue(cmbRole.SelectionBoxItem));

            user.FullName = txbFullName.Text;
            user.Username = txbUsername.Text;
            user.Password = txbPassword.Text;
            user.RoleId = roleid;

            AuxClasses.DBClass.entObj.SaveChanges();

            MessageBox.Show("Сохранено");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }
    }
}

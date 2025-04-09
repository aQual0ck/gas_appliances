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
    /// Логика взаимодействия для PageAddUser.xaml
    /// </summary>
    public partial class PageAddUser : Page
    {
        private AuxClasses.Users user;
        public PageAddUser()
        {
            InitializeComponent();
            cmbRole.SelectedValuePath = "RoleName";
            cmbRole.DisplayMemberPath = "RoleName";
            cmbRole.ItemsSource = AuxClasses.DBClass.entObj.Roles.ToList();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int roleid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbRole.SelectionBoxItem)["Id"].GetValue(cmbRole.SelectionBoxItem));
            user = new AuxClasses.Users()
            {
                FullName = txbFullName.Text,
                Username = txbUsername.Text,
                Password = txbPassword.Text,
                RoleId = roleid,
            };
            AuxClasses.DBClass.entObj.Users.Add(user);
            AuxClasses.DBClass.entObj.SaveChanges();
            MessageBox.Show("Добавлено");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private static readonly Regex _regex = new Regex("^[A-Za-z0-9]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
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
            if (string.IsNullOrEmpty(txbUsername.Text) || string.IsNullOrEmpty(txbPassword.Text) || string.IsNullOrEmpty(txbFullName.Text) || cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else
            {
                int roleid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbRole.SelectedItem)["Id"].GetValue(cmbRole.SelectedItem));
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
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }

        private void txbUsername_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void txbPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

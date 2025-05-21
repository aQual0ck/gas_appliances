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
    /// Interaction logic for PageAdminPanel.xaml
    /// </summary>
    public partial class PageAdminPanel : Page
    {
        private int roleid;
        public PageAdminPanel()
        {
            InitializeComponent();
            dgrUsers.ItemsSource = AuxClasses.DBClass.entObj.Users.ToList();

            cmbRole.SelectedValuePath = "RoleName";
            cmbRole.DisplayMemberPath = "RoleName";
            var role = AuxClasses.DBClass.entObj.Roles.ToList();
            role.Insert(0, new AuxClasses.Roles { Id = 0, RoleName = "Все роли" });
            cmbRole.ItemsSource = role;
            cmbRole.SelectedIndex = 0;

            if (string.IsNullOrEmpty(txbSearchUsers.Text))
            {
                txbSearchUsers.Text = "по имени";
                txbSearchUsers.Foreground = Brushes.Gray;
                txbSearchUsers.GotFocus += RemoveTextSearchUsers;
                txbSearchUsers.LostFocus += AddTextSearchUsers;
            }

            if (string.IsNullOrEmpty(txbSearchLogin.Text))
            {
                txbSearchLogin.Text = "по логину";
                txbSearchLogin.Foreground = Brushes.Gray;
                txbSearchLogin.GotFocus += RemoveTextSearchLogin;
                txbSearchLogin.LostFocus += AddTextSearchLogin;
            }
        }

        private void menuLogOut_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageLogin());
        }

        private void menuAddUser_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageAddUser());
        }

        private void txbSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void dgrUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.Navigate(new PageEditUser(dgrUsers.SelectedItem));
        }

        private void ApplyFilters()
        {
            if (cmbRole.SelectedItem != null)
                roleid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbRole.SelectedItem)["Id"].GetValue(cmbRole.SelectedItem));
            else
                roleid = 0;

            var queryUser = AuxClasses.DBClass.entObj.Users.AsQueryable();

            if (roleid != 0)
                queryUser = queryUser.Where(x => x.RoleId == roleid);

            if (!string.IsNullOrEmpty(txbSearchUsers.Text) && txbSearchUsers.Text != "по имени")
                queryUser = queryUser.Where(x => x.FullName.ToLower().Contains(txbSearchUsers.Text.ToLower()));

            if (!string.IsNullOrEmpty(txbSearchLogin.Text) && txbSearchLogin.Text != "по логину")
                queryUser = queryUser.Where(x => x.Username.ToLower().Contains(txbSearchLogin.Text.ToLower()));
            
            dgrUsers.ItemsSource = queryUser.ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void txbSearchLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void RemoveTextSearchUsers(object sender, EventArgs e)
        {
            if (txbSearchUsers.Text == "по имени")
            {
                txbSearchUsers.Text = "";
                txbSearchUsers.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchUsers(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchUsers.Text))
            {
                txbSearchUsers.Text = "по имени";
                txbSearchUsers.Foreground = Brushes.Gray;
            }
        }

        private void RemoveTextSearchLogin(object sender, EventArgs e)
        {
            if (txbSearchLogin.Text == "по логину")
            {
                txbSearchLogin.Text = "";
                txbSearchLogin.Foreground = Brushes.Black;
            }
        }

        private void AddTextSearchLogin(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txbSearchLogin.Text))
            {
                txbSearchLogin.Text = "по логину";
                txbSearchLogin.Foreground = Brushes.Gray;
            }
        }
    }
}

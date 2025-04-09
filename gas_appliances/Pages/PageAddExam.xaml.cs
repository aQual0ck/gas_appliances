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
using System.ComponentModel;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddExam.xaml
    /// </summary>
    public partial class PageAddExam : Page
    {
        AuxClasses.ApplianceCheck exam;
        public PageAddExam()
        {
            InitializeComponent();

            InitializeComponent();
            cmbAppliance.SelectedValuePath = "ApplianceName";
            cmbAppliance.DisplayMemberPath = "ApplianceName";
            cmbAppliance.ItemsSource = AuxClasses.DBClass.entObj.Appliance.ToList();

            cmbUser.SelectedValuePath = "FullName";
            cmbUser.DisplayMemberPath = "FullName";
            cmbUser.ItemsSource = AuxClasses.DBClass.entObj.Users.ToList();
        }

        private void cmbAppliance_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbAppliance.IsDropDownOpen = true;
            cmbAppliance.ItemsSource = AuxClasses.DBClass.entObj.Appliance.Where(s => s.ApplianceName.ToLower().Contains(cmbAppliance.Text.ToLower())).ToList();
        }

        private void cmbUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbUser.IsDropDownOpen = true;
            cmbUser.ItemsSource = AuxClasses.DBClass.entObj.Users.Where(s => s.FullName.ToLower().Contains(cmbUser.Text.ToLower())).ToList();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int applid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbAppliance.SelectionBoxItem)["Id"].GetValue(cmbAppliance.SelectionBoxItem));
            int userid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbUser.SelectionBoxItem)["Id"].GetValue(cmbUser.SelectionBoxItem));
            string dateExam = dpExam.SelectedDate?.ToString(App.DateFormat);
            DateTime dtExam = DateTime.Parse(dateExam);

            exam = new AuxClasses.ApplianceCheck
            {
                ApplianceId = applid,
                UserId = userid,
                CheckDate = dtExam
            };
            AuxClasses.DBClass.entObj.ApplianceCheck.Add(exam);
            AuxClasses.DBClass.entObj.SaveChanges();
            MessageBox.Show("Добавлено");
        }
    }
}

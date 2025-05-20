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
using gas_appliances.AuxClasses;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddExam.xaml
    /// </summary>
    public partial class PageAddExam : Page
    {
        private AuxClasses.ApplianceCheck exam;
        private List<Appliance> appl;
        private List<Users> user;
        public PageAddExam()
        {
            InitializeComponent();

            cmbAppliance.SelectedValuePath = "ApplianceName";
            cmbAppliance.DisplayMemberPath = "ApplianceName";
            appl = AuxClasses.DBClass.entObj.Appliance.ToList();
            cmbAppliance.ItemsSource = appl;

            cmbUser.SelectedValuePath = "FullName";
            cmbUser.DisplayMemberPath = "FullName";
            user = AuxClasses.DBClass.entObj.Users.ToList();
            cmbUser.ItemsSource = user;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAppliance.SelectedItem == null || cmbUser.SelectedItem == null || string.IsNullOrEmpty(dpExam.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else
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

        private void cmbAppliance_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbAppliance.Text;

            var filtered = appl.Where(item => item.ApplianceName.ToLower().Contains(text.ToLower())).ToList();

            cmbAppliance.ItemsSource = filtered;
            cmbAppliance.IsDropDownOpen = true;
            cmbAppliance.Text = text;

            var textBox = cmbAppliance.Template.FindName("PART_EditableTextBox", cmbAppliance) as TextBox;
            if (textBox != null)
            {
                textBox.CaretIndex = text.Length;
            }
        }

        private void cmbUser_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbUser.Text;

            var filtered = user.Where(item => item.FullName.ToLower().Contains(text.ToLower())).ToList();

            cmbUser.ItemsSource = filtered;
            cmbUser.IsDropDownOpen = true;
            cmbUser.Text = text;

            var textBox = cmbUser.Template.FindName("PART_EditableTextBox", cmbUser) as TextBox;
            if (textBox != null)
            {
                textBox.CaretIndex = text.Length;
            }
        }

        public static IEnumerable<Control> FindInputControls(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is TextBox || child is ComboBox || child is DatePicker)
                    yield return (Control)child;

                foreach (var descendant in FindInputControls(child))
                    yield return descendant;
            }
        }
    }
}

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
using System.Text.RegularExpressions;

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
        private List<string> applOriginal = new List<string>();
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageAddExam()
        {
            InitializeComponent();

            cmbAppliance.SelectedValuePath = "ApplianceName";
            cmbAppliance.DisplayMemberPath = "ApplianceName";
            appl = AuxClasses.DBClass.entObj.Appliance.ToList();
            foreach (Appliance appli in appl)
            {
                applOriginal.Add(appli.ApplianceName);
                appli.ApplianceName = appli.ApplianceName + " | " + appli.Owners.OwnerName;
            }
            cmbAppliance.ItemsSource = appl;

            cmbUser.SelectedValuePath = "FullName";
            cmbUser.DisplayMemberPath = "FullName";
            user = AuxClasses.DBClass.entObj.Users.ToList();
            cmbUser.ItemsSource = user;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (Appliance appli in appl)
            {
                appli.ApplianceName = applOriginal[id];
                id++;
            }
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
                int id = 0;
                foreach (Appliance appli in appl)
                {
                    appli.ApplianceName = applOriginal[id];
                    id++;
                }
                int applid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbAppliance.SelectedItem)["Id"].GetValue(cmbAppliance.SelectedItem));
                int userid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbUser.SelectedItem)["Id"].GetValue(cmbUser.SelectedItem));
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
                AuxClasses.FrameClass.frmObj.GoBack();
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

        private void dpExam_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

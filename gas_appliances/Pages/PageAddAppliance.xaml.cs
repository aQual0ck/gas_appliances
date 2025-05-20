using gas_appliances.AuxClasses;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для PageAddAppliance.xaml
    /// </summary>
    public partial class PageAddAppliance : Page
    {
        private AuxClasses.Appliance appl;
        private List<Owners> own;
        public PageAddAppliance()
        {
            InitializeComponent();
            cmbCategory.SelectedValuePath = "CategoryName";
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.ItemsSource = AuxClasses.DBClass.entObj.Category.ToList();

            cmbStatus.SelectedValuePath = "StatusName";
            cmbStatus.DisplayMemberPath = "StatusName";
            cmbStatus.ItemsSource = AuxClasses.DBClass.entObj.Statuses.ToList();

            cmbOwner.SelectedValuePath = "OwnerName";
            cmbOwner.DisplayMemberPath = "OwnerName";
            own = AuxClasses.DBClass.entObj.Owners.ToList();
            cmbOwner.ItemsSource = own;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool allFilled = true;

            foreach (var control in FindInputControls(this)) 
            {
                if (control is TextBox tb && string.IsNullOrWhiteSpace(tb.Text) && tb.Name != "txbNotes" && control.GetType() != typeof(DatePickerTextBox))
                    allFilled = false;
                else if (control is ComboBox cb && (cb.SelectedItem == null || cb.SelectedIndex == -1))
                    allFilled = false;

                if (allFilled == false) break;
            }

            if (allFilled == false)
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else
            {
                string dateIns = dpInstalled.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtIns = dateIns != null ? DateTime.Parse(dateIns) : (DateTime?)null;
                string dateNext = dpNextExam.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtNext = dateNext != null ? DateTime.Parse(dateNext) : (DateTime?)null;
                int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectionBoxItem)["Id"].GetValue(cmbCategory.SelectionBoxItem));
                int statid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbStatus.SelectionBoxItem)["Id"].GetValue(cmbStatus.SelectionBoxItem));
                int ownid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbOwner.SelectedItem)["Id"].GetValue(cmbOwner.SelectedItem));
                appl = new AuxClasses.Appliance()
                {
                    CategoryId = catid,
                    StatusId = statid,
                    ApplianceName = txbApplianceName.Text,
                    ApplianceAddress = txbApplianceAddress.Text,
                    ApplianceOwnerId = ownid,
                    InstalledSince = dtIns,
                    NextExamination = dtNext,
                    SerialNumber = txbSN.Text,
                    Notes = txbNotes.Text
                };
                AuxClasses.DBClass.entObj.Appliance.Add(appl);
                AuxClasses.DBClass.entObj.SaveChanges();
                MessageBox.Show("Добавлено");
            }
        }

        private void cmbOwner_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbOwner.Text;

            var filtered = own.Where(item => item.OwnerName.ToLower().Contains(text.ToLower())).ToList();

            cmbOwner.ItemsSource = filtered;
            cmbOwner.IsDropDownOpen = true;
            cmbOwner.Text = text;

            var textBox = cmbOwner.Template.FindName("PART_EditableTextBox", cmbOwner) as TextBox;
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

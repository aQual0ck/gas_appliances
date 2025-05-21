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
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageEditAppliance.xaml
    /// </summary>
    public partial class PageEditAppliance : Page
    {
        private AuxClasses.Appliance appl;
        private AuxClasses.Category cat;
        private AuxClasses.Statuses stat;
        private AuxClasses.Owners own;
        private List<Owners> ownList;
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageEditAppliance(object item)
        {
            InitializeComponent();
            DataContext = item;

            var id = TypeDescriptor.GetProperties(DataContext)["Id"].GetValue(DataContext);
            appl = AuxClasses.DBClass.entObj.Appliance.FirstOrDefault(x => x.Id == (int)id);

            var catid = TypeDescriptor.GetProperties(DataContext)["CategoryId"].GetValue(DataContext);
            cat = AuxClasses.DBClass.entObj.Category.FirstOrDefault(x => x.Id == (int)catid);

            var statid = TypeDescriptor.GetProperties(DataContext)["StatusId"].GetValue(DataContext);
            stat = AuxClasses.DBClass.entObj.Statuses.FirstOrDefault(x => x.Id == (int)statid);

            var ownid = TypeDescriptor.GetProperties(DataContext)["ApplianceOwnerId"].GetValue(DataContext);
            own = AuxClasses.DBClass.entObj.Owners.FirstOrDefault(x => x.Id == (int)ownid);

            cmbCategory.SelectedValuePath = "CategoryName";
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.ItemsSource = AuxClasses.DBClass.entObj.Category.ToList();
            cmbCategory.SelectedValue = cat.CategoryName;

            cmbStatus.SelectedValuePath = "StatusName";
            cmbStatus.DisplayMemberPath = "StatusName";
            cmbStatus.ItemsSource = AuxClasses.DBClass.entObj.Statuses.ToList();
            cmbStatus.SelectedValue = stat.StatusName;

            cmbOwner.SelectedValuePath = "OwnerName";
            cmbOwner.DisplayMemberPath = "OwnerName";
            ownList = AuxClasses.DBClass.entObj.Owners.ToList();
            cmbOwner.ItemsSource = ownList;
            cmbOwner.SelectedItem = AuxClasses.DBClass.entObj.Owners.FirstOrDefault(x => x.Id == own.Id);
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
                int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectedItem)["Id"].GetValue(cmbCategory.SelectedItem));
                int statid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbStatus.SelectedItem)["Id"].GetValue(cmbStatus.SelectedItem));
                int ownid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbOwner.SelectedItem)["Id"].GetValue(cmbOwner.SelectedItem));
                string dateIns = dpInstalled.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtIns = dateIns != null ? DateTime.Parse(dateIns) : (DateTime?)null;
                string dateNext = dpNextExam.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtNext = dateNext != null ? DateTime.Parse(dateNext) : (DateTime?)null;

                appl.CategoryId = catid;
                appl.StatusId = statid;
                appl.ApplianceName = txbApplianceName.Text;
                appl.ApplianceAddress = txbApplianceAddress.Text;
                appl.ApplianceOwnerId = ownid;
                appl.SerialNumber = txbSN.Text;
                appl.Notes = txbNotes.Text;

                AuxClasses.DBClass.entObj.SaveChanges();
                MessageBox.Show("Сохранено");
            }
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление прибора", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.Appliance.Remove(appl);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }

        private void cmbOwner_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbOwner.Text;

            var filtered = ownList.Where(item => item.OwnerName.ToLower().Contains(text.ToLower())).ToList();

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

        private void dpInstalled_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpNextExam_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

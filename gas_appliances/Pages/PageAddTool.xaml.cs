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
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageAddTool.xaml
    /// </summary>
    public partial class PageAddTool : Page
    {
        private List<ToolManufacturer> man;
        private List<string> manOriginal = new List<string>();
        public PageAddTool()
        {
            InitializeComponent();
            cmbCategory.SelectedValuePath = "ToolCategoryName";
            cmbCategory.DisplayMemberPath = "ToolCategoryName";
            cmbCategory.ItemsSource = AuxClasses.DBClass.entObj.ToolCategory.ToList();

            cmbManufacturer.SelectedValuePath = "ManufacturerName";
            cmbManufacturer.DisplayMemberPath = "ManufacturerName";
            man = AuxClasses.DBClass.entObj.ToolManufacturer.ToList();
            foreach (AuxClasses.ToolManufacturer tm in man)
            {
                manOriginal.Add(tm.ManufacturerName);
                tm.ManufacturerName = tm.ToolManufacturerType.ManufacturerTypeName + " " + $"\"{tm.ManufacturerName}\"";
            }
            cmbManufacturer.ItemsSource = man;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (AuxClasses.ToolManufacturer tm in man)
            {
                tm.ManufacturerName = manOriginal[id];
                id++;
            }
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private AuxClasses.Tool tool;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool allFilled = true;

            foreach (var control in FindInputControls(this))
            {
                if (control is TextBox tb && string.IsNullOrWhiteSpace(tb.Text) && tb.Name != "txbNotes" && tb.Name != "txbSN" && control.GetType() != typeof(DatePickerTextBox))
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
                int id = 0;
                foreach (AuxClasses.ToolManufacturer tm in man)
                {
                    tm.ManufacturerName = manOriginal[id];
                    id++;
                }
                string dateOp = dpOperating.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtOp = dateOp != null ? DateTime.Parse(dateOp) : (DateTime?)null;
                string dateNE = dpNextExam.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtNE = dateNE != null ? DateTime.Parse(dateNE) : (DateTime?)null;
                string dateDec = dpDecom.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtDec = dateDec != null ? DateTime.Parse(dateDec) : (DateTime?)null;
                int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectionBoxItem)["Id"].GetValue(cmbCategory.SelectionBoxItem));
                int manid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbManufacturer.SelectedItem)["Id"].GetValue(cmbManufacturer.SelectedItem));

                tool = new AuxClasses.Tool()
                {
                    CategoryId = catid,
                    ModelName = txbModelName.Text,
                    OperatingSince = dtOp,
                    NextExamination = dtNE,
                    DecomissionedSince = dtDec,
                    ManufacturerId = manid,
                    SerialNumber = txbSN.Text,
                    Notes = txbNotes.Text
                };
                AuxClasses.DBClass.entObj.Tool.Add(tool);
                AuxClasses.DBClass.entObj.SaveChanges();
                MessageBox.Show("Добавлено");
            }
        }

        private void cmbManufacturer_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbManufacturer.Text;

            var filtered = man.Where(item => item.ManufacturerName.ToLower().Contains(text.ToLower())).ToList();

            cmbManufacturer.ItemsSource = filtered;
            cmbManufacturer.IsDropDownOpen = true;

            var textBox = cmbManufacturer.Template.FindName("PART_EditableTextBox", cmbManufacturer) as TextBox;
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

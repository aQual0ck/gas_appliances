using gas_appliances.AuxClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PageEditTool.xaml
    /// </summary>
    public partial class PageEditTool : Page
    {
        private AuxClasses.ToolCategory cat;
        private AuxClasses.ToolManufacturer man;
        private AuxClasses.Tool tool;
        private List<ToolManufacturer> manList;
        private List<string> manOriginal = new List<string>();
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageEditTool(object item)
        {
            InitializeComponent();
            DataContext = item;

            var id = TypeDescriptor.GetProperties(DataContext)["Id"].GetValue(DataContext);
            tool = AuxClasses.DBClass.entObj.Tool.FirstOrDefault(x => x.Id == (int)id);

            var catid = TypeDescriptor.GetProperties(DataContext)["CategoryId"].GetValue(DataContext);
            cat = AuxClasses.DBClass.entObj.ToolCategory.FirstOrDefault(x => x.Id == (int)catid);

            var manid = TypeDescriptor.GetProperties(DataContext)["ManufacturerId"].GetValue(DataContext);
            man = AuxClasses.DBClass.entObj.ToolManufacturer.FirstOrDefault(x => x.Id == (int)manid);

            cmbCategory.SelectedValuePath = "ToolCategoryName";
            cmbCategory.DisplayMemberPath = "ToolCategoryName";
            cmbCategory.ItemsSource = AuxClasses.DBClass.entObj.ToolCategory.ToList();
            cmbCategory.SelectedValue = cat.ToolCategoryName;

            cmbManufacturer.SelectedValuePath = "ManufacturerName";
            cmbManufacturer.DisplayMemberPath = "ManufacturerName";
            manList = AuxClasses.DBClass.entObj.ToolManufacturer.ToList();
            foreach (AuxClasses.ToolManufacturer tm in manList)
            {
                manOriginal.Add(tm.ManufacturerName);
                tm.ManufacturerName = tm.ToolManufacturerType.ManufacturerTypeName + " " + $"\"{tm.ManufacturerName}\"";
            }
            cmbManufacturer.ItemsSource = manList;
            cmbManufacturer.SelectedItem = AuxClasses.DBClass.entObj.ToolManufacturer.FirstOrDefault(x => x.Id == man.Id);
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (AuxClasses.ToolManufacturer tm in manList)
            {
                tm.ManufacturerName = manOriginal[id];
                id++;
            }
            if (MessageBox.Show("Вы уверены?", "Удаление прибора", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.Tool.Remove(tool);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }

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
                foreach (AuxClasses.ToolManufacturer tm in manList)
                {
                    tm.ManufacturerName = manOriginal[id];
                    id++;
                }
                int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectedItem)["Id"].GetValue(cmbCategory.SelectedItem));
                int manid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbManufacturer.SelectedItem)["Id"].GetValue(cmbManufacturer.SelectedItem));
                string dateOp = dpOperating.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtOp = dateOp != null ? DateTime.Parse(dateOp) : (DateTime?)null;
                string dateNE = dpNextExam.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtNE = dateNE != null ? DateTime.Parse(dateNE) : (DateTime?)null;
                string dateDec = dpDecom.SelectedDate?.ToString(App.DateFormat);
                DateTime? dtDec = dateDec != null ? DateTime.Parse(dateDec) : (DateTime?)null;

                tool.CategoryId = catid;
                tool.ModelName = txbModelName.Text;
                tool.OperatingSince = dtOp;
                tool.NextExamination = dtNE;
                tool.DecomissionedSince = dtDec;
                tool.ManufacturerId = manid;
                tool.SerialNumber = txbSN.Text;
                tool.Notes = txbNotes.Text;

                AuxClasses.DBClass.entObj.SaveChanges();

                MessageBox.Show("Сохранено");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (AuxClasses.ToolManufacturer tm in manList)
            {
                tm.ManufacturerName = manOriginal[id];
                id++;
            }
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void cmbManufacturer_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbManufacturer.Text;

            var filtered = AuxClasses.DBClass.entObj.ToolManufacturer.Where(item => item.ManufacturerName.ToLower().Contains(text.ToLower())).ToList();
            cmbManufacturer.ItemsSource = filtered;
            cmbManufacturer.IsDropDownOpen = true;
            cmbManufacturer.Text = text;

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

        private void dpOperating_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpNextExam_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }

        private void dpDecom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

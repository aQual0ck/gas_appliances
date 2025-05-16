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
            int id = 0;
            foreach (AuxClasses.ToolManufacturer tm in man)
            {
                tm.ManufacturerName = manOriginal[id];
                id++;
            }
            string dateOp = dpOperating.SelectedDate?.ToString(App.DateFormat);
            DateTime dtOp = DateTime.Parse(dateOp);
            string dateNE = dpNextExam.SelectedDate?.ToString(App.DateFormat);
            DateTime dtNE = DateTime.Parse(dateNE);
            string dateDec = dpDecom.SelectedDate?.ToString(App.DateFormat);
            DateTime dtDec = DateTime.Parse(dateDec);
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
    }
}

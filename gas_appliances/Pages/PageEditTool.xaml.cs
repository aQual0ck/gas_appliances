using gas_appliances.AuxClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    /// Interaction logic for PageEditTool.xaml
    /// </summary>
    public partial class PageEditTool : Page
    {
        private AuxClasses.ToolCategory cat;
        private AuxClasses.ToolManufacturer man;
        private AuxClasses.Tool tool;
        private List<ToolManufacturer> manList;
        private List<string> manOriginal = new List<string>();
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
            int id = 0;
            foreach (AuxClasses.ToolManufacturer tm in manList)
            {
                tm.ManufacturerName = manOriginal[id];
                id++;
            }
            int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectionBoxItem)["Id"].GetValue(cmbCategory.SelectionBoxItem));
            int manid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbManufacturer.SelectedItem)["Id"].GetValue(cmbManufacturer.SelectedItem));
            string dateOp = dpOperating.SelectedDate?.ToString(App.DateFormat);
            DateTime dtOp = DateTime.Parse(dateOp);
            string dateNE = dpNextExam.SelectedDate?.ToString(App.DateFormat);
            DateTime dtNE = DateTime.Parse(dateNE);
            string dateDec = dpDecom.SelectedDate?.ToString(App.DateFormat);
            DateTime dtDec = DateTime.Parse(dateDec);

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
    }
}

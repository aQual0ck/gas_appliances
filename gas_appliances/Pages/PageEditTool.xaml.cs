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
    /// Interaction logic for PageEditTool.xaml
    /// </summary>
    public partial class PageEditTool : Page
    {
        private AuxClasses.ToolCategory cat;
        private AuxClasses.ToolManufacturer man;
        private AuxClasses.Tool tool;
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
            cmbManufacturer.ItemsSource = AuxClasses.DBClass.entObj.ToolManufacturer.ToList();
            cmbManufacturer.SelectedValue = man.ManufacturerName;
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление прибора", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.Tool.Remove(tool);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectionBoxItem)["Id"].GetValue(cmbCategory.SelectionBoxItem));
            int manid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbManufacturer.SelectionBoxItem)["Id"].GetValue(cmbManufacturer.SelectionBoxItem));
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
            AuxClasses.FrameClass.frmObj.GoBack();
        }
    }
}

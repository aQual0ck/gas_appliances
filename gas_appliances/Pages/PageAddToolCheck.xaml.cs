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
    /// Interaction logic for PageAddToolCheck.xaml
    /// </summary>
    public partial class PageAddToolCheck : Page
    {
        AuxClasses.ToolCheck exam;
        public PageAddToolCheck()
        {
            InitializeComponent();
            cmbTool.SelectedValuePath = "ModelName";
            cmbTool.DisplayMemberPath = "ModelName";
            cmbTool.ItemsSource = AuxClasses.DBClass.entObj.Tool.ToList();

            cmbUser.SelectedValuePath = "FullName";
            cmbUser.DisplayMemberPath = "FullName";
            cmbUser.ItemsSource = AuxClasses.DBClass.entObj.Users.ToList();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void cmbTool_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbTool.IsDropDownOpen = true;
            cmbTool.ItemsSource = AuxClasses.DBClass.entObj.Tool.Where(s => s.ModelName.ToLower().Contains(cmbTool.Text.ToLower())).ToList();
        }

        private void cmbUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbUser.IsDropDownOpen = true;
            cmbUser.ItemsSource = AuxClasses.DBClass.entObj.Users.Where(s => s.FullName.ToLower().Contains(cmbUser.Text.ToLower())).ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int toolid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbTool.SelectionBoxItem)["Id"].GetValue(cmbTool.SelectionBoxItem));
            int userid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbUser.SelectionBoxItem)["Id"].GetValue(cmbUser.SelectionBoxItem));
            string dateExam = dpExam.SelectedDate?.ToString(App.DateFormat);
            DateTime dtExam = DateTime.Parse(dateExam);

            exam = new AuxClasses.ToolCheck
            {
                ToolId = toolid,
                UserId = userid,
                CheckDate = dtExam
            };
            AuxClasses.DBClass.entObj.ToolCheck.Add(exam);
            AuxClasses.DBClass.entObj.SaveChanges();
            MessageBox.Show("Добавлено");
        }
    }
}

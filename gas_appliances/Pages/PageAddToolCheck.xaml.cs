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

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageAddToolCheck.xaml
    /// </summary>
    public partial class PageAddToolCheck : Page
    {
        private AuxClasses.ToolCheck exam;
        private List<Tool> tool;
        private List<Users> user;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTool.SelectedItem == null || cmbUser.SelectedItem == null || string.IsNullOrEmpty(dpExam.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else
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

        private void cmbTool_KeyUp(object sender, KeyEventArgs e)
        {
            string text = cmbTool.Text;

            var filtered = tool.Where(item => item.ModelName.ToLower().Contains(text.ToLower())).ToList();

            cmbTool.ItemsSource = filtered;
            cmbTool.IsDropDownOpen = true;
            cmbTool.Text = text;

            var textBox = cmbTool.Template.FindName("PART_EditableTextBox", cmbTool) as TextBox;
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

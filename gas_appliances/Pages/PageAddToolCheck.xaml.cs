using gas_appliances.AuxClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private List<string> toolOriginal = new List<string>();
        private static readonly Regex _regex = new Regex("^[0-9.]+$");
        private static bool IsInputAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        public PageAddToolCheck()
        {
            InitializeComponent();
            cmbTool.SelectedValuePath = "ModelName";
            cmbTool.DisplayMemberPath = "ModelName";
            tool = AuxClasses.DBClass.entObj.Tool.ToList();
            foreach (Tool t in tool)
            {
                toolOriginal.Add(t.ModelName);
                t.ModelName = t.ModelName + " | " + t.SerialNumber;
            }
            cmbTool.ItemsSource = tool;

            cmbUser.SelectedValuePath = "FullName";
            cmbUser.DisplayMemberPath = "FullName";
            user = AuxClasses.DBClass.entObj.Users.ToList();
            cmbUser.ItemsSource = user;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (Tool t in tool)
            {
                t.ModelName = toolOriginal[id];
                id++;
            }
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
                int id = 0;
                foreach (Tool t in tool)
                {
                    t.ModelName = toolOriginal[id];
                    id++;
                }
                int toolid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbTool.SelectedItem)["Id"].GetValue(cmbTool.SelectedItem));
                int userid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbUser.SelectedItem)["Id"].GetValue(cmbUser.SelectedItem));
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
                AuxClasses.FrameClass.frmObj.GoBack();
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

        private void dpExam_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInputAllowed(e.Text);
        }
    }
}

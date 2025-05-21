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

namespace gas_appliances.Pages
{
    /// <summary>
    /// Interaction logic for PageAddManufacturer.xaml
    /// </summary>
    public partial class PageAddManufacturer : Page
    {
        public PageAddManufacturer()
        {
            InitializeComponent();
            cmbType.SelectedValuePath = "ManufacturerTypeName";
            cmbType.DisplayMemberPath = "ManufacturerTypeName";
            cmbType.ItemsSource = AuxClasses.DBClass.entObj.ToolManufacturerType.ToList();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private AuxClasses.ToolManufacturer tm;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txbManufacturerName.Text) || cmbType.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else
            {
                int typeid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbType.SelectedItem)["Id"].GetValue(cmbType.SelectedItem));
                tm = new AuxClasses.ToolManufacturer()
                {
                    ManufacturerName = txbManufacturerName.Text,
                    ManufacturerTypeId = typeid,
                    ContactInfo = txbContactInfo.Text,
                    RepresentativeName = txbRepName.Text
                };
                AuxClasses.DBClass.entObj.ToolManufacturer.Add(tm);
                AuxClasses.DBClass.entObj.SaveChanges();
                MessageBox.Show("Добавлено");
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }
    }
}

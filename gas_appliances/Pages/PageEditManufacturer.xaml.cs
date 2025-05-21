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
    /// Interaction logic for PageEditManufacturer.xaml
    /// </summary>
    public partial class PageEditManufacturer : Page
    {
        private AuxClasses.ToolManufacturer man;
        private AuxClasses.ToolManufacturerType type;
        public PageEditManufacturer(object item)
        {
            InitializeComponent();
            DataContext = item;

            var id = TypeDescriptor.GetProperties(DataContext)["Id"].GetValue(DataContext);
            man = AuxClasses.DBClass.entObj.ToolManufacturer.FirstOrDefault(x => x.Id == (int)id);

            var typeid = TypeDescriptor.GetProperties(DataContext)["ManufacturerTypeId"].GetValue(DataContext);
            type = AuxClasses.DBClass.entObj.ToolManufacturerType.FirstOrDefault(x => x.Id == (int)typeid);

            cmbType.SelectedValuePath = "ManufacturerTypeName";
            cmbType.DisplayMemberPath = "ManufacturerTypeName";
            cmbType.ItemsSource = AuxClasses.DBClass.entObj.ToolManufacturerType.ToList();
            cmbType.SelectedValue = type.ManufacturerTypeName;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txbManufacturerName.Text) || cmbType.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
            else
            {
                int typeid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbType.SelectedItem)["Id"].GetValue(cmbType.SelectedItem));

                man.ManufacturerName = txbManufacturerName.Text;
                man.ManufacturerTypeId = typeid;
                man.ContactInfo = txbContactInfo.Text;
                man.RepresentativeName = txbRepName.Text;

                AuxClasses.DBClass.entObj.SaveChanges();

                MessageBox.Show("Сохранено");
            }
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление прибора", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.ToolManufacturer.Remove(man);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }
    }
}

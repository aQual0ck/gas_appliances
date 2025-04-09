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
    /// Логика взаимодействия для PageEditAppliance.xaml
    /// </summary>
    public partial class PageEditAppliance : Page
    {
        private AuxClasses.Appliance appl;
        private AuxClasses.Category cat;
        private AuxClasses.Statuses stat;
        private AuxClasses.Owners own;
        public PageEditAppliance(object item)
        {
            InitializeComponent();
            DataContext = item;

            var id = TypeDescriptor.GetProperties(DataContext)["Id"].GetValue(DataContext);
            appl = AuxClasses.DBClass.entObj.Appliance.FirstOrDefault(x => x.Id == (int)id);

            var catid = TypeDescriptor.GetProperties(DataContext)["CategoryId"].GetValue(DataContext);
            cat = AuxClasses.DBClass.entObj.Category.FirstOrDefault(x => x.Id == (int)catid);

            var statid = TypeDescriptor.GetProperties(DataContext)["StatusId"].GetValue(DataContext);
            stat = AuxClasses.DBClass.entObj.Statuses.FirstOrDefault(x => x.Id == (int)statid);

            var ownid = TypeDescriptor.GetProperties(DataContext)["ApplianceOwnerId"].GetValue(DataContext);
            own = AuxClasses.DBClass.entObj.Owners.FirstOrDefault(x => x.Id == (int)ownid);

            cmbCategory.SelectedValuePath = "CategoryName";
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.ItemsSource = AuxClasses.DBClass.entObj.Category.ToList();
            cmbCategory.SelectedValue = cat.CategoryName;

            cmbStatus.SelectedValuePath = "StatusName";
            cmbStatus.DisplayMemberPath = "StatusName";
            cmbStatus.ItemsSource = AuxClasses.DBClass.entObj.Statuses.ToList();
            cmbStatus.SelectedValue = stat.StatusName;

            cmbOwner.SelectedValuePath = "OwnerName";
            cmbOwner.DisplayMemberPath = "OwnerName";
            cmbOwner.ItemsSource = AuxClasses.DBClass.entObj.Owners.ToList();
            cmbOwner.SelectedValue = own.OwnerName;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void cmbOwner_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbOwner.IsDropDownOpen = true;
            cmbOwner.ItemsSource = AuxClasses.DBClass.entObj.Owners.Where(s => s.OwnerName.ToLower().Contains(cmbOwner.Text.ToLower())).ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectionBoxItem)["Id"].GetValue(cmbCategory.SelectionBoxItem));
            int statid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbStatus.SelectionBoxItem)["Id"].GetValue(cmbStatus.SelectionBoxItem));
            int ownid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbOwner.SelectionBoxItem)["Id"].GetValue(cmbOwner.SelectionBoxItem));
            string dateIns = dpInstalled.SelectedDate?.ToString(App.DateFormat);
            DateTime dtIns = DateTime.Parse(dateIns);
            string dateNext = dpNextExam.SelectedDate?.ToString(App.DateFormat);
            DateTime dtNext = DateTime.Parse(dateNext);

            appl.CategoryId = catid;
            appl.StatusId = statid;
            appl.ApplianceName = txbApplianceName.Text;
            appl.ApplianceAddress = txbApplianceAddress.Text;
            appl.ApplianceOwnerId = ownid;
            appl.SerialNumber = txbSN.Text;
            appl.Notes = txbNotes.Text;

            AuxClasses.DBClass.entObj.SaveChanges();
            MessageBox.Show("Сохранено");
        }

        private void menuDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление прибора", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AuxClasses.DBClass.entObj.Appliance.Remove(appl);
                AuxClasses.DBClass.entObj.SaveChanges();
                AuxClasses.FrameClass.frmObj.GoBack();
            }
        }
    }
}

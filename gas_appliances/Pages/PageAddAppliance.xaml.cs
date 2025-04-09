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
    /// Логика взаимодействия для PageAddAppliance.xaml
    /// </summary>
    public partial class PageAddAppliance : Page
    {
        private AuxClasses.Appliance appl;
        public PageAddAppliance()
        {
            InitializeComponent();
            cmbCategory.SelectedValuePath = "CategoryName";
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.ItemsSource = AuxClasses.DBClass.entObj.Category.ToList();

            cmbStatus.SelectedValuePath = "StatusName";
            cmbStatus.DisplayMemberPath = "StatusName";
            cmbStatus.ItemsSource = AuxClasses.DBClass.entObj.Statuses.ToList();

            cmbOwner.SelectedValuePath = "OwnerName";
            cmbOwner.DisplayMemberPath = "OwnerName";
            cmbOwner.ItemsSource = AuxClasses.DBClass.entObj.Owners.ToList();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AuxClasses.FrameClass.frmObj.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string dateIns = dpInstalled.SelectedDate?.ToString(App.DateFormat);
            DateTime dtIns = DateTime.Parse(dateIns);
            string dateNext = dpNextExam.SelectedDate?.ToString(App.DateFormat);
            DateTime dtNext = DateTime.Parse(dateNext);
            int catid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbCategory.SelectionBoxItem)["Id"].GetValue(cmbCategory.SelectionBoxItem));
            int statid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbStatus.SelectionBoxItem)["Id"].GetValue(cmbStatus.SelectionBoxItem));
            int ownid = Convert.ToInt32(TypeDescriptor.GetProperties(cmbOwner.SelectionBoxItem)["Id"].GetValue(cmbOwner.SelectionBoxItem));
            appl = new AuxClasses.Appliance()
            {
                CategoryId = catid,
                StatusId = statid,
                ApplianceName = txbApplianceName.Text,
                ApplianceAddress = txbApplianceAddress.Text,
                ApplianceOwnerId = ownid,
                InstalledSince = dtIns,
                NextExamination = dtNext,
                SerialNumber = txbSN.Text,
                Notes = txbNotes.Text
            };
            AuxClasses.DBClass.entObj.Appliance.Add(appl);
            AuxClasses.DBClass.entObj.SaveChanges();
            MessageBox.Show("Добавлено");
        }

        private void cmbOwner_TextChanged(object sender, TextChangedEventArgs e)
        {
            cmbOwner.IsDropDownOpen = true;
            cmbOwner.ItemsSource = AuxClasses.DBClass.entObj.Owners.Where(s => s.OwnerName.ToLower().Contains(cmbOwner.Text.ToLower())).ToList();
        }
    }
}

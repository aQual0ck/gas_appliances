using gas_appliances.AuxClasses;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gas_appliances.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page
    {
        public PageUser(AuxClasses.Users user)
        {
            InitializeComponent();
            tbUsername.Text += user.Username + " | " + user.FullName;
        }
    }
}

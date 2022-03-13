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

namespace IBAN_Validation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<IBAN> IBANlist = new List<IBAN>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Validate_Click(object sender, RoutedEventArgs e) { 
            {
                IBAN_table.ItemsSource = IBANlist;
            }

            string IBAN = IBAN_input.Text;
            IBANlist.Add(new IBAN(IBAN));

            IBAN_table.Items.Refresh();
        }
    }
}

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

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            ErrorBoard.Text = "";
            if(IBANlist.Count == 0)
            {
                IBAN_table.ItemsSource = IBANlist;
            }

            string IBANstr = IBAN_input.Text;
            if (IBANstr.Length != 0)
            {
                IBANlist.Add(new IBAN(IBANstr));
                IBAN_table.Items.Refresh();
            }
            else
            {
                ErrorBoard.Text = "Please enter an IBAN number";
            }
        }

        private void Choose_file_Click(object sender, RoutedEventArgs e)
        {
            ErrorBoard.Text = "";
            if (IBANlist.Count == 0)
            {
                IBAN_table.ItemsSource = IBANlist;
            }
            string filePath = getFilePath();

            if(filePath == "")
            {
                ErrorBoard.Text = "No file selected";
            }
            else
            {
                foreach (string line in System.IO.File.ReadLines(filePath))
                {
                    if(line.Length != 0)
                    {
                        IBANlist.Add(new IBAN(line));
                    }
                }
                IBAN_table.Items.Refresh();
            }
        }
        private string getFilePath()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            return dialog.FileName;
        }
    }
}

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
using CrypterBibi;
using Microsoft.Win32;

namespace CrypterUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string strOrigDatei;
        private Crypter _crypter;

        public MainWindow()
        {
            InitializeComponent();
            _crypter = new Crypter();
        }

       

        private void Verschluesseln_Click(object sender, RoutedEventArgs e)
        {
            _crypter.EncryptFile(strPass.Text,strOrigDatei);
        }

        private void btnEnschlüsseln_Click(object sender, RoutedEventArgs e)
        {
            _crypter.DecryptFile(strPass.Text,strOrigDatei);
        }

        private void btnDateiOeffnen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                strOrigDatei = openFileDialog.FileName;
                strDateiVerzeichniss.Text = strOrigDatei;
               
            }
        }
    }
}

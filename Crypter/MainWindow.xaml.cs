using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using Microsoft.Win32;
using Path = System.IO.Path;

namespace Crypter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string strOrigDatei;
        private string strEncryptDatei;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EncryptFile(strOrigDatei,strEncryptDatei);
        }
        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"12"; // Your Key Here
                                                     

                /*--------------Passwort in Bytes umwandeln-----------*/

                byte[] _salt = Convert.FromBase64String("saltsaltsalt");
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, _salt);
                var Key = pdb.GetBytes(32);
                var IV = pdb.GetBytes(16);

                

                //------ Ausgangsdatei erzeugen

               // FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);
               

                //------------ Den Crypter erzeugen
                RijndaelManaged RMCrypto = new RijndaelManaged();

                RMCrypto.IV = IV;
                RMCrypto.Key = Key;
               var crypter= RMCrypto.CreateEncryptor(RMCrypto.Key, RMCrypto.IV);


                string neuerPfad =Path.GetDirectoryName(inputFile)+"\\"+ base64Encode(Path.GetFileName(inputFile));
                
                              
                FileStream fsCrypt = new FileStream(neuerPfad, FileMode.Create);

                CryptoStream cs = new CryptoStream(fsCrypt,crypter,CryptoStreamMode.Write);
                
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
               
            }
            catch(Exception e)
            {
                MessageBox.Show($"Encryption failed! \n {e.Message}", "Error");
            }
        }

        public string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        public string base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                 
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        private void DecryptFile(string inputFile, string outputFile)
        {

            {
                string password = @"12"; // Your Key Here
          
                byte[] _salt = Convert.FromBase64String("saltsaltsalt");
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, _salt);
                var Key = pdb.GetBytes(32);
                var IV = pdb.GetBytes(16);

                string fileName = Path.GetDirectoryName(inputFile) + "\\" + base64Decode(Path.GetFileName(inputFile));
                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.IV = IV;
                RMCrypto.Key = Key;
                CryptoStream cs = new CryptoStream(fsCrypt,RMCrypto.CreateDecryptor(),CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(fileName, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DecryptFile(strOrigDatei,strOrigDatei+"decrypt");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                 strOrigDatei = openFileDialog.FileName;
                strDateiVerzeichniss.Text = strOrigDatei;
                strEncryptDatei = strOrigDatei;
            }

         
            


        }
    }
}

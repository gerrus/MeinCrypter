using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrypterBibi
{
    public class Crypter
    {
        private byte[] _key;
        private byte[] _iv;
        private string NurDateiName;

       
        void KeyundIVGenerator(string password, string salt)
        {
            string saltneu = salt.Substring(0, 12);
            byte[] _salt = Convert.FromBase64String(saltneu);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, _salt);
            _key = pdb.GetBytes(32);
            _iv = pdb.GetBytes(16);
        }

        public void EncryptFile(string password, string inputFile)
        {
            try
            {
                /*--------------Passwort in Bytes umwandeln-----------*/
                string verschlüsselterDateiName = DateiNameEncrypter.FileNameEncoder(NurDateiNameGeben(inputFile));

                KeyundIVGenerator(password, verschlüsselterDateiName);


              


                //------------ Den Crypter erzeugen
                var crypter = MakeCryptor();

                
                using ( FileStream fsCrypt = new FileStream(NeuerPfad(inputFile, verschlüsselterDateiName), FileMode.Create)   )
                {
                    using (CryptoStream cs = new CryptoStream(fsCrypt, crypter, CryptoStreamMode.Write))
                    {
                        using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                        {
                            int data;
                            while ((data = fsIn.ReadByte()) != -1)
                                cs.WriteByte((byte) data);
                        }
                    }
                }
                FileLöschen(inputFile);
            }
            catch (Exception e)
            {
             //   MessageBox.Show($"Encryption failed! \n {e.Message}", "Error");
            }
        }
        public void DecryptFile(string password,string inputFile)
        {
            try
            {

                string entschlüsselterDateiName = DateiNameEncrypter.FileNameDecoder(NurDateiNameGeben(inputFile));
                KeyundIVGenerator(password, NurDateiNameGeben(inputFile));
                // string fileName = Path.GetDirectoryName(inputFile) + "\\" + base64Decode(Path.GetFileName(inputFile));

                var crypter = MakeCryptor();

                using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fsCrypt, crypter, CryptoStreamMode.Read))
                    {
                        using (FileStream fsOut = new FileStream(NeuerPfad(inputFile, entschlüsselterDateiName), FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                                fsOut.WriteByte((byte)data);
                        }
                    }
                }

                FileLöschen(inputFile);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private string NeuerPfad(string inputFile, string verschlüsselterDateiName)
        {
            return Path.GetDirectoryName(inputFile) + "\\" + verschlüsselterDateiName;
        }

        string NurDateiNameGeben(string inputFile)
        {
            return Path.GetFileName(inputFile);
        }
        private ICryptoTransform MakeCryptor()
        {
            var RMCrypto = new RijndaelManaged
            {
                IV = _iv,
                Key = _key
            };

            var crypter = RMCrypto.CreateEncryptor(RMCrypto.Key, RMCrypto.IV);
            return crypter;
        }

        void FileLöschen(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}
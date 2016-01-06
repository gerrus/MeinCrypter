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

        void KeyundIVGenerator(string password, string salt)
        {
            byte[] _salt = Convert.FromBase64String(salt);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, _salt);
            _key = pdb.GetBytes(32);
            _iv = pdb.GetBytes(16);
        }

        public void EncryptFile(string password, string inputFile, string outputFile)
        {
            try
            {
                /*--------------Passwort in Bytes umwandeln-----------*/
                string verschlüsselterDateiName = DateiNameEncrypter.FileNameEncoder(inputFile);

                KeyundIVGenerator(password, verschlüsselterDateiName);


                //------ Ausgangsdatei erzeugen

                // FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);


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
            }
            catch (Exception e)
            {
                // MessageBox.Show($"Encryption failed! \n {e.Message}", "Error");
            }
        }
        public void DecryptFile(string inputFile, string outputFile)
        {

            {
               

                byte[] _salt = Convert.FromBase64String("saltsaltsalt");
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, _salt);
                var Key = pdb.GetBytes(32);
                var IV = pdb.GetBytes(16);

                string fileName = Path.GetDirectoryName(inputFile) + "\\" + base64Decode(Path.GetFileName(inputFile));
                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();
                RMCrypto.IV = IV;
                RMCrypto.Key = Key;
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(), CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(fileName, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        private string NeuerPfad(string inputFile, string verschlüsselterDateiName)
        {
            return Path.GetDirectoryName(inputFile) + "\\" + verschlüsselterDateiName;
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
    }
}
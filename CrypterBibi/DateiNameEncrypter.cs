using System;
using System.Text;

namespace CrypterBibi
{
      static class DateiNameEncrypter
    {
        
        /// <summary>
        ///  Entkodiert einen String , in dem Fall den Dateinamen
        /// </summary>
        /// <param name="fileName">Der zu verschlüsselnde Dateiname</param>
        /// <returns>Der verschlüsselte string</returns>
        public static string FileNameEncoder(string fileName)
        {
            try
            {
                byte[] encData_byte = new byte[fileName.Length];
                encData_byte = Encoding.UTF8.GetBytes(fileName);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in FileNameEncoder" + e.Message);
            }
        }

        /// <summary>
        /// Dekodiert einen String , in dem Fall den Dateinamen
        /// </summary>
        /// <param name="fileName">Der zu entschlüsselnde String</param>
        /// <returns>Der Dateiname</returns>
        public static string FileNameDecoder(string fileName)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(fileName);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in FileNameDecoder" + e.Message);
            }
        }
       
    }
}
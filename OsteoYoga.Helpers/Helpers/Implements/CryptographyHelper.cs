using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using OsteoYoga.Helper.Helpers.Interfaces;

namespace OsteoYoga.Helper.Helpers.Implements
{
    public class CryptographyHelper : ICryptographyHelper
    {
        private byte[] IV
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                byte[] tmpResult = ReadFully(assembly.GetManifestResourceStream("OsteoYoga.Helper.FileResources.Encrypt.IV"));
                return tmpResult.Skip(3).ToArray(); // Suppression de l'entete du fichier txt
            }
        }
        private byte[] Key
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                byte[] tmpResult = ReadFully(assembly.GetManifestResourceStream("OsteoYoga.Helper.FileResources.Encrypt.Key"));
                return tmpResult.Skip(3).ToArray(); // Suppression de l'entete du fichier txt
            }
        }


        public virtual string Encrypt(string password)
        {
            byte[] textAsByte = Encoding.Default.GetBytes(password);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();


            var encryptor = tdes.CreateEncryptor(Key, IV);

            byte[] cryptedTextAsByte = encryptor.TransformFinalBlock(textAsByte, 0, textAsByte.Length);

            return GetString(cryptedTextAsByte);
        }

        public virtual string Decrypt(string encryptPassword)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            var decryptor = tdes.CreateDecryptor(Key, IV);


            byte[] decryptedTextAsByte = decryptor.TransformFinalBlock(GetBytes(encryptPassword), 0, GetBytes(encryptPassword).Length);

            return Encoding.Default.GetString(decryptedTextAsByte);
        }
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}

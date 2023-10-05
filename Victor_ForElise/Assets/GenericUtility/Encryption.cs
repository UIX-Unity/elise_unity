using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

namespace Encryption
{
    public static class AESCryptor
    {
        public static string Encrypt(string inputString, string sKey)
        {
            if (string.IsNullOrEmpty(inputString) || string.IsNullOrEmpty(sKey))
            {
                return null;
            }

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            byte[] pwdBytes = Encoding.UTF8.GetBytes(sKey);
            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(inputString);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }

        public static string Decrypt(string inputString, string sKey)
        {
            if(string.IsNullOrEmpty(inputString) || string.IsNullOrEmpty(sKey))
            {
                return null;
            }

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(inputString);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(sKey);
            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;

            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }
    }

    public class DESCryptor
    {
        public static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream crytostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
            
            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            crytostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            crytostream.Close();
            fsEncrypted.Close();
            fsInput.Close();
        }

        public static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
            cryptostreamDecr.Close();
            fsread.Close();
        }

        public static bool TryReadDecryptFile(string sInputFilename, ref XmlDocument xmlDoc, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            xmlDoc.Load(cryptostreamDecr);

            cryptostreamDecr.Close();
            fsread.Close();

            return true;
        }

        public static void WriteEncryptFile(XmlDocument xmlDoc, string sOutputFilename, string sKey)
        {
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream crytostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
            xmlDoc.Save(crytostream);

            crytostream.Close();
            fsEncrypted.Close();
        }

        public static bool TryReadDecryptFile(string sInputFilename, string sKey, out string result)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            result = new StreamReader(cryptostreamDecr).ReadToEnd();

            cryptostreamDecr.Close();
            fsread.Close();

            return !string.IsNullOrEmpty(result);
        }

        public static string ReadDecryptFile(string sInputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            string result = new StreamReader(cryptostreamDecr).ReadToEnd();

            cryptostreamDecr.Close();
            fsread.Close();

            return result;
        }

        public static string ReadDecryptFile(TextAsset sInputTextAsset, string sKey)
        {
            MemoryStream mStrm = new MemoryStream(sInputTextAsset.bytes);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            CryptoStream cryptostreamDecr = new CryptoStream(mStrm, desdecrypt, CryptoStreamMode.Read);
            string result = new StreamReader(cryptostreamDecr).ReadToEnd();

            cryptostreamDecr.Close();
            mStrm.Close();

            return result;
        }
    }
}
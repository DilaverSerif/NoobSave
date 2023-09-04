using System;
using System.Text;

namespace NoobSave
{
    public static class NoobSaveCrypter
    {
        public static string EncryptJson(string jsonData, string encryptionKey)
        {
            byte[] jsonDataBytes = Encoding.UTF8.GetBytes(jsonData);
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            for (int i = 0; i < jsonDataBytes.Length; i++)
                jsonDataBytes[i] = (byte)(jsonDataBytes[i] ^ keyBytes[i % keyBytes.Length]);
            
            string encryptedJson = Convert.ToBase64String(jsonDataBytes);
            return encryptedJson;
        }

        public static string DecryptJson(string encryptedJson, string encryptionKey)
        {
            byte[] encryptedDataBytes = Convert.FromBase64String(encryptedJson);
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            for (int i = 0; i < encryptedDataBytes.Length; i++)
                encryptedDataBytes[i] = (byte)(encryptedDataBytes[i] ^ keyBytes[i % keyBytes.Length]);
            
            var decryptedJson = Encoding.UTF8.GetString(encryptedDataBytes);
            return decryptedJson;
        }
    }
}
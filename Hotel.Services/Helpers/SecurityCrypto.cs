using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Hotel.Services.Helpers
{
    public static class SecurityCrypto
    {
        /// <summary>
        /// Serialize and encrypt an object
        /// </summary>
        /// <param name="pObject">Object to be encrypted</param>
        /// <param name="pPass">password to encrypt the object</param>
        /// <returns>returns the result string of the ecnrypted object</returns>
        public static string SerializeAndEncrypt(Object pObject, string pPass)
        {
            Stream stream = SerializeObject(pObject);
            return Encrypt(stream, pPass);
        }

        /// <summary>
        /// Deserialize and deencrypt an object
        /// </summary>
        /// <param name="pData">data to be deserialized and decrypted</param>
        /// <param name="pType">type of object to be created</param>
        /// <param name="pPass">password to decrypt the object</param>
        /// <returns>returns an object</returns>
        public static Object DecryptAndDeseralize(string pData, Type pType, string pPass)
        {
            string data = Decrypt(pData, pPass);
            return DeserializeObject(data, pType);
        }

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="pObject">Object to be serialized</param>
        /// <returns>returns a stream of the serialized object</returns>
        private static Stream SerializeObject(Object pObject)
        {
            var stream = new MemoryStream();

            var xmlSerializer = new XmlSerializer(pObject.GetType());

            xmlSerializer.Serialize(stream, pObject);

            return stream;
        }

        /// <summary>
        /// Deserialize an Object
        /// </summary>
        /// <param name="pData"> data to be deserialised</param>
        /// <param name="pType"> type of object that will be created</param>
        /// <returns>created object</returns>
        private static Object DeserializeObject(string pData, Type pType)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                // serialise to object
                var serializer = new XmlSerializer(pType);
                stream = new StringReader(pData); // read xml data
                reader = new XmlTextReader(stream); // create reader
                // covert reader to object
                return (object) serializer.Deserialize(reader);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
                //if (stream != null) stream.Close();
            }
        }

        /// <summary>
        /// Encrypts a stream data
        /// </summary>
        /// <param name="pStream"> stream data to be encrypted</param>
        /// <param name="pPassphrase">password to encrypt the object</param>
        /// <returns>resulting string of the encrypted data</returns>
        private static string Encrypt(Stream pStream, string pPassphrase)
        {
            byte[] results;
            var UTF8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            var tDESKey = hashProvider.ComputeHash(UTF8.GetBytes(pPassphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            var tDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            tDESAlgorithm.Key = tDESKey;
            tDESAlgorithm.Mode = CipherMode.ECB;
            tDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            // byte[] DataToEncrypt = UTF8.GetBytes(Message);
            var dataToEncrypt = new byte[pStream.Length];
            pStream.Position = 0;
            pStream.Read(dataToEncrypt, 0, (int) pStream.Length);


            // Step 5. Attempt to encrypt the string
            try
            {
                var Encryptor = tDESAlgorithm.CreateEncryptor();
                results = Encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tDESAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(results);
        }


        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="pMessage">string to be decrypted</param>
        /// <param name="pPassphrase">Password to decrypt the object</param>
        /// <returns>resulting string of the decrypted string</returns>
        private static string Decrypt(string pMessage, string pPassphrase)
        {
            byte[] results;
            var UTF8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            var TDESKey = hashProvider.ComputeHash(UTF8.GetBytes(pPassphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider tDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            tDESAlgorithm.Key = TDESKey;
            tDESAlgorithm.Mode = CipherMode.ECB;
            tDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            var DataToDecrypt = Convert.FromBase64String(pMessage);

            // Step 5. Attempt to decrypt the string
            try
            {
                var Decryptor = tDESAlgorithm.CreateDecryptor();
                results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tDESAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(results);
        }
    }
}
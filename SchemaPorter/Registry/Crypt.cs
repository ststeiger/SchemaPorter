
namespace Microsoft.Extensions.Configuration.Registry.Crypto 
{

    public class CryptTest
    {
        public static void Test()
        {
            string input = "hello world äöü Привет мир";
            // input = "";
            // input = null;

            string cryptDES3 = Microsoft.Extensions.Configuration.Registry.Crypto.DES.Encrypt(input);
            string plainDES3 = Microsoft.Extensions.Configuration.Registry.Crypto.DES.Decrypt(cryptDES3);

            string cryptAES = Microsoft.Extensions.Configuration.Registry.Crypto.AES.Encrypt(input);
            string plainAES = Microsoft.Extensions.Configuration.Registry.Crypto.AES.Decrypt(cryptAES);

            System.Console.WriteLine(plainDES3);
            System.Console.WriteLine(plainAES);
        } // End Sub Test 

    } // End Class CryptTest 


    public class AES
    {
        protected static string s_key = "1b55ec1d96f637aa7b73c31765a12c2c8fb8b9f6ae8b14396475a20ed1a83dac";
        protected static string s_IV = "d4e3381cdd39ddb70f85e96d11b667e5";


        public static string GetKey()
        {
            return s_key;
        } // End Sub GetKey


        public static string GetIV()
        {
            return s_IV;
        } // End Sub GetIV


        public static void SetKey(ref string strInputKey)
        {
            s_key = strInputKey;
        } // End Sub SetKey 


        public static void SetIV(ref string strInputIV)
        {
            s_IV = strInputIV;
        } // End Sub SetIV


        public static string GenerateKey()
        {
            string retValue = null;

            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();

                byte[] bIV = aes.IV;
                byte[] bKey = aes.Key;
                aes.Clear();

                retValue= "IV: " + ByteArrayToHexString(bIV) + System.Environment.NewLine + "Key: " + ByteArrayToHexString(bKey);

                System.Array.Clear(bIV, 0, bIV.Length);
                bIV = null;

                System.Array.Clear(bKey, 0, bKey.Length);
                bKey = null;
            } // End Using aes 

            return retValue;
        } // End Function GenerateKey



        public static string Encrypt(string plainText)
        {
            string retValue = null;
            if (plainText == null)
                return null;

            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                // Create a new key and initialization vector.
                // aes.GenerateKey();
                // aes.GenerateIV();
                aes.Key = HexStringToByteArray(s_key);
                aes.IV = HexStringToByteArray(s_IV);


                // Get the key and initialization vector.
                byte[] encryptionKey = aes.Key;
                byte[] initializationVector = aes.IV;
                // strKey = ByteArrayToHexString(encryptionKey)
                // strIV = ByteArrayToHexString(initializationVector)

                // Get an encryptor.
                using (System.Security.Cryptography.ICryptoTransform aesEncryptor = aes.CreateEncryptor(encryptionKey, initializationVector))
                {
                    //Encrypt the data.
                    using (System.IO.MemoryStream msEncrypt = new System.IO.MemoryStream())
                    {
                        using (System.Security.Cryptography.CryptoStream csEncrypt = new System.Security.Cryptography.CryptoStream(msEncrypt, aesEncryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                        {
                            //Convert the data to a byte array.
                            byte[] plainTextBuffer = enc.GetBytes(plainText);

                            //Write all data to the crypto stream and flush it.
                            csEncrypt.Write(plainTextBuffer, 0, plainTextBuffer.Length);
                            csEncrypt.FlushFinalBlock();

                            //Get encrypted array of bytes.
                            byte[] cipherTextBuffer = msEncrypt.ToArray();
                            retValue = ByteArrayToHexString(cipherTextBuffer);
                            System.Array.Clear(cipherTextBuffer, 0, cipherTextBuffer.Length);
                            cipherTextBuffer = null;

                            System.Array.Clear(plainTextBuffer, 0, plainTextBuffer.Length);
                            plainTextBuffer = null;

                        } // End Using csEncrypt 

                    } // End Using msEncrypt 

                } // End Using aesEncryptor 

                System.Array.Clear(encryptionKey, 0, encryptionKey.Length);
                encryptionKey = null;

                System.Array.Clear(initializationVector, 0, initializationVector.Length);
                initializationVector = null;
            } // End Using aes 

            return retValue;
        } // End Function Encrypt


        public static string Decrypt(string encryptedInput)
        {
            string returnValue = null;

            if (string.IsNullOrEmpty(encryptedInput))
                return encryptedInput;

            // System.Text.Encoding enc = System.Text.Encoding.ASCII;
            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            byte[] cipherTextBuffer = HexStringToByteArray(encryptedInput);
            byte[] decryptionKey = HexStringToByteArray(s_key);
            byte[] initializationVector = HexStringToByteArray(s_IV);

            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {

                // This is where the message would be transmitted to a recipient
                // who already knows your secret key. Optionally, you can
                // also encrypt your secret key using a public key algorithm
                // and pass it to the mesage recipient along with the RijnDael
                // encrypted message.            
                //Get a decryptor that uses the same key and IV as the encryptor.
                using (System.Security.Cryptography.ICryptoTransform aesDecryptor = aes.CreateDecryptor(decryptionKey, initializationVector))
                {
                    //Now decrypt the previously encrypted message using the decryptor
                    // obtained in the above step.
                    using (System.IO.MemoryStream msDecrypt = new System.IO.MemoryStream(cipherTextBuffer))
                    {
                        using (System.Security.Cryptography.CryptoStream csDecrypt = new System.Security.Cryptography.CryptoStream(msDecrypt, aesDecryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                        {

                            //Dim baPlainTextBuffer() As Byte
                            //baPlainTextBuffer = New Byte(baCipherTextBuffer.Length) {}
                            byte[] baPlainTextBuffer = new byte[cipherTextBuffer.Length + 1];

                            //Read the data out of the crypto stream.
                            csDecrypt.Read(baPlainTextBuffer, 0, baPlainTextBuffer.Length);

                            //Convert the byte array back into a string.
                            returnValue = enc.GetString(baPlainTextBuffer);
                            if (!string.IsNullOrEmpty(returnValue))
                                returnValue = returnValue.Trim('\0');
                        } // End Using csDecrypt 

                    } // End Using msDecrypt 

                } // End Using aesDecryptor 

            } // End Using aes 

            System.Array.Clear(cipherTextBuffer, 0, cipherTextBuffer.Length);
            cipherTextBuffer = null;

            System.Array.Clear(decryptionKey, 0, decryptionKey.Length);
            decryptionKey = null;

            System.Array.Clear(initializationVector, 0, initializationVector.Length);
            initializationVector = null;

            return returnValue;
        } // End Function DeCrypt


        // VB.NET to convert a byte array into a hex string
        public static string ByteArrayToHexString(byte[] input)
        {
            string retValue = null;

            System.Text.StringBuilder output = new System.Text.StringBuilder(input.Length);

            for (int i = 0; i <= input.Length - 1; i++)
            {
                output.Append(input[i].ToString("X2"));
            } // Next i 

            retValue = output.ToString().ToLower();
            output.Clear();
            output = null;

            return retValue;
        } // End Function ByteArrayToHexString


        public static byte[] HexStringToByteArray(string hexString)
        {
            int numChars = hexString.Length;
            byte[] buffer = new byte[numChars / 2];

            for (int i = 0; i <= numChars - 1; i += 2)
            {
                buffer[i / 2] = System.Convert.ToByte(hexString.Substring(i, 2), 16);
            } // Next i 

            return buffer;
        } // End Function HexStringToByteArray

    } // End Class AES 


    public class DES
    {


        protected static string s_symmetricKey = "z67f3GHhdga78g3gZUIT(6/&ns289hsB_5Tzu6";
        //Protected Shared strSymmetricKey As String = "Als symmetrischer Key kann irgendein Text verwendet werden. äöü'"

        // http://www.codeproject.com/KB/aspnet/ASPNET_20_Webconfig.aspx
        // http://www.codeproject.com/KB/database/Connection_Strings.aspx
        public static string Decrypt(string sourceText)
        {
            string returnValue = "";

            if (sourceText == null)
                return sourceText;

            if (string.IsNullOrEmpty(sourceText))
                return returnValue;


            using (System.Security.Cryptography.TripleDES Des = System.Security.Cryptography.TripleDES.Create())
            {

                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    Des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s_symmetricKey));
                    Des.Mode = System.Security.Cryptography.CipherMode.ECB;

                    using (System.Security.Cryptography.ICryptoTransform desDecryptor = Des.CreateDecryptor())
                    {
                        byte[] buff = System.Convert.FromBase64String(sourceText);
                        returnValue = System.Text.Encoding.UTF8.GetString(desDecryptor.TransformFinalBlock(buff, 0, buff.Length));

                        System.Array.Clear(buff, 0, buff.Length);
                        buff = null;
                    } // End Using desDecryptor 

                } // End Using md5

            } // End Using Des

            return returnValue;
        } // End Function DeCrypt


        public static string Encrypt(string sourceText)
        {
            string returnValue = "";

            if (sourceText == null)
                return null;

            using (System.Security.Cryptography.TripleDES des3 = System.Security.Cryptography.TripleDES.Create())
            {

                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    des3.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s_symmetricKey));
                    des3.Mode = System.Security.Cryptography.CipherMode.ECB;
                    using (System.Security.Cryptography.ICryptoTransform desEncryptor = des3.CreateEncryptor())
                    {
                        byte[] buff = System.Text.Encoding.UTF8.GetBytes(sourceText);
                        returnValue = System.Convert.ToBase64String(desEncryptor.TransformFinalBlock(buff, 0, buff.Length));

                        System.Array.Clear(buff, 0, buff.Length);
                        buff = null;
                    } // End Using desEncryptor 

                } // End Using HashMD5

            } // End Using des3

            return returnValue;
        } // End Function Crypt


        public static string GenerateKey()
        {
            string returnValue = null;

            using (System.Security.Cryptography.TripleDES des3 = System.Security.Cryptography.TripleDES.Create())
            {
                des3.GenerateKey();
                des3.GenerateIV();
                byte[] bIV = des3.IV;
                byte[] bKey = des3.Key;

                returnValue = "IV: " + AES.ByteArrayToHexString(bIV) + System.Environment.NewLine + "Key: " + AES.ByteArrayToHexString(bKey);

                System.Array.Clear(bIV, 0, bIV.Length);
                bIV = null;

                System.Array.Clear(bKey, 0, bKey.Length);
                bKey = null;
            }

            return returnValue;
        } // End Function GenerateKey


        public static string GenerateHash(string sourceText)
        {
            string returnValue = "";
            byte[] textSourceBytes = System.Text.Encoding.UTF8.GetBytes(sourceText);

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hash = md5.ComputeHash(textSourceBytes);
                returnValue = System.Convert.ToBase64String(hash);

                System.Array.Clear(hash,0, hash.Length);
                hash = null;
            } // End Using Md5

            System.Array.Clear(textSourceBytes, 0, textSourceBytes.Length);
            textSourceBytes = null;

            return returnValue;
        } // End Function GenerateHash


    } // End Class DES


} // End Namespace Microsoft.Extensions.Configuration.Registry.Crypto 

    using System.Text;
    using System.Security.Cryptography;
    using System.IO;
    using System;

    namespace SaveSystem.Utilities
    {
    /// <summary>
    /// Provides encryption and decryption utilities for save files.
    /// </summary>
    internal sealed class EncryptorUtility
    {
        private const int KEY_LENGTH = 32;
        private const int IV_LENGTH = 16;

            /// <summary>
            /// Encrypts the provided byte array using AES encryption with the given key.
            /// </summary>
            /// <param name="data">The data to encrypt.</param>
            /// <param name="encryptKey">The encryption key (32 characters long).</param>
            /// <returns>The encrypted byte array.</returns>
                internal static byte[] EncryptFile(byte[] data, string encryptKey)
                {
                    byte[] key = Encoding.UTF8.GetBytes(encryptKey);
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.GenerateIV();
                        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                        using (var ms = new MemoryStream())
                        {
                            ms.Write(aes.IV, 0, aes.IV.Length);
                            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                using (var sw = new StreamWriter(cs))
                                {
                                    sw.Write(data);
                                }
                            }
                            return ms.ToArray();
                        }
                    }
                }
        /// <summary>
        /// Decrypts the provided byte array using AES decryption with the given key.
        /// </summary>
        /// <param name="data">The encrypted data to decrypt.</param>
        /// <param name="encryptKey">The encryption key (32 characters long).</param>
        /// <returns>The decrypted byte array.</returns>
        /// <exception cref="Exception">Thrown if the provided data is invalid or improperly formatted.</exception>
        internal static byte[] DecryptFile(byte[] data, string encryptKey)
                {
                    if (data.Length < IV_LENGTH)
                    {
                        throw new Exception("Wrong Lengh of Encrypted File");
                    }

                    byte[] key = Encoding.UTF8.GetBytes(encryptKey);
                    byte[] iv = new byte[IV_LENGTH];
                    byte[] chipher = new byte[data.Length - IV_LENGTH];

                    Array.Copy(data, iv, iv.Length);
                    Array.Copy(data, IV_LENGTH, chipher, 0, chipher.Length);

                    using (var aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;
                        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                        using var ms = new MemoryStream(chipher);
                        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                        using var sr = new StreamReader(cs);
                        return ms.ToArray();
                    }
                }
                internal static string GenerateKey()
                {
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        byte[] randomBytes = new byte[KEY_LENGTH];
                        rng.GetBytes(randomBytes);

                        return Convert.ToBase64String(randomBytes).Substring(0, KEY_LENGTH);
                    }
                }
        }
    }

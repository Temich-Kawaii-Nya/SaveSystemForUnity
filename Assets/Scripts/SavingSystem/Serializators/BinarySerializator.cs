using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SaveSystem.Utilities;

namespace SaveSystem.Core
{
    /// <summary>
    /// Handle data in binary format
    /// </summary>
    internal sealed class BinarySerializator : ISerializator
    {
        internal SaveSystemSettings _systemSettings;

        internal BinarySerializator(SaveSystemSettings systemSettings)
        {
            _systemSettings = systemSettings;
        }
        public T DeserializeFile<T>(byte[] data)
        {
            if (_systemSettings.encrypt)
            {
                data = EncryptorUtility.DecryptFile(data, _systemSettings.cryptoKey);
            }

            using (var memoryStream = new MemoryStream(data))
            {
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(memoryStream);
            }
        }
        public byte[] SerializeData<T>(T data)
        {
            byte[] binaryData;

            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, data);
                binaryData = memoryStream.ToArray();
            }

            if (_systemSettings.encrypt)
            {
                binaryData = EncryptorUtility.EncryptFile(binaryData, _systemSettings.cryptoKey);
            }

            return binaryData;
        }
    }
}

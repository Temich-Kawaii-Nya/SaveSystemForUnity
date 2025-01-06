using Unity.Plastic.Newtonsoft.Json;
using SaveSystem.Utilities;
using System.Text;

namespace SaveSystem.Core
{
    /// <summary>
    /// Handles serialization and deserialization of data using JSON format.
    /// </summary>
    internal sealed class JsonSerializator : ISerializator
    {
        internal SaveSystemSettings _systemSettings;
        internal JsonSerializator(SaveSystemSettings systemSettings)
        {
            _systemSettings = systemSettings;
        }
        public T DeserializeFile<T>(byte[] json)
        {
            if (_systemSettings.encrypt)
            {
                json = EncryptorUtility.DecryptFile(json, _systemSettings.cryptoKey);
            }
            var jsonString = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        public byte[] SerializeData<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            var byteJson = Encoding.UTF8.GetBytes(json);

            if (_systemSettings.encrypt)
            {
                byteJson = EncryptorUtility.EncryptFile(byteJson, _systemSettings.cryptoKey);
            }
            return byteJson;
        }
    }
}


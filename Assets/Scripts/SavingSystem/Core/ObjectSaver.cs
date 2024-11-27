using System;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;

namespace SaveSystem.Core
{
    /// <summary>
    /// Saves and loads fields of savable objects
    /// </summary>
    internal sealed class ObjectSaver
    {
        private SaveFiledsBinder _saveRef;
        private SavableEntities _storage;
        private SaveManager _saveManager;
        public ObjectSaver(SaveFiledsBinder reflaxion, SavableEntities storage, SaveManager saveManager)
        {
            _saveRef = reflaxion;
            _storage = storage;
            _saveManager = saveManager;
        }
        /// <summary>
        /// Save all data from entities
        /// </summary>
        public void Save()
        {
            foreach (var obj in _storage.SavableEntitiesList)
            {
                SaveObject(obj);
            }
        }
        /// <summary>
        /// loads all data for entities
        /// </summary>
        public void Load()
        {
            foreach (var obj in _storage.SavableEntitiesList)
            {
                LoadObject(obj);
            }
        }

        /// <summary>
        /// Saves the state of a single object.
        /// </summary>
        /// <param name="obj">The object to save.</param>
        private void SaveObject(object obj)
        {
            var type = obj.GetType();
            string typeName = type.Name;
            if (_saveRef.Classes.TryGetValue(type, out FieldInfo[] fields))
            {
                foreach (var field in fields)
                {
                    var key = typeName + "." + field.Name;
                    SaveField(key, field.GetValue(obj));
                }
            }
        }

        /// <summary>
        /// Loads the state of a single object.
        /// </summary>
        /// <param name="obj">The object to load.</param>
        private void LoadObject(object obj)
        {
            var type = obj.GetType();
            string typeName = type.Name;
            if (_saveRef.Classes.TryGetValue(type, out FieldInfo[] fields))
            {
                foreach (var field in fields)
                {
                    var key = typeName + "." + field.Name;
                    var fieldType = field.FieldType;
                    var value = GetField(key, fieldType);
                    field.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// Serializes and saves a field value into the save data.
        /// </summary>
        /// <typeparam name="T">The type of the field value.</typeparam>
        /// <param name="key">The unique key for the field.</param>
        /// <param name="value">The value to save.</param>
        private void SaveField<T>(string key, T value)
        {
            string valueJson = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var data = _saveManager.SaveData.savedData ??= new();

            if (data.ContainsKey(key))
                data[key] = valueJson;
            else
                data.Add(key, valueJson);
        }

        /// <summary>
        /// Deserializes and retrieves a field value from the save data.
        /// </summary>
        /// <param name="key">The unique key for the field.</param>
        /// <param name="type">The type of the field value.</param>
        /// <returns>The deserialized field value.</returns>
        private object GetField(string key, Type type)
        {
            _saveManager.SaveData.savedData.TryGetValue(key, out string json);
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}

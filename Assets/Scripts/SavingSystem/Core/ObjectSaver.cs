using System;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;

namespace SaveSystem.Core
{
    /// <summary>
    /// Saves and loads fields of savable objects
    /// </summary>
    internal class ObjectSaver
    {
        private SaveFiledBinder _saveRef;
        private SavableEntities _storage;
        private SaveManager _saveManager;
        public ObjectSaver(SaveFiledBinder reflaxion, SavableEntities storage, SaveManager saveManager)
        {
            _saveRef = reflaxion;
            _storage = storage;
            _saveManager = saveManager;
        }
        public void Save()
        {
            foreach (var obj in _storage.SavableEntitiesList)
            {
                SaveObject(obj);
            }
        }
        public void Load()
        {
            foreach (var obj in _storage.SavableEntitiesList)
            {
                LoadObject(obj);
            }
        }
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
        private object GetField(string key, Type type)
        {
            _saveManager.SaveData.savedData.TryGetValue(key, out string json);
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}

using SaveSystem.Models;
using SaveSystem.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SaveSystem.Core
{
    /// <summary>
    /// Handle operations with files 
    /// </summary>
    internal sealed class FileHandler
    {
        public SaveManager _savesMetas;

        private ISerializator _serializator;

        private SaveSystemSettings _saveSystemSettings;

        private ObjectSaver _objectSaver;

        internal FileHandler(ISerializator strategy, SaveSystemSettings systemSettings, SaveManager saveMeta, ObjectSaver saver)
        {
            _serializator = strategy;
            _saveSystemSettings = systemSettings;
            _savesMetas = saveMeta;
            _objectSaver = saver;
        }
        public SaveMeta CreateSave(string name, string player = "", float progress = 0f, string description = "")
        {
            var id = Guid.NewGuid();
            var meta = new SaveMeta(id, name, player, progress, description);
            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);
            meta.filePath = Path.Combine(_saveSystemSettings.Path, fileName);
            meta.lastLoadTime = DateTime.Now;
            meta.lastUpdate = DateTime.Now;
            _savesMetas.SaveMetas.Add(id, meta);

            return meta;
        }
        /// <summary>
        /// Load all data from save and apply it to savable objects
        /// </summary>
        public async Task LoadSave(SaveMeta meta)
        {
            var filePath = GetFilePath(meta);

            var json = await File.ReadAllBytesAsync(filePath);

            var loadedData = _serializator.DeserializeFile<SavedData>(json);
            
            _savesMetas.SaveData = loadedData;
            
            _objectSaver.Load();
        }
        /// <summary>
        /// Save all savable objects data to save
        /// </summary>
        /// 
        public async Task Save(SaveMeta meta)
        {
            _objectSaver.Save();

            var filePath = GetFilePath(meta);

            var json = _serializator.SerializeData(_savesMetas.SaveData);

            await File.WriteAllBytesAsync(filePath, json);


            meta.time = SaveMetaUtility.CalculatePlayTime(meta);

            meta.lastUpdate = DateTime.Now;

            meta.volume = new FileInfo(filePath).Length;
        }
        /// <summary>
        /// Loads saves meta files
        /// </summary>
        public async Task LoadSaveMetasFile()
        {
            var filePath = Path.Combine(_saveSystemSettings.Path, "save.meta");
            var json = await File.ReadAllBytesAsync(filePath);

            var metas = _serializator.DeserializeFile<Dictionary<Guid, SaveMeta>>(json);

            _savesMetas.SaveMetas = metas;
        }
        /// <summary>
        /// Saves meta files
        /// </summary>
        public async Task SaveMetasFile()
        {
            var filePath = Path.Combine(_saveSystemSettings.Path, "save.meta");
            var json = _serializator.SerializeData(_savesMetas.SaveMetas);
        
            await File.WriteAllBytesAsync(filePath, json);
        }
        /// <summary>
        /// Delete save file
        /// </summary>
        public void DeleteSave(SaveMeta meta)
        {
            DeleteSaveData(meta);
        }
        /// <summary>
        /// Delete all save files
        /// </summary>
        public void DeleteAllSaves()
        {
            foreach (var meta in _savesMetas.SaveMetas.Values)
            {
                DeleteSaveData(meta);
            }
        }
        private void DeleteSaveData(SaveMeta meta)
        {
            var path = GetSaveFilePath(meta);

            if (File.Exists(path))
                File.Delete(path);

            _savesMetas.SaveMetas.Remove(meta.id);
        }
        private string GetSaveFilePath(SaveMeta meta)
        {
            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);

            var filePath = Path.Combine(_saveSystemSettings.Path, fileName);

            return filePath;
        }
        private string GetFilePath(SaveMeta meta)
        {
            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);
            return Path.Combine(_saveSystemSettings.Path, fileName);
        }
    }

}
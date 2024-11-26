using SaveSystem.Models;
using SaveSystem.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SaveSystem.Core
{
    /// <summary>
    /// Handle operations with files 
    /// </summary>
    internal sealed class FileHandler : IFileHandler
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
        public IEnumerator LoadSave(SaveMeta meta) //
        {
            var isLoaded = false;

            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);

            var filePath = Path.Combine(_saveSystemSettings.Path, fileName);

            var json = File.ReadAllBytes(filePath);

            var loadedData = _serializator.DeserializeFile<SavedData>(json);

            _savesMetas.SaveData = loadedData;

            _objectSaver.Load();

            isLoaded = true;

            while (!isLoaded)
            {
                yield return null;
            }
        }
        /// <summary>
        /// Save all savable objects data to save
        /// </summary>
        public IEnumerator Save(SaveMeta meta)
        {
            var isSaved = false;

            _objectSaver.Save();

            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);

            var filePath = Path.Combine(_saveSystemSettings.Path, fileName);

            var json = _serializator.SerializeData(_savesMetas.SaveData);

            File.WriteAllBytes(filePath, json);


            meta.time = SaveMetaUtility.CalculatePlayTime(meta);

            meta.lastUpdate = DateTime.Now;

            meta.volume = new FileInfo(filePath).Length;

            isSaved = true;

            while (!isSaved)
            {
                yield return null;
            }
        }
        /// <summary>
        /// Loads saves meta files
        /// </summary>
        public IEnumerator LoadSaveMetasFile()
        {
            var isLoaded = false;

            var filePath = Path.Combine(_saveSystemSettings.Path, "save.meta");

            var json = File.ReadAllBytes(filePath);

            var metas = _serializator.DeserializeFile<Dictionary<Guid, SaveMeta>>(json);

            _savesMetas.SaveMetas = metas;

            isLoaded = true;

            while (isLoaded)
            {
                yield return null;
            }
        }
        /// <summary>
        /// Saves meta files
        /// </summary>
        public IEnumerator SaveMetasFile()
        {
            var isSaved = false;

            var filePath = Path.Combine(_saveSystemSettings.Path, "save.meta");
            var json = _serializator.SerializeData(_savesMetas.SaveMetas);
        
            File.WriteAllBytes(filePath, json);

            isSaved = true;

            while (isSaved)
            {
                yield return null;
            }
        }
        /// <summary>
        /// Delete save file
        /// </summary>
        public IEnumerator DeleteSave(SaveMeta meta)
        {
            DeleteSaveData(meta);
            yield return null;
        }
        /// <summary>
        /// Delete all save files
        /// </summary>
        public IEnumerator DeleteAllSaves()
        {
            foreach (var meta in _savesMetas.SaveMetas.Values)
            {
                DeleteSaveData(meta);
            }
            yield return null;
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
    }

}
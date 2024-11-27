using SaveSystem.Models;
using SaveSystem.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SaveSystem.Core
{
    /// <summary>
    /// Handles file operations for save data, including saving, loading, and deleting save files and metadata.
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

        /// <summary>
        /// Creates a new save file metadata entry and updates the metadata manager.
        /// </summary>
        /// <param name="name">Name of the save file.</param>
        /// <param name="player">Optional name of the player associated with the save.</param>
        /// <param name="progress">Optional progress value for the save.</param>
        /// <param name="description">Optional description of the save.</param>
        /// <returns>Ñreated <see cref="SaveMeta"/> object.</returns>
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
        /// Loads save data from a file and applies it to savable objects.
        /// </summary>
        /// <param name="meta">Metadata of the save file to load.</param>
        public async Task LoadSave(SaveMeta meta)
        {
            var filePath = GetFilePath(meta);

            var json = await File.ReadAllBytesAsync(filePath);

            var loadedData = _serializator.DeserializeFile<SavedData>(json);
            
            _savesMetas.SaveData = loadedData;
            
            _objectSaver.Load();
        }
        /// <summary>
        /// Saves the current state of savable objects to a save file.
        /// </summary>
        /// <param name="meta">Metadata of the save file to update.</param>
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
        /// Loads metadata for all saves from a file.
        /// </summary>
        public async Task LoadSaveMetasFile()
        {
            var filePath = Path.Combine(_saveSystemSettings.Path, "save.meta");
            var json = await File.ReadAllBytesAsync(filePath);

            var metas = _serializator.DeserializeFile<Dictionary<Guid, SaveMeta>>(json);

            _savesMetas.SaveMetas = metas;
        }
        /// <summary>
        /// Saves metadata for all saves to a file.
        /// </summary>
        public async Task SaveMetasFile()
        {
            var filePath = Path.Combine(_saveSystemSettings.Path, "save.meta");
            var json = _serializator.SerializeData(_savesMetas.SaveMetas);
        
            await File.WriteAllBytesAsync(filePath, json);
        }
        /// <summary>
        /// Deletes a specific save file and removes its metadata entry.
        /// </summary>
        public void DeleteSave(SaveMeta meta)
        {
            DeleteSaveData(meta);
        }
        /// <summary>
        /// Deletes all save files and clears their metadata entries.
        /// </summary>
        public void DeleteAllSaves()
        {
            foreach (var meta in _savesMetas.SaveMetas.Values)
            {
                DeleteSaveData(meta);
            }
        }

        /// <summary>
        /// Deletes a save file and its associated metadata.
        /// </summary>
        /// <param name="meta">Metadata of the save file to delete.</param>
        private void DeleteSaveData(SaveMeta meta)
        {
            var path = GetSaveFilePath(meta);

            if (File.Exists(path))
                File.Delete(path);

            _savesMetas.SaveMetas.Remove(meta.id);
        }

        /// <summary>
        /// Gets the file path for a save file using its metadata.
        /// </summary>
        /// <param name="meta">Metadata of the save file.</param>
        /// <returns>Absolute file path for the save file.</returns>
        private string GetSaveFilePath(SaveMeta meta)
        {
            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);

            var filePath = Path.Combine(_saveSystemSettings.Path, fileName);

            return filePath;
        }

        /// <summary>
        /// Alias for retrieving the save file path using metadata.
        /// </summary>
        /// <param name="meta">Metadata of the save file.</param>
        /// <returns>Absolute file path for the save file.</returns>
        private string GetFilePath(SaveMeta meta)
        {
            var fileName = SaveMetaUtility.GetFileName(meta, _saveSystemSettings.saveFileExtension);
            return Path.Combine(_saveSystemSettings.Path, fileName);
        }
    }

}
using SaveSystem.Core;
using SaveSystem.Models;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Main class providing operations for managing saves.
    /// </summary>
    public sealed class SaveSystem : MonoBehaviour
    {
        /// <summary>
        /// Configuration settings for the save system, which can be set via the editor menu: Tools -> SaveSystem Settings.
        /// </summary>
        [SerializeField] private SaveSystemSettings _settings;
        
        private SaveFiledsBinder _saveFieldBinder;
        private SavableEntities _savableEntities;
        private SaveManager _saveMetaManager;

        private void Awake()
        {
            _savableEntities = new SavableEntities();
            _saveFieldBinder = new SaveFiledsBinder();
            _saveMetaManager = new SaveManager();
        }
        /// <summary>
        /// Creates a new save file with the specified parameters.
        /// </summary>
        /// <param name="name">Name of the save file.</param>
        /// <param name="player">Optional name of the player associated with the save.</param>
        /// <param name="description">Optional description of the save.</param>
        /// <param name="progress">Optional progress value associated with the save (e.g., completion percentage).</param>
        /// <returns>The metadata of the newly created save file.</returns>
        public async Task<SaveMeta> CreateNewSave(string name, string player = "", string description = "", float progress = 0f)
        {
            var saveHandler = CreateFileHandler();
            var meta = saveHandler.CreateSave(name, player, progress, description);
          
            await saveHandler.SaveMetasFile();
            await saveHandler.Save(meta);

            return meta;
        }
        /// <summary>
        /// Loads all save meta files
        /// </summary>
        public async Task LoadMeta()
        {
            await CreateFileHandler().LoadSaveMetasFile();
        }
        /// <summary>
        /// Saves all save meta files
        /// </summary>
        public async Task SaveMeta()
        {
            await CreateFileHandler().SaveMetasFile();
        }
        /// <summary>
        /// Loads data from a save file and restores the state of savable entities.
        /// </summary>
        /// <param name="meta">The metadata of the save file to load.</param>
        public async Task LoadData(SaveMeta meta)
        {
            await CreateFileHandler().LoadSave(meta);
        }
        /// <summary>
        /// Saves the current state of savable entities to a save file.
        /// </summary>
        /// <param name="meta">The metadata of the save file to update.</param>
        public async Task SaveData(SaveMeta meta)
        {
            await CreateFileHandler().Save(meta);
        }
        /// <summary>
        /// Deletes a specific save file.
        /// </summary>
        /// <param name="meta">The metadata of the save file to delete.</param>
        public void DeleteSave(SaveMeta meta)
        {
            CreateFileHandler().DeleteSave(meta);
        }
        /// <summary>
        /// Deletes all save files and their metadata.
        /// </summary>
        public void DeleteAllSaves()
        {
            CreateFileHandler().DeleteAllSaves();
        }
        /// <summary>
        /// Creates a serializer instance based on the save type defined in the settings (e.g., JSON or binary).
        /// </summary>
        /// <returns>An implementation of the <see cref="ISerializator"/> interface.</returns>
        private ISerializator CreateSerializator()
        {
            switch (_settings.saveType)
            {
                case SaveSystemSettings.SaveType.json:
                    return new JsonSerializator(_settings);
                case SaveSystemSettings.SaveType.bin:
                    return new BinarySerializator(_settings);
                default:
                    return new JsonSerializator(_settings);
            }
        }
        /// <summary>
        /// Creates a file handler to manage file operations, including serialization, metadata management, and object saving.
        /// </summary>
        /// <returns>An instance of <see cref="FileHandler"/>.</returns>
        private FileHandler CreateFileHandler()
        {
            var saver = new ObjectSaver(_saveFieldBinder, _savableEntities, _saveMetaManager);
            var saveHandler = new FileHandler(CreateSerializator(), _settings, _saveMetaManager, saver);
            return saveHandler;
        }
    }
}
using SaveSystem.Core;
using SaveSystem.Models;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// SaveSystem is a main class that provides all operation with saves
    /// </summary>
    public sealed class SaveSystem : MonoBehaviour
    {
        /// <summary>
        /// Settings can be created in Editor: tools -> SaveSystem Settings
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
        /// Creates new Save
        /// </summary>
        
        public async Task<SaveMeta> CreateNewSave(string name, string player = "", float progress = 0f, string description = "")
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
        /// Loads data from save and restore savable entities state
        /// </summary>
        public async Task LoadData(SaveMeta meta)
        {
            await CreateFileHandler().LoadSave(meta);
        }
        /// <summary>
        /// Save savable entities state
        /// </summary>
        public async Task SaveData(SaveMeta meta)
        {
            await CreateFileHandler().Save(meta);
        }
        /// <summary>
        /// Delete save file
        /// </summary>
        public void DeleteSave(SaveMeta meta)
        {
            CreateFileHandler().DeleteSave(meta);
        }
        /// <summary>
        /// Delete all save files
        /// </summary>
        public void DeleteAllSaves()
        {
            CreateFileHandler().DeleteAllSaves();
        }

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
        private FileHandler CreateFileHandler()
        {
            var saver = new ObjectSaver(_saveFieldBinder, _savableEntities, _saveMetaManager);
            var saveHandler = new FileHandler(CreateSerializator(), _settings, _saveMetaManager, saver);
            return saveHandler;
        }
    }
}
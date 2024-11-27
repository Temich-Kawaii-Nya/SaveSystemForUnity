using System;
using UnityEngine;

namespace SaveSystem.Core
{
    /// <summary>
    /// Contains settings for configuring the behavior of the save system.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "SaveSystemSettings", menuName = "SaveSystem/Settings")]
    internal sealed class SaveSystemSettings : ScriptableObject
    {
        /// <summary>
        /// Enum representing the type of serialization format used for saving data.
        /// </summary>
        public enum SaveType
        {
            json,
            bin
        }

        /// <summary>
        /// Enum representing the location where save files are stored.
        /// <para>ApplicationDataPath - Application.persistentDataPath</para>
        /// </summary>
        public enum SavePath
        {
            ApplicationDataPath,
            PlayerPrefs,
            Custom
        }

        /// <summary>
        /// Returns the full path to the save directory based on the selected save path setting.
        /// </summary>
        public string Path
        {
            get
            {
                return ConstructPath();
            }
        }

        /// <summary>
        /// Determines where save files are stored.
        /// </summary>
        public SavePath savePath = SavePath.ApplicationDataPath;

        /// <summary>
        /// Specifies the format used to save data. Defaults to JSON.
        /// </summary>
        public SaveType saveType = SaveType.json;

        /// <summary>
        /// Enables or disables encryption for save files.
        /// </summary>
        public bool encrypt = false;

        /// <summary>
        /// Enables or disables automatic creation of backup files for save data.
        /// </summary>
        public bool autoCreateBak = false;

        /// <summary>
        /// File extension for save files.
        /// </summary>
        [HideInInspector] public string saveFileExtension = ".sav";

        /// <summary>
        /// File extension for backup files.
        /// </summary>
        [HideInInspector] public string bakFileExtension = ".bak";


        /// <summary>
        /// Custom path for saving files. Used only if <see cref="SavePath.Custom"/> is selected.
        /// </summary>
        [HideInInspector] public string customPath;

        /// <summary>
        /// Cryptographic key used for encryption, if enabled.
        /// </summary>
        [HideInInspector] public string cryptoKey;

        /// <summary>
        /// Constructs the full save path based on the selected save path option.
        /// </summary>
        /// <returns>The full path to the save directory.</returns>
        private string ConstructPath()
        {
            switch (savePath)
            {
                case SavePath.ApplicationDataPath:
                    return GetApplicationSavePath();
                case SavePath.Custom:
                    return GetCustomPath();
                default:
                    return GetApplicationSavePath();
            }
        }

        /// <summary>
        /// Returns the path to the save directory for the custom path option.
        /// </summary>
        /// <returns>The full custom save path.</returns>
        private string GetCustomPath()
        {
            return System.IO.Path.Combine(customPath, "Saves");
        }

        /// <summary>
        /// Returns the default application save directory located in the persistent data path.
        /// </summary>
        /// <returns>The full application save path.</returns>
        private string GetApplicationSavePath()
        {
            return System.IO.Path.Combine(Application.persistentDataPath, "Saves");
        }
    }
}


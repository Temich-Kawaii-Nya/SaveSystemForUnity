using System;
using UnityEngine;

namespace SaveSystem.Core
{
    [Serializable]
    [CreateAssetMenu(fileName = "SaveSystemSettings", menuName = "SaveSystem/Settings")]
    internal sealed class SaveSystemSettings : ScriptableObject
    {
        public enum SaveType
        {
            json,
            bin
        }

        public enum SavePath
        {
            ApplicationDataPath,
            PlayerPrefs,
            Custom
        }

        public string Path
        {
            get
            {
                return ConstructPath();
            }
        }

        public SavePath savePath = SavePath.ApplicationDataPath;
        public SaveType saveType = SaveType.json;
        public bool encrypt = false;
        public bool autoCreateBak = false;

        [HideInInspector] public string saveFileExtension = ".sav";
        [HideInInspector] public string bakFileExtension = ".bak";

        [HideInInspector] public string customPath;

        [HideInInspector] public string cryptoKey;

        private string ConstructPath()
        {
            switch (savePath)
            {
                case SavePath.ApplicationDataPath:
                    return GetApplicationSavePath();
                case SavePath.Custom:
                    return GetCustomPath();
                default:
                    return "None";
            }
        }
        private string GetCustomPath()
        {
            return System.IO.Path.Combine(customPath, "Saves");
        }
        private string GetApplicationSavePath()
        {
            return System.IO.Path.Combine(Application.persistentDataPath, "Saves"); //TODO hardcode
        }
    }
}


using UnityEditor;
using UnityEngine;
using SaveSystem.Utilities;

namespace SaveSystem.Core.Editor
{
    internal sealed class SaveSystemSettingsEditorWindow : EditorWindow
    {
        private SaveSystemSettings _settings;

        [MenuItem("Tools/Save System Settings")]
        public static void ShowWindow()
        {
            GetWindow<SaveSystemSettingsEditorWindow>("Save System Settings");
        }

        private void OnEnable()
        {
            _settings = AssetDatabase.LoadAssetAtPath<SaveSystemSettings>("Assets/SaveSystemSettings.asset");
        }

        private void OnGUI()
        {
            GUILayout.Label("Save System Settings", EditorStyles.boldLabel);

            if (_settings == null)
            {
                EditorGUILayout.HelpBox("SaveSystemSettings asset not found!", MessageType.Warning);
                if (GUILayout.Button("Create SaveSystemSettings"))
                {
                    CreateSettingsAsset();
                }
                return;
            }

            _settings.savePath = (SaveSystemSettings.SavePath)EditorGUILayout.EnumPopup("Save Path", _settings.savePath);

            if (_settings.savePath == SaveSystemSettings.SavePath.Custom)
            {
                _settings.customPath = EditorGUILayout.TextField("Custom Save Path", _settings.customPath);
            }

            _settings.saveType = (SaveSystemSettings.SaveType)EditorGUILayout.EnumPopup("Save Type", _settings.saveType);

            EditorGUILayout.LabelField("Save File Extension", _settings.saveFileExtension);
            EditorGUILayout.LabelField("Backup File Extension", _settings.bakFileExtension);

            _settings.autoCreateBak = EditorGUILayout.Toggle("Auto Create Backup File", _settings.autoCreateBak);

            _settings.encrypt = EditorGUILayout.Toggle("Encrypt Save Files", _settings.encrypt);

            if (_settings.encrypt)
            {
                EditorGUILayout.TextField("Encrypt Key", _settings.cryptoKey);
                if (_settings.cryptoKey.Length < 1)
                {
                    if (GUILayout.Button("Generate Key"))
                    {
                        _settings.cryptoKey = EncryptorUtility.GenerateKey();
                    }
                }
            }

            EditorGUILayout.LabelField("Calculated Save Path", _settings.Path);

            if (GUILayout.Button("Save Settings"))
            {
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
                Debug.Log("Settings saved!");
            }
        }

        private void CreateSettingsAsset()
        {
            SaveSystemSettings newSettings = ScriptableObject.CreateInstance<SaveSystemSettings>();
            AssetDatabase.CreateAsset(newSettings, "Assets/SaveSystemSettings.asset");
            AssetDatabase.SaveAssets();
            _settings = newSettings;
        }
    }
}

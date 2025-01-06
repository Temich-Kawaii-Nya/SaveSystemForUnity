using UnityEditor;
using UnityEngine;

namespace SaveSystem.Core.Editor
{
    [CustomEditor(typeof(SaveSystemSettings))]
    internal sealed class SaveSystemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SaveSystemSettings settings = (SaveSystemSettings)target;

            if (settings.savePath == SaveSystemSettings.SavePath.Custom)
            {
                settings.customPath = EditorGUILayout.TextField("Custom Path", settings.customPath);
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}

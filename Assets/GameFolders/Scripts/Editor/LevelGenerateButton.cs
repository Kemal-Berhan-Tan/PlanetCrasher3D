#if UNITY_EDITOR

using GameFolders.Scripts.Objects;
using UnityEditor;
using UnityEngine;

namespace GameFolders.Scripts.Editor
{
    [CustomEditor(typeof(LevelItem))]
    public class LevelGenerateButton : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelItem script = (LevelItem)target;

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Generate The Level"))
            {
                script.GenerateTheLevel(false);
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(script);
            }
        }
    }
}
#endif
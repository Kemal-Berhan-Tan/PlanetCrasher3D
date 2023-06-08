#if UNITY_EDITOR
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Controllers;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Objects;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GameFolders.Scripts.Editor
{
    [CustomEditor(typeof(InteractableItem))]
    public class SelectedEnumResultToItemInInspector : UnityEditor.Editor
    {
        private InteractableItem script;

        private void OnSceneGUI()
        {
            script = (InteractableItem)target;

            CurvySpline curvy =
                GameObject.Find("CurrentLevelCurvySpline").GetComponent<CurvySpline>();
            if (curvy)
                script.GetComponent<SplineController>().Spline = curvy;
        }

        public override void OnInspectorGUI()
        {
            script = (InteractableItem)target;

            EditorGUI.BeginChangeCheck();
            script.ItemType = (InteractableItemTypes)EditorGUILayout.EnumPopup("Item Type", script.ItemType);

            if (script.ItemType == InteractableItemTypes.Alien)
            {
                script.PowerTitle = (PowerTitles)EditorGUILayout.EnumPopup("Power Title", script.PowerTitle);
                script.PowerTitleValue =
                    EditorGUILayout.IntField("Power Title Relegation Value", script.PowerTitleValue);
                script.PodyPartDamageValue =
                    EditorGUILayout.IntField("Body Part Damage Value", script.PodyPartDamageValue);

                script.PowerTitleText =
                    EditorGUILayout.ObjectField("Power Title Text", script.PowerTitleText, typeof(TMP_Text), true) as
                        TMP_Text;
                script.RigObject =
                    EditorGUILayout.ObjectField("Rig Object", script.RigObject, typeof(GameObject), true) as
                        GameObject;

                if (script.PowerTitleText)
                    script.PowerTitleText.text = script.PowerTitle.ToString().Replace("_", " ");
            }
            else
            {
                if (script.ItemType == InteractableItemTypes.TitleUpgradeItems)
                    script.PowerTitleValue =
                        EditorGUILayout.IntField("Power Title Upgrade Value", script.PowerTitleValue);
                else if (script.ItemType == InteractableItemTypes.Obstacle)
                    script.PodyPartDamageValue =
                        EditorGUILayout.IntField("Body Part Damage Value", script.PodyPartDamageValue);
                script.DefObject =
                    EditorGUILayout.ObjectField("Def Object", script.DefObject, typeof(GameObject), true) as GameObject;
                script.BrokenObject =
                    EditorGUILayout.ObjectField("Broken Object", script.BrokenObject, typeof(GameObject), true) as
                        GameObject;
            }

            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(script);
        }
    }
}
#endif
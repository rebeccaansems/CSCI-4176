using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Custom inspector editor for DialogueSystemController (e.g., Dialogue Manager).
    /// </summary>
    [CustomEditor(typeof(DialogueSystemController))]
    public class DialogueSystemControllerEditor : Editor
    {

        private const string LightSkinIconFilename = "DialogueManager Inspector Light.png";
        private const string DarkSkinIconFilename = "DialogueManager Inspector Dark.png";
        private const string DefaultLightSkinIconFilepath = "Assets/Dialogue System/DLLs/DialogueManager Inspector Light.png";
        private const string DefaultDarkSkinIconFilepath = "Assets/Dialogue System/DLLs/DialogueManager Inspector Dark.png";
        private const string IconFilepathEditorPrefsKey = "PixelCrushers.DialogueSystem.IconFilename";

        private static Texture2D icon = null;
        private static GUIStyle iconButtonStyle = null;
        private static GUIContent iconButtonContent = null;

        private DialogueSystemController dialogueSystemController = null;

        private void OnEnable()
        {
            dialogueSystemController = target as DialogueSystemController;
        }

        /// <summary>
        /// Draws the inspector GUI. Adds a Dialogue System banner.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawExtraFeatures();
            var originalDialogueUI = GetCurrentDialogueUI();
            DrawDefaultInspector();
            var newDialogueUI = GetCurrentDialogueUI();
            if (newDialogueUI != originalDialogueUI) CheckDialogueUI(newDialogueUI);
        }

        private GameObject GetCurrentDialogueUI()
        {
            if (dialogueSystemController == null || dialogueSystemController.displaySettings == null) return null;
            return dialogueSystemController.displaySettings.dialogueUI;
        }

        private void CheckDialogueUI(GameObject newDialogueUIObject)
        {
            if (dialogueSystemController.displaySettings.dialogueUI == null) return;
            var newUIs = dialogueSystemController.displaySettings.dialogueUI.GetComponentsInChildren<UnityUIDialogueUI>(true);
            if (newUIs.Length > 0)
            {
                DialogueManagerWizard.HandleUnityUIDialogueUI(newUIs[0], DialogueManager.Instance);
            }
        }

        private void DrawExtraFeatures()
        {
            if (icon == null) icon = FindIcon();
            if (dialogueSystemController == null || icon == null) return;
            if (iconButtonStyle == null)
            {
                iconButtonStyle = new GUIStyle(EditorStyles.label);
                iconButtonStyle.normal.background = icon;
                iconButtonStyle.active.background = icon;
            }
            if (iconButtonContent == null)
            {
                iconButtonContent = new GUIContent(string.Empty, "Click to open Dialogue Editor.");
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(iconButtonContent, iconButtonStyle, GUILayout.Width(icon.width), GUILayout.Height(icon.height)))
            {
                Selection.activeObject = dialogueSystemController.initialDatabase;
                PixelCrushers.DialogueSystem.DialogueEditor.DialogueEditorWindow.OpenDialogueEditorWindow();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Wizard...", GUILayout.Width(64)))
            {
                DialogueManagerWizard.Init();
            }
            GUILayout.EndHorizontal();
            EditorWindowTools.DrawHorizontalLine();
        }

        public static Texture2D FindIcon()
        {
            // Search default location:
            var iconFilepath = EditorGUIUtility.isProSkin ? DefaultDarkSkinIconFilepath : DefaultLightSkinIconFilepath;
            icon = AssetDatabase.LoadAssetAtPath(iconFilepath, typeof(Texture2D)) as Texture2D;
            if (icon != null) return icon;

            // If the customer moved the Dialogue System to another folder, check if we searched before:
            if (EditorPrefs.HasKey(IconFilepathEditorPrefsKey))
            {
                icon = AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString(IconFilepathEditorPrefsKey), typeof(Texture2D)) as Texture2D;
                if (icon == null)
                {
                    EditorPrefs.DeleteKey(IconFilepathEditorPrefsKey);
                }
                else {
                    return icon;
                }
            }

            // Otherwise search project:
            var iconFilename = EditorGUIUtility.isProSkin ? DarkSkinIconFilename : LightSkinIconFilename;
            var filter = "t:texture2D"; // FindAssets doesn't filter names with spaces properly in all versions, so search all textures.
            var results = AssetDatabase.FindAssets(filter);
            if (results != null)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    var filepath = AssetDatabase.GUIDToAssetPath(results[i]);
                    if (filepath.EndsWith(iconFilename))
                    {
                        icon = AssetDatabase.LoadAssetAtPath(filepath, typeof(Texture2D)) as Texture2D;
                        if (icon != null)
                        {
                            EditorPrefs.SetString(IconFilepathEditorPrefsKey, filepath);
                            return icon;
                        }
                    }
                }
            }
            return null;
        }

    }

}

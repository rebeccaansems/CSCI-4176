#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This script provides a facility to update the localized text of 
    /// all localized Text elements. You will typically call it from the
    /// event handler of a language selection button or pop-up menu. It
    /// also localizes Texts at start.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/unity_u_i_dialogue_u_i.html#unityUILocalization")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Update Localized UI Texts")]
    public class UpdateLocalizedUITexts : MonoBehaviour
    {

        /// <summary>
        /// The PlayerPrefs key to store the player's selected language code.
        /// </summary>
        public string languagePlayerPrefsKey = "Language";

        IEnumerator Start()
        {
            yield return null; // Wait for Text components to start.
            var languageCode = string.Empty;
            if (!string.IsNullOrEmpty(languagePlayerPrefsKey) && PlayerPrefs.HasKey(languagePlayerPrefsKey))
            {
                languageCode = PlayerPrefs.GetString(languagePlayerPrefsKey);
            }
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = DialogueManager.DisplaySettings.localizationSettings.language;
            }
            UpdateTexts(languageCode);
        }

        /// <summary>
        /// Updates the current language and all localized Texts.
        /// </summary>
        /// <param name="languageCode">Language code.</param>
        public void UpdateTexts(string languageCode)
        {
            if (DialogueDebug.LogInfo) Debug.Log(DialogueDebug.Prefix + ": Setting language to '" + languageCode + "'.", this);
            DialogueManager.DisplaySettings.localizationSettings.useSystemLanguage = false;
            DialogueManager.DisplaySettings.localizationSettings.language = languageCode;
            PixelCrushers.DialogueSystem.Localization.Language = languageCode;
            if (!string.IsNullOrEmpty(languagePlayerPrefsKey))
            {
                PlayerPrefs.SetString(languagePlayerPrefsKey, languageCode);
            }
            foreach (var localizeUIText in FindObjectsOfType<LocalizeUIText>())
            {
                localizeUIText.LocalizeText();
            }
        }

#if UNITY_EDITOR
        [MenuItem("Window/Dialogue System/Tools/Clear Saved Localization Settings", false, 1)]
        public static void ClearSavedLocalizationSettings()
        {
            PlayerPrefs.DeleteKey("Language");
        }
#endif

    }

}
#endif
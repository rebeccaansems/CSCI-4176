using UnityEngine;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This script localizes the content of the Text element or Dropdown element
    /// on this GameObject. You can assign the localized text table to this script 
    /// or the Dialogue Manager. The element's starting text value(s) serves as the 
    /// field name(s) to look up in the table.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/unity_u_i_dialogue_u_i.html#unityUILocalization")]
#endif
    [AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Localize UI Text")]
    public class LocalizeUIText : MonoBehaviour
    {

        [Tooltip("Optional; overrides the Dialogue Manager's table.")]
        public LocalizedTextTable localizedTextTable;

		[Tooltip("Optional; if assigned, use this instead of the Text field's value as the field lookup value.")]
        public string fieldName = string.Empty;

        protected UnityEngine.UI.Text text = null;
        protected List<string> fieldNames = new List<string>();
        protected bool started = false;
#if (UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1)
        protected UnityEngine.Object dropdown = null;
#else
        protected UnityEngine.UI.Dropdown dropdown = null;
#endif

        protected virtual void Start()
        {
            started = true;
            LocalizeText();
        }

        protected virtual void OnEnable()
        {
            LocalizeText();
        }

        public virtual void LocalizeText()
        {
            if (!started) return;

            // Skip if no language set:
            if (string.IsNullOrEmpty(PixelCrushers.DialogueSystem.Localization.Language)) return;
            if (localizedTextTable == null)
            {
                localizedTextTable = DialogueManager.DisplaySettings.localizationSettings.localizedText;
                if (localizedTextTable == null)
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": No localized text table is assigned to " + name + " or the Dialogue Manager.", this);
                    return;
                }
            }

            if (!HasCurrentLanguage())
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + "Localized text table '" + localizedTextTable + "' does not have a language '" + PixelCrushers.DialogueSystem.Localization.Language + "'", this);
                return;
            }

            // Make sure we have a Text or Dropdown:
            if (text == null && dropdown == null)
            {
                text = GetComponent<UnityEngine.UI.Text>();
#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1)
                dropdown = GetComponent<UnityEngine.UI.Dropdown>();
#endif
                if (text == null && dropdown == null)
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": LocalizeUIText didn't find a Text or Dropdown component on " + name + ".", this);
                    return;
                }
            }

            // Get the original values to use as field lookups:
            if (string.IsNullOrEmpty(fieldName)) fieldName = (text != null) ? text.text : string.Empty;
#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1)
            if ((fieldNames.Count == 0) && (dropdown != null)) dropdown.options.ForEach(opt => fieldNames.Add(opt.text));
#endif
            // Localize Text:
            if (text != null)
            {
                if (!localizedTextTable.ContainsField(fieldName))
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": Localized text table '" + localizedTextTable.name + "' does not have a field: " + fieldName, this);
                }
                else
                {
                    text.text = localizedTextTable[fieldName];
                }
            }

#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1)
            // Localize Dropdown:
            if (dropdown != null)
            {
                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    if (i < fieldNames.Count)
                    {
                        dropdown.options[i].text = localizedTextTable[fieldNames[i]];
                    }
                }
                dropdown.captionText.text = localizedTextTable[fieldNames[dropdown.value]];
            }
#endif
        }

        protected virtual bool HasCurrentLanguage()
        {
            if (localizedTextTable == null) return false;
            foreach (var language in localizedTextTable.languages)
            {
                if (string.Equals(language, PixelCrushers.DialogueSystem.Localization.Language))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the field name, which is the key to use in the localized text table.
        /// By default, the field name is the initial value of the Text component.
        /// </summary>
        /// <param name="fieldName"></param>
        public virtual void UpdateFieldName(string newFieldName = "")
        {
            if (text == null) text = GetComponent<UnityEngine.UI.Text>();
            fieldName = string.IsNullOrEmpty(newFieldName) ? text.text : newFieldName;
        }

        /// <summary>
        /// If changing the Dropdown options, call this afterward to update the localization.
        /// </summary>
        public virtual void UpdateOptions()
        {
            fieldNames.Clear();
            LocalizeText();
        }
    }

}

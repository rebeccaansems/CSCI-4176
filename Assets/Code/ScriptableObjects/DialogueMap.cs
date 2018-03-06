using System.Collections.Generic;
using UnityEngine;

public class DialogueMap : ScriptableObject
{
    [System.Serializable]
    public class Dialogue
    {
        [Tooltip("Used as a short name to specifically reference text")]
        public string Name;
        [Tooltip("The text that is spoken by the jelly, multiple text able to be assigned. Default is 0 (top)")]
        [SerializeField]
        private string[] Text;
        [Tooltip("This text cannot be shown without direct player input (ie this text must be coded to be shown)")]
        public bool WaitForInput;

        public string GetText()
        {
            return GetText(0);
        }

        public string GetText(int textIndex)
        {
            if (textIndex > Text.Length)
            {
                return string.Format("[ERROR]: Name: {0} Index: {1}", Name, textIndex);
            }
            return Text[textIndex];
        }
    }
}
using UnityEngine;
using System;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Add this to the Dialogue Manager to allow it to dispatch quest state updates
    /// to QuestStateListener components on other GameObjects.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/quest_log_window.html#questIndicator")]
#endif
    [AddComponentMenu("Dialogue System/Miscellaneous/Quest Indicators/Quest State Dispatcher (on Dialogue Manager)")]
    public class QuestStateDispatcher : MonoBehaviour
    {

        private List<QuestStateListener> m_listeners = new List<QuestStateListener>();

        public void AddListener(QuestStateListener listener)
        {
            if (listener == null) return;
            m_listeners.Add(listener);
        }

        public void RemoveListener(QuestStateListener listener)
        {
            m_listeners.Remove(listener);
        }

        public void OnQuestStateChange(string questName)
        {
            for (int i = 0; i < m_listeners.Count; i++)
            {
                var listener = m_listeners[i];
                if (string.Equals(questName, listener.questName))
                {
                    listener.OnChange();
                }
            }
        }

    }
}
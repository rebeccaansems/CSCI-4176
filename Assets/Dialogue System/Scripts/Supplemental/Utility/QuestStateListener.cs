using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Add this to a GameObject such as an NPC that wants to know about quest state changes
    /// to a specific quest. You can add multiple QuestStateListener components to listen
    /// to multiple quests.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/quest_log_window.html#questIndicator")]
#endif
    [AddComponentMenu("Dialogue System/Miscellaneous/Quest Indicators/Quest State Listener")]
    public class QuestStateListener : MonoBehaviour
    {

        [QuestPopup(true)]
        public string questName;

        [Serializable]
        public class QuestStateIndicatorLevel
        {
            [Tooltip("Quest state to listen for.")]
            public QuestState questState;

            [Tooltip("Conditions that must also be true.")]
            public Condition condition;

            [Tooltip("Indicator level to use when this quest state is reached.")]
            public int indicatorLevel;

            public UnityEvent onEnterState = new UnityEvent();
        }

        public QuestStateIndicatorLevel[] questStateIndicatorLevels = new QuestStateIndicatorLevel[0];

        [Serializable]
        public class QuestEntryStateIndicatorLevel
        {
            [Tooltip("Quest entry number.")]
            public int entryNumber;

            [Tooltip("Quest entry state to listen for.")]
            public QuestState questState;

            [Tooltip("Conditions that must also be true.")]
            public Condition condition;

            [Tooltip("Indicator level to use when this quest state is reached.")]
            public int indicatorLevel;

            public UnityEvent onEnterState = new UnityEvent();
        }

        public QuestEntryStateIndicatorLevel[] questEntryStateIndicatorLevels = new QuestEntryStateIndicatorLevel[0];

        private QuestStateDispatcher m_questStateDispatcher;
        private QuestStateIndicator m_questStateIndicator;
        private bool m_started = false;

        void Awake()
        {
            m_questStateDispatcher = FindObjectOfType<QuestStateDispatcher>();
            m_questStateIndicator = GetComponent<QuestStateIndicator>();
            if (m_questStateDispatcher == null)
            {
                var dialogueManager = FindObjectOfType<DialogueSystemController>();
                if (dialogueManager != null)
                {
                    m_questStateDispatcher = dialogueManager.gameObject.AddComponent<QuestStateDispatcher>();
                }
                else
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning("Dialogue System: " + name + ": Can't find the Dialogue Manager to add a QuestStateDispatcher.", this);
                    enabled = false;
                }                
            }
        }

        IEnumerator Start()
        {
            yield return null;
            if (enabled)
            {
                if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: " + name + ": Listening for state changes to quest '" + questName + "'.", this);
                m_started = true;
                m_questStateDispatcher.AddListener(this);
                UpdateIndicator();
            }
        }

        void OnEnable()
        {
            if (m_started) m_questStateDispatcher.AddListener(this);
        }

        void OnDisable()
        {
            if (m_questStateDispatcher != null) m_questStateDispatcher.RemoveListener(this);
        }

        public void OnChange()
        {
            UpdateIndicator();
        }

        /// <summary>
        /// Update the current quest state indicator based on the specified quest state indicator 
        /// levels and quest entry state indicator levels.
        /// </summary>
        public void UpdateIndicator()
        {
            // Check quest state:
            var questState = QuestLog.GetQuestState(questName);
            for (int i = 0; i < questStateIndicatorLevels.Length; i++)
            {
                var questStateIndicatorLevel = questStateIndicatorLevels[i];
                if (questState == questStateIndicatorLevel.questState && questStateIndicatorLevel.condition.IsTrue(null))
                {
                    if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: " + name + ": Quest '" + questName + "' changed to state " + questState + ".", this);
                    if (m_questStateIndicator != null) m_questStateIndicator.SetIndicatorLevel(this, questStateIndicatorLevel.indicatorLevel);
                    questStateIndicatorLevel.onEnterState.Invoke();
                }
            }

            // Check quest entry states:
            for (int i = 0; i < questEntryStateIndicatorLevels.Length; i++)
            {
                var questEntryStateIndicatorLevel = questEntryStateIndicatorLevels[i];
                var questEntryState = QuestLog.GetQuestEntryState(questName, questEntryStateIndicatorLevel.entryNumber);
                if (questEntryState == questEntryStateIndicatorLevel.questState && questEntryStateIndicatorLevel.condition.IsTrue(null))
                {
                    if (DialogueDebug.LogInfo) Debug.Log("Dialogue System: " + name + ": Quest '" + questName + "' entry " + questEntryStateIndicatorLevel.entryNumber + " changed to state " + questEntryState + ".", this);
                    if (m_questStateIndicator != null) m_questStateIndicator.SetIndicatorLevel(this, questEntryStateIndicatorLevel.indicatorLevel);
                    questEntryStateIndicatorLevel.onEnterState.Invoke();
                }
            }
        }

    }
}
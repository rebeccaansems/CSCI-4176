using UnityEngine;
using UnityEngine.Events;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Increments an element of the Lua Variable[] table when the GameObject is destroyed or 
    /// disabled, and then updates the quest tracker if it's attached to the Dialogue Manager
    /// object or its children. This script is useful for kill quests or gathering quests.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/increment_on_destroy.html")]
#endif
    [AddComponentMenu("Dialogue System/Actor/Increment On Destroy")]
    public class IncrementOnDestroy : MonoBehaviour
    {

        public enum IncrementOn { Destroy, Disable }

        [Tooltip("Increment on Destroy or Disable.")]
        public IncrementOn incrementOn = IncrementOn.Destroy;

        /// <summary>
        /// The variable to increment.
        /// </summary>
        [Tooltip("Increment this Dialogue System variable.")]
        public string variable = string.Empty;

        /// <summary>
        /// The increment amount. To decrement, use a negative number.
        /// </summary>
        [Tooltip("Increment the variable by this amount. Use a negative value to decrement.")]
        public int increment = 1;

        /// <summary>
        /// The minimum value.
        /// </summary>
        [Tooltip("After incrementing, ensure that the variable is at least this value.")]
        public int min = 0;

        /// <summary>
        /// The maximum value.
        /// </summary>
        [Tooltip("After incrementing, ensure that the variable is no more than this value.")]
        public int max = 100;

        [Tooltip("Optional alert message to show when incrementing.")]
        public string alertMessage = string.Empty;

        [Tooltip("Duration to show alert, or 0 to use default duration.")]
        public float alertDuration = 0;

        [Tooltip("If set, only increment if the conditions are true.")]
        public Condition condition = new Condition();

        public UnityEvent onIncrement = new UnityEvent();

        private bool listenForOnDestroy = false;

        protected string ActualVariableName
        {
            get { return string.IsNullOrEmpty(variable) ? OverrideActorName.GetInternalName(transform) : variable; }
        }

        /// <summary>
        /// Only listen for OnDestroy if the script has been enabled.
        /// </summary>
        public void OnEnable()
        {
            listenForOnDestroy = true;
            PersistentDataManager.RegisterPersistentData(gameObject);
        }

        /// <summary>
        /// If the level is being unloaded, this GameObject will be destroyed.
        /// We don't want to count this in the variable, so disable the script.
        /// </summary>
        public void OnLevelWillBeUnloaded()
        {
            listenForOnDestroy = false;
        }

        /// <summary>
        /// If the application is ending, don't listen, as this can log errors
        /// in the console.
        /// </summary>
        public void OnApplicationQuit()
        {
            listenForOnDestroy = false;
        }

        /// <summary>
        /// When this object is destroyed, increment the counter and update the quest tracker
        /// if incrementOn is set to Destroy.
        /// </summary>
        public void OnDestroy()
        {
            if (incrementOn == IncrementOn.Destroy) TryIncrement();
        }

        /// <summary>
        /// When this object is disabled, increment the counter and update the quest tracker
        /// if incrementOn is set to Disable.
        /// </summary>
        public void OnDisable()
        {
            PersistentDataManager.UnregisterPersistentData(gameObject);
            if (incrementOn == IncrementOn.Disable) TryIncrement();
        }

        public void TryIncrement()
        {
            if (!Application.isPlaying) return;
            if (DialogueManager.Instance == null || DialogueManager.DatabaseManager == null || DialogueManager.MasterDatabase == null) return;
            if (!(listenForOnDestroy && condition.IsTrue(null))) return;
            int oldValue = DialogueLua.GetVariable(ActualVariableName).AsInt;
            int newValue = Mathf.Clamp(oldValue + increment, min, max);
            DialogueLua.SetVariable(ActualVariableName, newValue);
            DialogueManager.SendUpdateTracker();
            if (!(string.IsNullOrEmpty(alertMessage) || DialogueManager.Instance == null))
            {
                if (Mathf.Approximately(0, alertDuration))
                {
                    DialogueManager.ShowAlert(alertMessage);
                }
                else
                {
                    DialogueManager.ShowAlert(alertMessage, alertDuration);
                }
            }
            onIncrement.Invoke();
        }

    }

}
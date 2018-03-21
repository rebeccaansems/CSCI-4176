using UnityEngine;
using UnityEngine.Events;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This component activates game objects and enables components when it receives 
    /// OnTriggerEnter and the conditions are true, and deactivates/disables when it
    /// receives OnTriggerExit and the conditions are true.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/range_trigger.html")]
#endif
    [AddComponentMenu("Dialogue System/Actor/Range Trigger")]
    public class RangeTrigger : MonoBehaviour
    {

        /// <summary>
        /// The condition that must be true in order to activate/deactivate target
        /// game objects and components when the trigger is entered or exited.
        /// </summary>
        [Tooltip("These conditions must be true for the Range Trigger to affect GameObjects and components and invoke events")]
        public Condition condition;

        /// <summary>
        /// The game objects to affect.
        /// </summary>
        [Tooltip("Activate these GameObjects on trigger enter, deactivate them on trigger exit")]
        public GameObject[] gameObjects;

        /// <summary>
        /// The components to affect.
        /// </summary>
        [Tooltip("Enable these components on trigger enter, disable them on trigger exit")]
        public Component[] components;

        public UnityEvent onEnter = new UnityEvent();

        public UnityEvent onExit = new UnityEvent();

        /// <summary>
        /// Activates the target game objects and components if the condition is true.
        /// </summary>
        /// <param name='other'>
        /// The collider that entered the trigger.
        /// </param>
        public void OnTriggerEnter(Collider other)
        {
            if (condition.IsTrue(other.transform)) SetTargets(true);
        }

        /// <summary>
        /// Deactivates the target game objects and components if the condition is true.
        /// </summary>
        /// <param name='other'>
        /// The collider that exited the trigger.
        /// </param>
        public void OnTriggerExit(Collider other)
        {
            if (condition.IsTrue(other.transform)) SetTargets(false);
        }

        /// <summary>
        /// Activates the target game objects and components if the condition is true.
        /// </summary>
        /// <param name='other'>
        /// The 2D collider that entered the trigger.
        /// </param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (condition.IsTrue(other.transform)) SetTargets(true);
        }

        /// <summary>
        /// Deactivates the target game objects and components if the condition is true.
        /// </summary>
        /// <param name='other'>
        /// The 2D collider that exited the trigger.
        /// </param>
        public void OnTriggerExit2D(Collider2D other)
        {
            if (condition.IsTrue(other.transform)) SetTargets(false);
        }

        /// <summary>
        /// Sets the targets active/inactive. Colliders and Renderers aren't MonoBehaviours, so we
        /// cast them separately to access their 'enabled' properties.
        /// </summary>
        /// <param name='value'>
        /// <c>true</c> for active, <c>false</c> for inactive.
        /// </param>
        private void SetTargets(bool value)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(value);
            }
            foreach (var component in components)
            {
                if (component is Collider)
                {
                    (component as Collider).enabled = value;
                }
                else if (component is Renderer)
                {
                    (component as Renderer).enabled = value;
                }
                else if (component is Animation)
                {
                    (component as Animation).enabled = value;
                }
                else if (component is Animator)
                {
                    (component as Animator).enabled = value;
                }
                else if (component is UnityEngine.Behaviour)
                {
                    (component as UnityEngine.Behaviour).enabled = value;
                }
                else
                {
                    if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Internal error - Range Trigger doesn't know how to handle {1} of type {2}", new object[] { DialogueDebug.Prefix, component, component.GetType().Name }));
                }
            }
            if (value == true)
            {
                onEnter.Invoke();
            }
            else
            {
                onExit.Invoke();
            }
        }

    }

}

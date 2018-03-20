using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This component indicates that the game object is usable. This component works in
    /// conjunction with the Selector component. If you leave overrideName blank but there
    /// is an OverrideActorName component on the same object, this component will use
    /// the name specified in OverrideActorName.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/usable.html")]
#endif
    [AddComponentMenu("Dialogue System/Actor/Usable")]
    public class Usable : MonoBehaviour
    {

        /// <summary>
        /// (Optional) Overrides the name shown by the Selector.
        /// </summary>
        public string overrideName;

        /// <summary>
        /// (Optional) Overrides the use message shown by the Selector.
        /// </summary>
        public string overrideUseMessage;

        /// <summary>
        /// The max distance at which the object can be used.
        /// </summary>
        public float maxUseDistance = 5f;

        /// <summary>
        /// Gets the name of the override, including parsing if it contains a [lua]
        /// or [var] tag.
        /// </summary>
        /// <returns>The override name.</returns>
        public string GetName()
        {
            if (string.IsNullOrEmpty(overrideName))
            {
                return name;
            }
            else if (overrideName.Contains("[lua") || overrideName.Contains("[var"))
            {
                return FormattedText.Parse(overrideName, DialogueManager.MasterDatabase.emphasisSettings).text;
            }
            else
            {
                return overrideName;
            }
        }

        public virtual void Start()
        {
            if (string.IsNullOrEmpty(overrideName))
            {
                OverrideActorName overrideActorName = GetComponentInChildren<OverrideActorName>();
                if (overrideActorName != null) overrideName = overrideActorName.overrideName;
            }
        }

    }

}

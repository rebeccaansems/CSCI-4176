using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem
{

    public static class UITools
    {

        /// <summary>
        /// Ensures that the scene has an EventSystem.
        /// </summary>
        public static void RequireEventSystem()
        {
            var eventSystem = GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
            if (eventSystem == null)
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": The scene is missing an EventSystem. Adding one.");
                new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem),
                               typeof(UnityEngine.EventSystems.StandaloneInputModule)
#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                               , typeof(UnityEngine.EventSystems.TouchInputModule)
#endif
                               );
            }
        }

        public static int GetAnimatorNameHash(AnimatorStateInfo animatorStateInfo)
        {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            return animatorStateInfo.nameHash;
#else
			return animatorStateInfo.fullPathHash;
#endif
        }

        /// <summary>
        /// Dialogue databases use Texture2D for actor portraits. Unity UI uses sprites.
        /// UnityUIDialogueUI converts textures to sprites. This dictionary contains
        /// converted sprites so we don't need to reconvert them every single time we
        /// want to show an actor's portrait.
        /// </summary>
        public static Dictionary<Texture2D, Sprite> spriteCache = new Dictionary<Texture2D, Sprite>();

        public static void ClearSpriteCache()
        {
            spriteCache.Clear();
        }

        public static Sprite CreateSprite(Texture2D texture)
        {
            if (texture == null) return null;
            if (spriteCache.ContainsKey(texture)) return spriteCache[texture];
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            spriteCache.Add(texture, sprite);
            return sprite;
        }

        public static string GetUIFormattedText(FormattedText formattedText)
        {
            if (formattedText == null)
            {
                return string.Empty;
            }
            else if (formattedText.italic)
            {
                return "<i>" + formattedText.text + "</i>";
            }
            else
            {
                return formattedText.text;
            }
        }

        private static UnityUIDialogueUI dialogueUI = null;

        /// <summary>
        /// Sends "OnTextChange(text)" to the dialogue UI GameObject.
        /// </summary>
        /// <param name="text"></param>
        public static void SendTextChangeMessage(UnityEngine.UI.Text text)
        {
            if (text == null) return;
            if (dialogueUI == null) dialogueUI = text.GetComponentInParent<UnityUIDialogueUI>();
            if (dialogueUI == null) return;
            dialogueUI.SendMessage("OnTextChange", text, SendMessageOptions.DontRequireReceiver);
        }

        public static void Select(UnityEngine.UI.Selectable selectable, bool allowStealFocus = true)
        {
            var currentEventSystem = UnityEngine.EventSystems.EventSystem.current;
            if (currentEventSystem == null || selectable == null) return;
            if (currentEventSystem.currentSelectedGameObject == null || allowStealFocus)
            {
                currentEventSystem.SetSelectedGameObject(selectable.gameObject);
                selectable.Select();
                selectable.OnSelect(null);
            }
        }

    }

}

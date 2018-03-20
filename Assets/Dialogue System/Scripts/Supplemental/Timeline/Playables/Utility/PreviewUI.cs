#if UNITY_2017_1_OR_NEWER && !(UNITY_2017_3 && UNITY_WSA)
using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This MonoBehaviour is used internally by the Dialogue System's
    /// playables to show an editor representation of activity that can
    /// only be accurately viewed at runtime.
    /// </summary>
    [AddComponentMenu("")] // No menu item. Only used internally.
    public class PreviewUI : MonoBehaviour
    {

        public static void ShowMessage(string message, float duration, int lineOffset)
        {
            var canvas = new GameObject("Editor Preview UI", typeof(Canvas), typeof(PreviewUI)).GetComponent<Canvas>();
            canvas.gameObject.tag = "EditorOnly";
            canvas.gameObject.hideFlags = HideFlags.DontSave;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;
            var text = new GameObject("Preview Text", typeof(UnityEngine.UI.Text), typeof(UnityEngine.UI.Outline)).GetComponent<UnityEngine.UI.Text>();
            text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, text.rectTransform.localPosition.y + 20 * lineOffset, text.rectTransform.localPosition.z);
            text.alignment = TextAnchor.MiddleCenter;
            text.transform.SetParent(canvas.transform, false);
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            canvas.GetComponent<PreviewUI>().ShowMessageOnInstance(message, duration);
        }

        public void ShowMessageOnInstance(string message, float duration)
        {
            StartCoroutine(ShowMessageOnInstanceCoroutine(message, duration));
        }

        public IEnumerator ShowMessageOnInstanceCoroutine(string message, float duration)
        { 
            var text = GetComponentInChildren<UnityEngine.UI.Text>();
            if (text == null) yield break;
            text.text = message;
            var displayDuration = Mathf.Approximately(0, duration) ? 2 : duration;
            var endTime = Time.realtimeSinceStartup + displayDuration;
            while (Time.realtimeSinceStartup < endTime)
            {
                yield return null;
            }
            if (Application.isEditor && !Application.isPlaying)
            {
                DestroyImmediate(gameObject);
#if UNITY_EDITOR
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}
#endif
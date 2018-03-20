// Based on: http://wiki.unity3d.com/index.php/Click_To_Move_C
// By: Vinicius Rezendrix
using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// Navigates to the place where the player mouse clicks.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/navigate_on_mouse_click.html")]
#endif
    [AddComponentMenu("Dialogue System/Actor/Player/Navigate On Mouse Click")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavigateOnMouseClick : MonoBehaviour
    {

        public AnimationClip idle;
        public AnimationClip run;
        public float stoppingDistance = 0.5f;

        public enum MouseButtonType { Left, Right, Middle };
        public MouseButtonType mouseButton = MouseButtonType.Left;

        public bool ignoreClicksOnUI = true;

        private Transform myTransform;
        private NavMeshAgent navMeshAgent;
        private Animation anim;

        void Awake()
        {
            myTransform = transform;
            anim = GetComponent<Animation>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.LogWarning(string.Format("{0}: No NavMeshAgent found on {1}. Disabling {2}.", DialogueDebug.Prefix, name, this.GetType().Name));
                enabled = false;
            }
        }

        void Update()
        {
            if (navMeshAgent.remainingDistance < stoppingDistance)
            {
                if (idle != null && anim != null) anim.CrossFade(idle.name);
            }
            else
            {
                if (run != null && anim != null) anim.CrossFade(run.name);
            }

            if (ignoreClicksOnUI && UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Moves the Player if the Left Mouse Button was clicked:
            if (Input.GetMouseButtonDown((int)mouseButton) && GUIUtility.hotControl == 0)
            {
                Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist))
                {
                    navMeshAgent.SetDestination(ray.GetPoint(hitdist));
                }
            }

            // Moves the player if the mouse button is held down:
            else if (Input.GetMouseButton((int)mouseButton) && GUIUtility.hotControl == 0)
            {

                Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist))
                {
                    navMeshAgent.SetDestination(ray.GetPoint(hitdist));
                }
            }
        }
    }
}
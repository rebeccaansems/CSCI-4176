using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This is a very simple example script that destroys a GameObject if
    /// it receives the message "TakeDamage(float)" or "Damage(float)". You
    /// can also assign an optional "corpse" prefab to replace the GameObject.
    /// </summary>
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
    [HelpURL("http://pixelcrushers.com/dialogue_system/manual/html/die_on_take_damage.html")]
#endif
    [AddComponentMenu("Dialogue System/Actor/Die On TakeDamage")]
    public class DieOnTakeDamage : MonoBehaviour
    {

        public GameObject deadPrefab;

        void TakeDamage(float damage)
        {
            if (deadPrefab != null)
            {
                GameObject dead = Instantiate(deadPrefab, transform.position, transform.rotation) as GameObject;
                if (dead != null) dead.transform.parent = transform.parent;
            }
            Destroy(gameObject);
        }

        void Damage(float damage)
        {
            TakeDamage(damage);
        }

    }

}

using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {

    public GameObject panel;

    public void HidePanel()
    {
       panel.SetActive(false);
    }
}

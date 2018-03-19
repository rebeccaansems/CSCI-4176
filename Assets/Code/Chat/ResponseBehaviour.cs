using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class ResponseBehaviour : MonoBehaviour {
    public GameObject responseButton;
    public RectTransform ParentPanel;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 5; i++)
         {
             GameObject respButton = (GameObject)Instantiate(responseButton);
             respButton.transform.SetParent(ParentPanel, false);
             respButton.transform.localScale = new Vector3(1, 1, 1);
             
             Button tempButton = respButton.GetComponent<Button>();
             int tempInt = i;

             tempButton.onClick.AddListener(() => ButtonClicked(tempInt));
         }
	}
	void ButtonClicked(int buttonNo)
         {
             Debug.Log ("Button clicked = " + buttonNo);
         }
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour {

	GameObject[] gameObjectArray1;
	GameObject[] gameObjectArray2;

	void Start() {
		gameObjectArray1 = GameObject.FindGameObjectsWithTag ("Buttons");
		gameObjectArray2 = GameObject.FindGameObjectsWithTag ("Extra_Buttons");
	}

	public void ChangeButton(bool buttonGroup) 
	{
		if (buttonGroup) {
			foreach (GameObject go in gameObjectArray1) {
				go.SetActive (false);
			}
			foreach (GameObject go in gameObjectArray2) {
				go.SetActive (true);
			}
		}
		else {
			foreach (GameObject go in gameObjectArray1) {
				go.SetActive (true);
			}
			foreach (GameObject go in gameObjectArray2) {
				go.SetActive (false);
			}
		}
	}
}
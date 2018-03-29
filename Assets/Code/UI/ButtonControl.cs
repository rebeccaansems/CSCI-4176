/**
 * Referenced from https://answers.unity.com/questions/894211/set-objects-child-to-activeinactive.html
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour {

	GameObject[] gameObjectArray1;
	GameObject[] gameObjectArray2;
	GameObject tmpObject;

	Transform[] allChildren;

	void Start() {
		gameObjectArray1 = GameObject.FindGameObjectsWithTag ("Buttons");
		tmpObject = GameObject.FindGameObjectWithTag ("TestButtons");
		allChildren = tmpObject.GetComponentsInChildren<Transform>(true);

	}

	public void ChangeButton(bool buttonGroup) 
	{
		if (buttonGroup) {
			foreach (GameObject go in gameObjectArray1) {
				go.SetActive (false);
			}
			foreach (Transform child in allChildren) {
				child.gameObject.SetActive (true);
			} 

		}

		else {
			foreach (GameObject go in gameObjectArray1) {
				go.SetActive (true);
			}
			foreach (Transform child in allChildren) {
				child.gameObject.SetActive (false);
			}
		}
	}
		
}
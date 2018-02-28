using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour {
	
	public void RemoveButton() 
	{
		GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag ("Buttons");

		foreach(GameObject go in gameObjectArray)
		{
			go.SetActive (false);
		}
	

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodBehaviour : MonoBehaviour {
	[Range(0, 4)]
	public int currentMood;
	// Initalizing mood Objects
	public GameObject moodOne;
	public GameObject moodTwo;
	public GameObject moodThree;
	public GameObject moodFour;
	public GameObject moodFive;
	public GameObject[] moods;

	// Opacities of selected and not selected mood sprites
	public float notSelected, selected;

	void Start()
	{
		moods = new GameObject[] {moodOne, moodTwo, moodThree, moodFour, moodFive};
		notSelected = 0.2f;
		selected = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		// https://forum.unity.com/threads/touching-a-2d-sprite.233483/ code used for clicking on certain sprites *bless*
		if (Input.GetMouseButtonDown (0)) {
			// Check where the mouse (or touch) occurred 
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
			// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
			if (hitInfo) {
				// Get the gameObject that was tapped
				GameObject selectedMood = hitInfo.transform.gameObject;
				// Change the current mood and opacities of other moods
				selectMood (selectedMood);
			}
		}
	}

	void selectMood(GameObject mood) {
		// Change opacity based on whether that mood is selected or not

		// https://stackoverflow.com/questions/32415545/how-can-i-decrease-opacity-in-unity
		// Code for changing opacity ^^
		for (int i = 0; i < moods.Length; i++) {
			if (moods [i] != mood) {
				var col = moods [i].GetComponent<SpriteRenderer> ().material.color;
				col.a = notSelected;
				moods [i].GetComponent<SpriteRenderer> ().material.color = col;
			} else {
				var col = moods [i].GetComponent<SpriteRenderer> ().material.color;
				col.a = selected;
				moods [i].GetComponent<SpriteRenderer> ().material.color = col;
				currentMood = i + 1; // +1 to account for array starting at 0

				//TODO: Log mood at this time to keep a record of moods
			}
		}
	}
}

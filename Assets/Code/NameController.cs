using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameController : MonoBehaviour {
	public Text text;
	public InputField textInput;
	public Button editButton;
	public Button submitButton;
	public CanvasGroup inputCanvas;
	public string jellyName;
	public bool allowEnter;

	// Use this for initialization
	void Start () {
		Button editBtn = editButton.GetComponent<Button>();
		editBtn.onClick.AddListener(onEditClick);

		Button submitBtn = submitButton.GetComponent<Button>();
		submitBtn.onClick.AddListener(onInputSubmit);

		int firstLoad = PlayerPrefs.GetInt("firstLoad", 1);
		if (firstLoad == 1) {
			// First time playing
			// Hide text input
			showInput();
			PlayerPrefs.SetInt ("firstLoad", 0);
		} else {
			HideInput();
			jellyName = PlayerPrefs.GetString("jellyName", "Jelly Pal");
		}

	}
	
	// Update is called once per frame
	void Update () {
		text.text = jellyName;

		if (allowEnter && (textInput.text.Length > 0) && (Input.GetKey (KeyCode.Return) || Input.GetKey (KeyCode.KeypadEnter))) {
			onInputSubmit ();
			allowEnter = false;
		} else
			allowEnter = textInput.isFocused;
	}

	void HideInput() {
		inputCanvas.alpha = 0f;
		inputCanvas.blocksRaycasts = false;
	}

	void showInput() {
		inputCanvas.alpha = 1f;
		inputCanvas.blocksRaycasts = true;
	}

	void onEditClick() {
		showInput ();
	}

	void onInputSubmit() {
		HideInput ();
		string name = textInput.text;
		PlayerPrefs.SetString ("jellyName", name);
		jellyName = name;
	}
	
}

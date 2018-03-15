using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour {

    public Button remove;
    public Text description;
    public Text date;
    public Text time;

	// Use this for initialization
	void Start () {

    }

    /*
     * Set the text items in a single listItem
     */
    public void Setup(string item, string itemDate, string itemTime)
    {
        description.text = item;
        date.text = itemDate;
        time.text = itemTime;

    }

}

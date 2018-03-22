using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour {

    public Button remove;
    public Text description;
    public Text date;
    public Text time;

    /*
     * Set the text items in a single listItem
     */
    public void Setup(string item, string itemDate, string itemTime){

        description.text = item;
        date.text = itemDate;
        time.text = itemTime;
    }

    // Remove any listeners upon destorying a ListItem object
    public void OnDestroy()
    {
        remove.onClick.RemoveAllListeners();
    }
}

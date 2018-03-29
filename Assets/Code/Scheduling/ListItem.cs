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
    public void OnDestroy(){
        remove.onClick.RemoveAllListeners();
    }
    
    /*
     *  Function to get the description text
     */
    public string GetDescription(){
        return this.description.text;
    }

    /*
     *  Function to get the date text
     */
    public string GetDate(){
        return this.date.text;
    }

    /*
     *  Function to get the time text
     */
    public string GetTime(){
        return this.time.text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scheduler : MonoBehaviour {
    // Dropdown menus for date and time components
    public Dropdown month, day, year, hour, minutes, amPm;
    // Inputfield for users description of the item to be scheduled
    public InputField description;
    // Reference to the List Item Button Prefab
    public GameObject newItem;
    // Reference to the scroll wview where List Item Button Prefabs are added
    public GameObject scrollView;
    // References to the list of items in each drop down menu
    public List<Dropdown.OptionData> monthOptions, dayOptions, yearOptions, hourOptions, minuteOptions, amPmOptions;
    // List of scheduled items
    public static List<ListItem> savedItems = new List<ListItem>();
    

    // Initialize 
    public void Start () {
        // Obtain list of available values for each drop down menu
        monthOptions = month.GetComponent<Dropdown>().options;
        dayOptions = day.GetComponent<Dropdown>().options;
        yearOptions = year.GetComponent<Dropdown>().options;
        hourOptions = hour.GetComponent<Dropdown>().options;
        minuteOptions = minutes.GetComponent<Dropdown>().options;
        amPmOptions = amPm.GetComponent<Dropdown>().options;
        description.characterLimit = 30;
        
        // Create a list to keep track of all listitems being added to the scene
        List<ListItem> newItems = new List<ListItem>();
        // Loop over all saved list items
        for (int i = 0; i < savedItems.Count; i++){
            ListItem saved = savedItems[i];
            // Create a new GameObject
            GameObject itemToAdd = Instantiate(newItem) as GameObject;
            ListItem parts = itemToAdd.GetComponent<ListItem>();
            // Set the scheduled item's description, date and time based on the saved items values
            parts.Setup(saved.GetDescription(), saved.GetDate(), saved.GetTime());
            // Add item to the scrollview list
            itemToAdd.transform.SetParent(scrollView.transform, false);
            itemToAdd.SetActive(true);
            // Add the list item to the new list
            newItems.Add(parts);
       }
       // set the saved items list to the newly generated items list
       savedItems = newItems;
    }

    /*
     * Function to add a List Item Button Object to a the scrollview with user selected values from dropdowns
     */
    public void addItem(){
        // The three items being added to the list item button object
        string item = null, date = null, time = null;
        // Variables to hold selected values from dropdowns
        string monthText, dayText, yearText, hourText, minutesText, amPmText;
        // Get the description of the item to be scheduled
        item = description.text;
        // Obtain the selected month, day and year
        monthText = monthOptions[month.GetComponent<Dropdown>().value].text;
        dayText = dayOptions[day.GetComponent<Dropdown>().value].text;
        yearText = yearOptions[year.GetComponent<Dropdown>().value].text;
        // Add the month, day and year to the date of the item to be scheduled if the default dropdown menu values are not selected
        if(!(monthText == "Select Month")){
            date += monthText + " ";
        }
        if(!(dayText == "Select Day")){
            date += dayText + ", ";
        }
        if(!(yearText == "Select Year")){
            date += yearText;
        }
        // Obtain the selected hour, minutes and AM/PM values
        hourText = hourOptions[hour.GetComponent<Dropdown>().value].text;
        minutesText = minuteOptions[minutes.GetComponent<Dropdown>().value].text;
        amPmText = amPmOptions[amPm.GetComponent<Dropdown>().value].text;
        // Add the hour, minutes, AM/PM to the date of the item to be scheduled if the default dropdown menu values are not selected
        if (!(hourText == "Select Hour")){
            time += hourText + ":";
        }
        if(!(minutesText == "Select Minutes")){
            time += minutesText + " ";
        }
        if(!(amPmText == "Select AM/PM")){
            time += amPmText;
        }
        // Create a new List Item Button Object
        GameObject itemToAdd = Instantiate(newItem) as GameObject;
        ListItem parts = itemToAdd.GetComponent<ListItem>();
        // Set the scheduled item's description, date and time
        parts.Setup(item, date, time);
        // Add item to the scrollview list
        itemToAdd.transform.SetParent(scrollView.transform, false);
        itemToAdd.SetActive(true);
        // Add the List item to the list of scheduled items
        savedItems.Add(parts);
        
    }
}

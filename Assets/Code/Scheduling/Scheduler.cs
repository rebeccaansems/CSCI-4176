using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scheduler : MonoBehaviour {

    public Dropdown month, day, year, hour, minutes, amPm;
    public InputField description;
    public GameObject newItem;
    public GameObject scrollView;
    List<Dropdown.OptionData> monthOptions, dayOptions, yearOptions, hourOptions, minuteOptions, amPmOptions;
    

    // Use this for initialization
    void Start () {
        
        monthOptions = month.GetComponent<Dropdown>().options;
        dayOptions = day.GetComponent<Dropdown>().options;
        yearOptions = year.GetComponent<Dropdown>().options;
        hourOptions = hour.GetComponent<Dropdown>().options;
        minuteOptions = minutes.GetComponent<Dropdown>().options;
        amPmOptions = amPm.GetComponent<Dropdown>().options;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addItem()
    {
        string item = null, date = null, time = null;
        string monthText, dayText, yearText, hourText, minutesText, amPmText;
        item = description.text;
        monthText = monthOptions[month.GetComponent<Dropdown>().value].text;
        dayText = dayOptions[day.GetComponent<Dropdown>().value].text;
        yearText = yearOptions[year.GetComponent<Dropdown>().value].text;
        if(!(monthText == "Select Month"))
        {
            date += monthText + " ";
        }
        if(!(dayText == "Select Day"))
        {
            date += dayText + ",";
        }
        if(!(yearText == "Select Year"))
        {
            date += yearText;
        }
        hourText = hourOptions[hour.GetComponent<Dropdown>().value].text;
        minutesText = minuteOptions[minutes.GetComponent<Dropdown>().value].text;
        amPmText = amPmOptions[amPm.GetComponent<Dropdown>().value].text;
        if(!(hourText == "Select Hour"))
        {
            time += hourText + ":";
        }
        if(!(minutesText == "Select Minutes"))
        {
            time += minutesText + " ";
        }
        if(!(amPmText == "Select AM/PM"))
        {
            time += amPmText;
        }

        GameObject itemToAdd = Instantiate(newItem) as GameObject;
        ListItem parts = itemToAdd.GetComponent<ListItem>();
        parts.Setup(item, date, time);
        itemToAdd.transform.SetParent(scrollView.transform, false);
        itemToAdd.SetActive(true);

    }

    
}

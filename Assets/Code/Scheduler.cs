using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scheduler : MonoBehaviour {

    public Dropdown month, day, year, hour, minutes, amPm;
    public InputField description;
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
        string item, date, time;

        item = description.text;
        date = monthOptions[month.GetComponent<Dropdown>().value].text + " " + dayOptions[day.GetComponent<Dropdown>().value].text + "," + yearOptions[year.GetComponent<Dropdown>().value].text;
        time = hourOptions[hour.GetComponent<Dropdown>().value].text + ":" + minuteOptions[minutes.GetComponent<Dropdown>().value].text + " " + amPmOptions[amPm.GetComponent<Dropdown>().value].text;
        description.text = item + " " + date + " " + time;
        //description.text = month.GetComponent()

        
    }
}

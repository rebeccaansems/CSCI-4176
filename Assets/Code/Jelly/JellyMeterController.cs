using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JellyMeterController : MonoBehaviour
{
    [Header("Jelly Meter Attributes")]

    [Tooltip("Value must be positive")]
    public float LossFoodPerMinute;
    [Tooltip("Value must be positive")]
    public float LossInterPerMinute;
    [Range(0, 100)]
    public float CurrentFood, CurrentInteraction;

    [Header("UI Elements")]
    public Slider MeterFood;
    public Slider MeterInteraction;

    public void Start()
    {
        //Load data from phone's save file
        CurrentFood = PlayerPrefs.GetFloat("CurrentFood", 100);
        CurrentInteraction = PlayerPrefs.GetFloat("CurrentInteraction", 100);

        UpdateMetersGraphically();

        //Food and interaction meters are updated every THREE seconds while on game main screen
        StartCoroutine(UpdateMetersNumerically());
    }




    IEnumerator UpdateMetersNumerically()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);

            CurrentFood -= (LossFoodPerMinute / 18);
            CurrentInteraction -= (LossInterPerMinute / 18);

            //Save current values
            PlayerPrefs.SetFloat("CurrentFood", CurrentFood);
            PlayerPrefs.SetFloat("CurrentInteraction", CurrentInteraction);

            UpdateMetersGraphically();
        }
    }

    private void UpdateMetersGraphically()
    {
        MeterFood.value = CurrentFood;
        MeterInteraction.value = CurrentInteraction;
    }

}

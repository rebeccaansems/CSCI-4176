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
        //Food and interaction meters are updated every THREE seconds while on game main screen
        StartCoroutine(UpdateMeters());
    }




    IEnumerator UpdateMeters()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);

            CurrentFood -= (LossFoodPerMinute / 18);
            CurrentInteraction -= (LossInterPerMinute / 18);

            MeterFood.value = CurrentFood;
            MeterInteraction.value = CurrentInteraction;
        }
    }

}

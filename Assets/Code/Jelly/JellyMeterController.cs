using Assets.SimpleAndroidNotifications;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JellyMeterController : MonoBehaviour
{
    [Header("Jelly Meter Attributes")]

    [Range(0, 5)]
    public float LossFoodPerMinute;
    [Range(0, 5)]
    public float LossInterPerMinute;
    [Range(0, 5)]
    public float LossFoodPerMinutePaused;
    [Range(0, 5)]
    public float LossInterPerMinutePaused;
    [Range(0, 100)]
    public float CurrentFood, CurrentInteraction;
    [Range(0, 100)]
    public int[] IdleLevelsFood;

    [Header("UI Elements")]
    public Slider MeterFood;
    public Slider MeterInteraction;

    private DateTime lastTimeGameWasOpened;

    public void Start()
    {
        //Load data from phone's save file
        CurrentFood = PlayerPrefs.GetFloat("CurrentFood", 100);
        CurrentInteraction = PlayerPrefs.GetFloat("CurrentInteraction", 100);
        
        //Get last time the player closed the app in order to determine what meters should be adjusted too
        if (PlayerPrefs.GetString("LastTimeGameWasOpened", string.Empty) != string.Empty)
        {
            lastTimeGameWasOpened = DateTime.Parse(PlayerPrefs.GetString("LastTimeGameWasOpened", string.Empty));
            UpdateMetersNumericallyPaused();
        }
        
        UpdateMetersGraphically();
        StartCoroutine(UpdateMetersNumericallyPlaying());
    }

    //Food and interaction meters are updated every THREE seconds while on game main screen
    private IEnumerator UpdateMetersNumericallyPlaying()
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

    //Update CurrentFood and CurrentInteraction based on how long game was paused
    private void UpdateMetersNumericallyPaused()
    {
        double minutesNotPlayed = DateTime.UtcNow.Subtract(lastTimeGameWasOpened).TotalMinutes;

        CurrentFood -= (float)(LossFoodPerMinutePaused * minutesNotPlayed);
        CurrentInteraction -= (float)(LossInterPerMinutePaused * minutesNotPlayed);

        UpdateMetersGraphically();
    }

    //Update the display of meters showing food/interaction levels
    private void UpdateMetersGraphically()
    {
        EnsureValuesAreValid();
        UpdateIdleAnimationBasedOnFood();

        MeterFood.value = CurrentFood;
        MeterInteraction.value = CurrentInteraction;
    }

    //Calculate how long until meters are empty and schedule notification to go then
    private void SendNotifications()
    {
        double minutesUntilEmptyFood = (-CurrentFood) / LossFoodPerMinutePaused;
        double minutesUntilEmptyInteraction = (-CurrentInteraction) / LossInterPerMinutePaused;

        NotificationManager.SendWithAppIcon(TimeSpan.FromMinutes(minutesUntilEmptyFood), "Jelly Friend Needs Food",
            string.Format("{0} is getting awfully hungry, don't forget to feed them!", PlayerPrefs.GetString("jellyName", "Jelly Pal")),
            Color.yellow);

        NotificationManager.SendWithAppIcon(TimeSpan.FromMinutes(minutesUntilEmptyInteraction), "Jelly Friend Needs Interaction",
            string.Format("{0} is getting awfully lonely, don't forget to talk to them!", PlayerPrefs.GetString("jellyName", "Jelly Pal")),
            Color.yellow);
    }

#if !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            OnApplicationQuit();
        }
    }
#endif

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastTimeGameWasOpened", DateTime.UtcNow.ToString());
    }

    //Prevent food/interaction from going below 0
    private void EnsureValuesAreValid()
    {
        if (CurrentFood < 0)
        {
            CurrentFood = 0;
        }

        if (CurrentInteraction < 0)
        {
            CurrentInteraction = 0;
        }
    }

    //Change idle animation based on food levels
    private void UpdateIdleAnimationBasedOnFood()
    {
        for (int i = 0; i < IdleLevelsFood.Length; i++)
        {
            if (CurrentFood >= IdleLevelsFood[i])
            {
                this.GetComponent<Animator>().SetInteger("IdleLevel", i);
                i = IdleLevelsFood.Length;
            }
        }
    }

}

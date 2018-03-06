using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodBehaviour : MonoBehaviour
{

    public JellySpeechController jellySpeech;

    public void LogMood(int currentMood)
    {
        //allow speech to continue because mood has been set
        jellySpeech.ForceDisplayUpdate(currentMood);
    }

}

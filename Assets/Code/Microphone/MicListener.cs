using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MicListener : MonoBehaviour
{
    //Make this mic listener a static singleton instance
    public static MicListener instance;
    //List of audio sources, we will want the default at index 0
    public List<AudioSource> audioSources;
    //When debug is true shows values read from mic
    public bool debug = false;
    //Bubble that blow when mic is active
    public ParticleSystem bubbles;
    //When the user blows in the mic we are going to make Jelly Pal float
    public GameObject jellyPal;

    //Standard amount of sample per microphone reading
    private int samplesToTake = 1024;
    //Using the default audio source
    private int audioSource = 0;
    //Float the Jelly no lower than its original position
    private double baseLevel;
    //Only show animation for 5 seconds after mic input detected
    private float bubbleTime = 0.0f;

    void Start()
    {
        //Set the base level of the Jelly
        baseLevel = jellyPal.transform.position.y;

        //Getting the local audio source from the game object
        if (audioSources.Count == 0)
        {
            if (this.GetComponent<AudioSource>() != null)
            {
                audioSources.Add(this.GetComponent<AudioSource>());
            }
            else
            {
                Debug.LogError("Error! no audio sources attached to MicListener.css");
            }
        }

        //Initialize all the audio sources
        foreach (AudioSource source in audioSources)
        {
            source.Play();
        }
    }

    void Update()
    {
        //Get normalized readings from the mic
        float[] samples = GetAudioSamples(audioSource);
        //Get their average volume
        float average = AverageVolume(samples);
        bubbleTime -= Time.deltaTime;

        if (average > 0.003F)
        {
            if (bubbleTime <= 0.0f)
            {
                //Restart bubble animation timeout
                bubbleTime = 1.0f;
                //Show bubbles
                bubbles.Play(true);
            }
            if (isVisible(jellyPal))
            {
                //Float Jelly up
                jellyPal.transform.Translate(0F, 0.05F, 0F);
            }
        } else
        {
            if (bubbleTime <= 0.0f)
            {
                bubbles.Stop(true);
            }
            //Sink back down
            if (jellyPal.transform.position.y > baseLevel)
            {
                jellyPal.transform.Translate(0F, -0.01F, 0F);
            }
        }
    }

    //Get samples for the desired amount of time
    public float[] GetAudioSamples(int audioSourceIndex)
    {
        //Get the samples
        float[] samples = audioSources[audioSourceIndex].GetOutputData(samplesToTake, 0);
        return samples;
    }

    //Normalize values between 0 and 1 to account for bad readings
    float[] NormalizeArray(float[] input)
    {
        float[] output = new float[input.Length];
        float max = -Mathf.Infinity;
        //get the max value in the array
        for (int i = 0; i < input.Length; i++)
        {
            max = Mathf.Max(max, Mathf.Abs(input[i]));
        }

        //divide everything by the max value
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = input[i] / max;
        }

        return output;
    }

    //Get the average input volume from the normalized samples
    float AverageVolume(float[] input)
    {
        float total = 0;
        for (int i = 0; i < input.Length; i++)
        {
            total += input[i];
        }

        float ave = total / input.Length;
        return ave;
    }

    //Make sure the sprite is visible by checking its y pos
    bool isVisible(GameObject sprite)
    {
        int height = Screen.height;
        float spriteY = Camera.main.WorldToScreenPoint(jellyPal.transform.position).y;
        if (spriteY < height)
        {
            return true;
        }
        // not visible on screen
        return false;
    }
}


using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongPlayer : MonoBehaviour {

    // Stores the song to be played
    public AudioClip audioClip;
    public GameObject drumBeat;

    // Seconds per beat to track progress through song
    private float spb;
    private float bpm;
    private float startTime;
    private float trackTime;
    private int beatNum;
    // Tracks if we are on a new beat
    private int previousBeat = 0;
    
    void Start () {
        // Song being used currently is 85bpm
        bpm = 85.0f;
        // Assumes 60fps https://www.gamasutra.com/blogs/YuChao/20170316/293814/Music_Syncing_in_Rhythm_Games.php
        spb = 60f / bpm;

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClip;

        // Make sure the beat isn't shown to start

        // Start tracking time in the track
        startTime = (float)AudioSettings.dspTime;
        audio.Play();
        //TODO: Need to set sample rate? https://docs.unity3d.com/ScriptReference/AudioSource.Play.html
    }
    
    void Update () {
        //TODO: Show a moving beat display to let the user know when to tap
        //TODO: Track the time tapped on the beat and track score
        
        trackTime = (float)(AudioSettings.dspTime - startTime);

        // Find which beat we are on
        beatNum = (int) Math.Floor(trackTime / spb);
        
        // For debug display on beat, some beats does not show, probably due to update too fast
        if (beatNum > previousBeat)
        {
            previousBeat = beatNum;
            drumBeat.SetActive(true);
        } else
        {
            // Hide beat indicator if previously shown
            drumBeat.SetActive(false);
        }
    }
}

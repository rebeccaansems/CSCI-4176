using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongPlayer : MonoBehaviour {

    // Stores the song to be played
    public AudioClip audioClip;
    // public GameObject drumBeat;
    public Beat beatNote;
    public static float beatTime;

    // Seconds per beat to track progress through song
    private float spb;
    private float bpm;
    private float startTime;
    private float trackTime;
    // Tracks if we are on a new beat
    private int previousBeat = 0;
    // When was the last beat created?
    // TODO: Replace with list of times to spawn beats
    private int previousBeatSpawned = 0;
    private int beatNum;
    
    void Start () {
        // Song being used currently is 85bpm
        bpm = 85.0f;
        // seconds per beat https://www.gamasutra.com/blogs/YuChao/20170316/293814/Music_Syncing_in_Rhythm_Games.php
        spb = 60f / bpm;

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClip;

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
        beatTime = trackTime / spb;
        beatNum = (int) Math.Floor(trackTime / spb);

        // For debug display on beat, some beats does not show, probably due to update too fast
        if (beatNum > previousBeat)
        {
            //Debug.Log(beatNum);
            previousBeat = beatNum;

            // Currently clicking on every second beat
            if (beatNum > previousBeatSpawned)
            {
                beatNote.songBeatPosition = beatNum + 1;
                beatNote.beatInitAdvance = 1;
                Instantiate(beatNote);
                previousBeatSpawned = beatNum + 1;
            }
        }
    }
}

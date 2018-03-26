using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SongPlayer : MonoBehaviour
{
    // Track how many were missed for failingS
    public static int missed = 0;

    // Stores the song to be played
    public AudioClip audioClip;
    // public GameObject drumBeat;
    public Beat beatNote;
    public static float beatTime;
    public GameObject welcomePanel;
    public GameObject gameOverPanel;
    public GameObject hitLine;
    public GameObject warning;
    public Text endScoreDisplay;
    public Text score;

    // constants for comparison
    private int WARN_THRESHOLD = 2;
    private int FAIL_THRESHOLD = 4;

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
    // How many beats should we spawn in advance
    private int spawnBeatInAdvance = 1;
    private int beatNum;
    // determines if the game is running or in menus
    private AudioSource audio;
    private Boolean playing;

    // Exit a menu and start the song
    public void StartPlay()
    {
        welcomePanel.SetActive(false);
        hitLine.SetActive(true);
        playing = true;
        audio = GetComponent<AudioSource>();
        audio.clip = audioClip;
        // Start tracking time in the track
        startTime = (float)AudioSettings.dspTime;
        audio.Play();
    }

    void Start()
    {
        // reset static value
        missed = 0;
        // Song being used currently is 85bpm
        bpm = 85.0f;
        // seconds per beat https://www.gamasutra.com/blogs/YuChao/20170316/293814/Music_Syncing_in_Rhythm_Games.php
        spb = 60f / bpm;
        // game starts in a menu
        playing = false;
        welcomePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        hitLine.SetActive(false);
        warning.SetActive(false);
    }

    void Update()
    {
        if (playing)
        {
            CheckWarningDisplay();

            // check if the user has failed
            if (GameFailed())
            {
                GameOver();
            }

            trackTime = (float)(AudioSettings.dspTime - startTime);
            // Debug.Log(trackTime);
            // Find which beat we are on
            beatTime = trackTime / spb;

            if (IsTrackRunning(trackTime + spawnBeatInAdvance, audioClip.length))
            {
                DisplayBeats();
            } else if (!IsTrackRunning(trackTime, audioClip.length))
            {
                GameOver();
            }
        }
    }

    // Displays warning animation above given miss range
    private void CheckWarningDisplay()
    {
        if (missed > WARN_THRESHOLD)
        {
            warning.SetActive(true);
        } else
        {
            warning.SetActive(false);
        }
    }

    // Checks if miss count has gone above the permitted range
    private Boolean GameFailed()
    {
        if (missed > FAIL_THRESHOLD)
        {
            return true;
        }
        return false;
    }

    // Convert timestamp floats to decimal for comparison, returns true is track is longer than current time
    private Boolean IsTrackRunning(float runTime, float endTime)
    {
        if ((decimal) runTime <= (decimal) endTime)
        {
            return true;
        }
        return false;
    }

    private void DisplayBeats()
    {
        beatNum = (int)Math.Floor(trackTime / spb);

        // For debug display on beat, some beats does not show, probably due to update too fast
        if (beatNum > previousBeat)
        {
            previousBeat = beatNum;

            // Currently clicking on every beat
            // if (beatNum > previousBeatSpawned)
            {
                beatNote.songBeatPosition = beatNum + spawnBeatInAdvance;
                beatNote.beatInitAdvance = spawnBeatInAdvance;
                Instantiate(beatNote);
                previousBeatSpawned = beatNum + spawnBeatInAdvance;
            }
        }
    }

    // Show the game over panel displaying options
    private void GameOver()
    {
        playing = false;
        endScoreDisplay.text = "Good job!\n You scored " + score.text + " points!";
        gameOverPanel.SetActive(true);
        // stop any components that interfere with panel
        audio.Stop();
        hitLine.SetActive(false);
        warning.SetActive(false);
    }
}

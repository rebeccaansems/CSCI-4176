using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour {
    
    // Which beat in the song does this correspond to
    public int songBeatPosition;
    // The beat this was initialized on
    public int beatInitAdvance;

    // The position of the beat target
    private float TARGET_Y_POS = 0;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private float step;
    
    public void ClickedBeat()
    {
        // TODO: Find the position of the beat to score points
        Debug.Log(transform.position);
        // Remove the beat
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        transform.SetParent(GameObject.Find("Canvas").transform, false);
        startPoint = transform.position;
        endPoint = GameObject.Find("Beat Target").transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        // Remove the beat if it has been missed
        if (SongPlayer.beatTime > songBeatPosition)
        {
            Destroy(gameObject);
        }
        
        // The step function linearly approaches an asymptote at 1, from https://www.gamasutra.com/blogs/YuChao/20170316/293814/Music_Syncing_in_Rhythm_Games.php
        step = (beatInitAdvance - (songBeatPosition - SongPlayer.beatTime)) / beatInitAdvance;
        
		transform.position = Vector2.Lerp(
            startPoint,
            endPoint,
            step
        );
    }
}

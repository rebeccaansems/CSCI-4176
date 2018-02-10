using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellySpeechController : MonoBehaviour {

    //Different static phrases the jelly can say
    private readonly string GREETING = "Hi! I am your jelly pal!";

    //touch controls
    private Touch touch;
    private bool touchEnabled;

    //The jelly meters display
    public GameObject metersPanel;
    public GameObject speechPanel;

    //UI text
    public Text speechText;
    //Flag to check if we are running the typing coroutine
    private bool isTyping;

    void Start () {
        //Hide the meters until the speech is done
        metersPanel.SetActive(false);
        isTyping = false;
        touchEnabled = true;
        StartCoroutine(AnimateText(GREETING));
    }

    void Update()
    {
        //Navigate the text using touch controls or left mouse click
        if (Input.touchCount > 0 && touchEnabled || Input.GetMouseButtonDown(0))
        {
            updateDisplay();
        }
    }

    void updateDisplay()
    {
        if (isTyping)
        {
            //This causes the animation to stop
            isTyping = false;
        }
        else
        {
            //Display the next text or hide the speech box
            //TODO: for now just hiding speech box
            metersPanel.SetActive(true);
            speechPanel.SetActive(false);
        }
    }

    //Animate text from: https://answers.unity.com/questions/219281/making-text-boxes-with-letters-appearing-one-at-a.html
    IEnumerator AnimateText(string fullStr)
    {
        isTyping = true;
        int i = 0;
        string partialStr = "";
        while (i < fullStr.Length)
        {
            if (isTyping)
            {
                //Append one character at a time to make it appeach as though the jelly is speaking
                partialStr += fullStr[i++];
                speechText.text = partialStr;
                yield return new WaitForSeconds(0.1F);
            } else
            {
                //Cancel the typing and output
                speechText.text = fullStr;
                i = fullStr.Length;
            }
            
        }
        isTyping = false;
    }

}

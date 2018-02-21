using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellySpeechController : MonoBehaviour
{

    //Different static phrases the jelly can say
    private readonly string GREETING = "Hi! I am your jelly pal!";
    private readonly string CHECK_IN = "How are you feeling today?";
    private readonly string MOOD_RESPONSE_NEUTRAL = "I will add that to your mood calendar.";

    //touch controls
    private Touch touch;
    private bool touchEnabled;

    //The jelly meters display
    public GameObject metersPanel;
    //Jelly speech bubble
    public GameObject speechPanel;
    //UI navigation
    public GameObject uiNavigation;
    //Smiley for mood input
    public GameObject moodInput;
    //Jelly name display
    public GameObject jellyName;
    //UI text
    public Text speechText, nameText;

    //Flag to check if we are running the typing coroutine
    private bool isTyping;
    //Store the next text for the jelly to say in this list
    private List<string> dialogueTree;

    void Start()
    {
        dialogueTree = new List<string>();
        //Add some phrases to the dialog tree
        dialogueTree.Add(GREETING);
        dialogueTree.Add(CHECK_IN);
        dialogueTree.Add(MOOD_RESPONSE_NEUTRAL);
        //Hide the meters until the speech is done
        metersPanel.SetActive(false);
        uiNavigation.SetActive(false);
        moodInput.SetActive(false);
        jellyName.SetActive(false);

        isTyping = false;
        touchEnabled = true;

        //Start dialogue from beginning of list
        StartCoroutine(AnimateText(dialogueTree[0]));

        //change jelly name to be what we named the jelly
        nameText.text = PlayerPrefs.GetString("jellyName", "Jelly Pal") + ": ";
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
        nameText.text = PlayerPrefs.GetString("jellyName", "Jelly Pal")+": ";
        if (isTyping)
        {
            //This causes the animation to stop
            isTyping = false;
        }
        else
        {
            //Remove the text that was just used
            dialogueTree.RemoveAt(0);
            if (dialogueTree.Count > 0)
            {
                string nextPrompt = dialogueTree[0];

                //Check for special prompt conditions
                if (nextPrompt.Equals(CHECK_IN))
                {
                    //Show the smileys
                    moodInput.SetActive(true);
                }
                else
                {
                    //Make anything that could have been turned on is off now
                    moodInput.SetActive(false);
                    // TODO: get the selected mood
                }

                StartCoroutine(AnimateText(nextPrompt));

            }
            else
            {
                //Display the next text or hide the speech box
                metersPanel.SetActive(true);
                speechPanel.SetActive(false);
                uiNavigation.SetActive(true);
                jellyName.SetActive(true);
            }
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
            }
            else
            {
                //Cancel the typing and output
                speechText.text = fullStr;
                i = fullStr.Length;
            }

        }
        isTyping = false;
    }

}

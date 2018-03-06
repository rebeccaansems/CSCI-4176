using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JellySpeechController : MonoBehaviour
{
    //Store the next text for the jelly to say in this list
    public List<DialogueMap.Dialogue> DialogueList;

    //touch controls
    private Touch touch;
    private bool touchEnabled;

    //The text portion we're currently at in the text tree
    public int CurrentDialogueIndex;

    //The jelly meters display
    public GameObject metersPanel;
    //Jelly speech bubble
    public GameObject speechPanel;
    //UI navigation
    public GameObject uiNavigation;
    //Smiley for mood input
    public GameObject moodPanel;
    //Jelly name display
    public GameObject jellyName;
    //UI text
    public Text speechText, nameText;

    //Flag to check if we are running the typing coroutine
    private bool isTyping;

    private void Start()
    {
        //Hide the meters until the speech is done
        metersPanel.SetActive(false);
        uiNavigation.SetActive(false);
        moodPanel.SetActive(false);
        jellyName.SetActive(false);

        isTyping = false;
        touchEnabled = true;

        CurrentDialogueIndex = 0;

        //Start dialogue from beginning of list
        StartCoroutine(AnimateText(DialogueList[CurrentDialogueIndex].GetText()));

        //change jelly name to be what we named the jelly
        nameText.text = PlayerPrefs.GetString("jellyName", "Jelly Pal") + ": ";
    }

    private void Update()
    {
        //Navigate the text using touch controls or left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        nameText.text = PlayerPrefs.GetString("jellyName", "Jelly Pal") + ": ";
        if (isTyping)
        {
            //This causes the animation to stop
            isTyping = false;
        }
        else if (DialogueList.Count > CurrentDialogueIndex + 1 && DialogueList[CurrentDialogueIndex + 1].WaitForInput == false)
        {
            CurrentDialogueIndex++;
            if (DialogueList.Count > CurrentDialogueIndex)
            {
                //Check for special prompt conditions
                if (DialogueList[CurrentDialogueIndex].Name == "CHECK IN")
                {
                    //Show the smileys
                    moodPanel.SetActive(true);
                }

                StartCoroutine(AnimateText(DialogueList[CurrentDialogueIndex].GetText()));
            }
        }
        else if (DialogueList.Count <= CurrentDialogueIndex + 1)
        {
            ShowMainDisplay();
        }
    }

    public void ForceDisplayUpdate()
    {
        ForceDisplayUpdate(0);
    }

    public void ForceDisplayUpdate(int response)
    {
        CurrentDialogueIndex++;

        if (DialogueList.Count > CurrentDialogueIndex)
        {
            StartCoroutine(AnimateText(DialogueList[CurrentDialogueIndex].GetText(response)));
        }
        else
        {
            ShowMainDisplay();
        }
    }

    private void ShowMainDisplay()
    {
        //Display the next text or hide the speech box
        metersPanel.SetActive(true);
        speechPanel.SetActive(false);
        moodPanel.SetActive(false);
        uiNavigation.SetActive(true);
        jellyName.SetActive(true);
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

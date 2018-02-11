using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//Streams microphone data through an AudioSource component, based on implementation from (which was purchased):
//https://assetstore.unity.com/packages/tools/audio/audio-visualizer-47866
[RequireComponent(typeof(AudioSource))]
public class MicInput : MonoBehaviour
{

        //This is a silent mixer so that we won't be playing audio back to the user 
        public AudioMixerGroup mixer;
        //Setting the device to zero gets the default mic
        public int deviceNum = 0;

        string currentAudioInput = "none";
        float delay = 0.030f;
        const float freq = 24000f;
        //List of available devices
        string[] inputDevices;
        AudioSource aSource;

        void Start()
        {
            aSource = GetComponent<AudioSource>();
            //Get the list of microphones on this device
            inputDevices = new string[Microphone.devices.Length];
            //Populate the list with all available microphones
            for (int i = 0; i < Microphone.devices.Length; i++)
            {
                inputDevices[i] = Microphone.devices[i].ToString();
                Debug.Log("Device: " + i + ": " + inputDevices[i]);
            }
            //Start streaming the mic through an audio clip
            aSource.clip = Microphone.Start(Microphone.devices[deviceNum].ToString(), true, 5, (int)freq);

            //Hookup the game AudioMixerGroup to the AudioSource
            aSource.outputAudioMixerGroup = mixer;

            //Start playing back the audio streamed in from the mic
            aSource.Play();
        }

        private void Update()
        {
            //Make sure the mic is still active
            if (aSource.isPlaying)
                return;

            //Restart the mic if stopped, assure it has been checked in delay window
            int microphoneSamples = Microphone.GetPosition(currentAudioInput);
            if (microphoneSamples / freq > delay)
            {
                //Play the audio source
                aSource.timeSamples = (int)(microphoneSamples - (delay * freq));
                aSource.Play();
            }
        }
}

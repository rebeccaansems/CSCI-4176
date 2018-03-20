using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands {

    /// <summary>
    /// This script implements the sequencer command SetTimeout(duration),
    /// which sets the response menu timeout duration. Set duration 
    /// to 0 (zero) to disable the timer.
    /// </summary>
    [AddComponentMenu("")] // Hide from menu.
    public class SequencerCommandSetTimeout : SequencerCommand {
		
		public void Start() {
			float duration = GetParameterAsFloat(0);
			if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: SetTimeout({1})", DialogueDebug.Prefix, duration));
			if (DialogueManager.DisplaySettings != null && DialogueManager.DisplaySettings.inputSettings != null) {
				DialogueManager.DisplaySettings.inputSettings.responseTimeout = duration;
			}
			Stop();
		}
	}
}

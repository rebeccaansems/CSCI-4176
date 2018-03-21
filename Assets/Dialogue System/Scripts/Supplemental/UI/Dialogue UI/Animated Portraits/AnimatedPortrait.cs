using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// Specifies the animated portrait to use for this actor.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Dialogue/Animated Portrait")]
	public class AnimatedPortrait : MonoBehaviour {

		[Tooltip("Animator controller that runs this actor's animated portrait. It should animate an Image component, not a SpriteRenderer.")]
		public RuntimeAnimatorController animatorController;
	}

}
using System.Collections;
using UnityEngine;

namespace core
{
	  public class Fader : MonoBehaviour
	  {
			#region Variables set in Inspector

			[SerializeField] Canvas faderCanvas = null;

			[SerializeField] float alphaJumpStep = 0.017f; // deltaAlpha over 1 frame at 60fps
			[SerializeField] float fadeTime = 3f;

			#endregion

			#region Variables set by Script

			float deltaTransparency;

			const string faderCanvasName = "Fader Canvas";

			CanvasGroup faderControl;

			#endregion

			#region Internal Methods

			internal void FadeIn(float a = -1f) { StartCoroutine(FadeToTransparent(a)); }
			internal void FadeOut(float a = -1f) { StartCoroutine(FadeToBlack(a)); }
			internal void SetSortLevel() { faderCanvas.sortingOrder = FindObjectsOfType<Canvas>().Length + 1; }
			internal void ToggleRaycast() { faderControl.blocksRaycasts = !faderControl.blocksRaycasts; }

			#endregion

			#region Private Methods

			private IEnumerator FadeToBlack(float a)
			{
				  bool processComplete = false;

				  while (processComplete == false)
				  {
						if (a >= alphaJumpStep) deltaTransparency = Time.deltaTime / a;
						else deltaTransparency = Time.deltaTime / fadeTime;

						faderControl.alpha += deltaTransparency;

						if (faderControl.alpha > 0.99f) processComplete = true;

						yield return null;
				  }
				  
				  faderControl.alpha = 1f;
				  yield break;
			}
			
			private IEnumerator FadeToTransparent(float a)
			{
				  bool processComplete = false;

				  while (processComplete == false)
				  {
						if (a >= alphaJumpStep) deltaTransparency = Time.deltaTime / a;
						else deltaTransparency = Time.deltaTime / fadeTime;

						faderControl.alpha -= deltaTransparency;
						if (faderControl.alpha < 0.02f) processComplete = true;

						yield return null;
				  }

				  faderControl.alpha = 0f;
				  yield break;
			}

			#endregion

			#region Utility Methods

			internal void Awake()
			{
				  if(!faderCanvas) foreach(var canvas in FindObjectsOfType<Canvas>()) if (canvas.name == faderCanvasName) faderCanvas = canvas;
				  faderControl = faderCanvas.GetComponent<CanvasGroup>();
			}

			#endregion
	  }
}
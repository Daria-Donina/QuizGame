using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Class displaying and controlling text label.
/// </summary>
public class TextViewer : MonoBehaviour
{
    [SerializeField] 
	private Text textComponent;

	[Range(0, 2)]
	[SerializeField] 
	private float fadeDuration;

	/// <summary>
	/// Shows and updates text.
	/// </summary>
	/// <param name="text">Text to show.</param>
    public void UpdateText(string text)
	{
		textComponent.text = $"Find {text}";

		//Animate Fade-In
		textComponent.DOFade(1, fadeDuration);
	}

	/// <summary>
	/// Hides text.
	/// </summary>
	public void Hide()
	{
		textComponent.DOFade(0, 0);
	}
}

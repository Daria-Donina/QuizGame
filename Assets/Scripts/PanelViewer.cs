using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// Class displaying and controlling finishing panel.
/// </summary>
public class PanelViewer : MonoBehaviour
{
	[SerializeField]
	private GameObject panel;

	[SerializeField]
	[Range(0, 255)]
	private float opacityValue = 100;

	[SerializeField]
	[Range(0, 3)]
	private float fadeDuration = 1;

	private Image _image;

	private void Awake()
	{
		_image = panel.GetComponent<Image>();
	}

	/// <summary>
	/// Shows panel.
	/// </summary>
    public void Show()
	{
		panel.SetActive(true);
		_image.DOFade(opacityValue / 255, fadeDuration);
	}

	/// <summary>
	/// Hides panel.
	/// </summary>
	public void Hide()
	{
		_image.DOFade(0, 0);
		panel.SetActive(false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Scripts;

/// <summary>
/// Class displaying and controlling grid.
/// </summary>
public class GridViewer : MonoBehaviour
{
	[SerializeField]
	private ViewModel viewModel;

    [SerializeField]
    private GameObject cellPrefab;

	[SerializeField]
	[Range(1, 10)]
	private float spacing;

	[SerializeField]
	[Range(0, 10)]
	private float offsetY;

	[SerializeField]
	[Range(0, 2)]
	private float cellAnimationSpeed;

	[SerializeField]
	private Vector3 cellScale = Vector3.one * 3;

	/// <summary>
	/// Displays given row of sprites.
	/// </summary>
	/// <param name="row">A row of sprites.</param>
	public void DisplayRow(Sprite[] row)
	{
		for (int i = 0; i < row.Length; ++i)
		{
			Vector3 position = 
				Vector3.right * spacing * (i % 3 - 1) + Vector3.up * (spacing * (- i / 3 + 1) - offsetY);

			// Instantiating cell and given sprite as game objects.
			var cell = Instantiate(cellPrefab, position, Quaternion.identity);
			var child = CreateGameObjectForSprite(row[i]);

			// creating a cell from two objects.
			child.transform.position = cell.transform.position;
			child.transform.SetParent(cell.transform);

			// making the cell a child of the grid.
			cell.transform.SetParent(transform);

			// Subscribing view model on button events.
			var childSprite = child.GetComponent<SpriteRenderer>().sprite;
			cell.GetComponent<Button>().onClick.AddListener(delegate () { viewModel.OnButtonClick(childSprite); });

			// Cell bounce when it's created.
			AnimateCellBounce(cell.transform);
		}
	}

	/// <summary>
	/// Animates cell when it's clicked.
	/// </summary>
	/// <param name="index">Index of the cell.</param>
	/// <param name="isAnswerCorrect">Indicates whether answer is right or wrong.</param>
	public void OnCellClickedAnimate(int index, bool isAnswerCorrect)
	{
		var cell = transform.GetChild(index);

		if (isAnswerCorrect)
		{
			AnimateRightAnswer(cell);
		}
		else
		{
			AnimateWrongAnswer(cell);
		}	
	}

	/// <summary>
	/// Clears cells after level.
	/// </summary>
	public void ClearAfterLevel()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	private Sequence AnimateCellBounce(Transform cell)
	{
		var sequence = DOTween.Sequence();

		sequence.Append(cell.DOScale(0, 0f));
		sequence.Append(cell.DOScale(cellScale + Vector3.one, cellAnimationSpeed));
		sequence.Append(cell.DOScale(cellScale, cellAnimationSpeed / 2));

		sequence.Play();

		return sequence;
	}

	private void AnimateRightAnswer(Transform cell)
	{
		var sequence = AnimateCellBounce(cell);

		// star effect
		var particles = cell.GetComponent<ParticleSystem>();
		particles.playbackSpeed = 8;
		particles.Play();

		// signaling that animation is completed and new level can be started.
		sequence.OnComplete(viewModel.OnChangeLevel);
	}

	private void AnimateWrongAnswer(Transform cell)
	{
		cell.DOPunchPosition(Vector3.right, cellAnimationSpeed);
	}

	private GameObject CreateGameObjectForSprite(Sprite sprite)
	{
		var gameObject = new GameObject();

		// Shows game object on top of the cell
		var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 1;

		spriteRenderer.sprite = sprite;

		return gameObject;
	}
}

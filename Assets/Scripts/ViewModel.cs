using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	/// <summary>
	/// Class that connects logic and ui.
	/// </summary>
	class ViewModel : MonoBehaviour
	{
		[SerializeField]
		private DataSet[] sets;

		[SerializeField]
		private GridViewer grid;

		[SerializeField]
		private TextViewer text;

		[SerializeField]
		private PanelViewer finishPanel;

		private UnityEvent<Sprite[]> LevelGenerated = new UnityEvent<Sprite[]>();
		private UnityEvent<string> GoalChosen = new UnityEvent<string>();
		private UnityEvent<int, bool> AnswerReady = new UnityEvent<int, bool>();

		private LevelModel _model;
		private List<Sprite> _sprites = new List<Sprite>();

		private void Awake()
		{
			_model = new LevelModel(sets.Length, this);

			GoalChosen.AddListener(text.UpdateText);
			LevelGenerated.AddListener(grid.DisplayRow);
			AnswerReady.AddListener(grid.OnCellClickedAnimate);

			OnGameStart();
		}

		private void OnDestroy()
		{
			LevelGenerated.RemoveAllListeners();
			GoalChosen.RemoveAllListeners();
			AnswerReady.RemoveAllListeners();
		}

		/// <summary>
		/// Called when game's started.
		/// </summary>
		public void OnGameStart()
		{
			finishPanel.Hide();
			_model.CreateLevel();
		}

		/// <summary>
		/// Get set count.
		/// </summary>
		/// <param name="index">Index of the set.</param>
		/// <returns>Set count.</returns>
		public int GetSetCount(int index)
		{
			return sets[index].Count;
		}

		/// <summary>
		/// Called when level model created.
		/// </summary>
		/// <param name="indexes">Chosen elements' indexes.</param>
		public void OnLevelModelCreated(int[] indexes) 
		{
			var set = sets[_model.SetIndex];

			SendToText(set);

			SendToGrid(set, indexes);
		}

		/// <summary>
		/// Called when button (cell) is clicked.
		/// </summary>
		/// <param name="sprite"></param>
		public void OnButtonClick(Sprite sprite)
		{
			// Finding index of clicked sprite.
			var clickedIndex = _sprites.IndexOf(sprite);

			// Sending answer to the viewer.
			AnswerReady.Invoke(clickedIndex, _model.IsAnswerRight(clickedIndex));
		}

		/// <summary>
		/// Called when level's changed.
		/// </summary>
		public void OnChangeLevel()
		{
			grid.ClearAfterLevel();

			_model.CreateLevel();
		}

		/// <summary>
		/// Called when the game's finished.
		/// </summary>
		public void OnGameFinished()
		{
			finishPanel.Show();

			text.Hide();
		}

		private void SendToText(DataSet set)
		{
			var name = set.GetName(_model.GoalIndex);

			GoalChosen.Invoke(name);
		}

		private void SendToGrid(DataSet set, int[] indexes)
		{
			_sprites.Clear();

			for (int i = 0; i < indexes.Length; ++i)
			{
				_sprites.Add(set.GetSprite(indexes[i]));
			}

			LevelGenerated.Invoke(_sprites.ToArray());
		}
	}
}

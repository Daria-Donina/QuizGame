using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
	/// <summary>
	/// Class that models levels and contains logic.
	/// </summary>
	class LevelModel
	{
		public int NumberInRow { get; private set; } = 3;
		public int RowCount 
		{
			get => _rowCount;
			private set
			{
				_rowCount = value;

				if (value == 4)
				{
					_rowCount = 0;
					GameFinished.Invoke();
				}
			}
		}
		public int GoalIndex { get; private set; }
		public int GoalPlace { get; private set; }
		public int SetIndex { get; private set; }

		private List<int> _usedGoals;
		private List<int> _usedIndexes;

		private System.Random _rnd;
		private ViewModel _viewModel;

		private int _setsLength;
		private int _rowCount;

		private UnityEvent<int[]> LevelModelCreated = new UnityEvent<int[]>();
		private UnityEvent GameFinished = new UnityEvent();

		public LevelModel(int setsLength, ViewModel viewModel)
		{
			_rnd = new System.Random();
			_usedIndexes = new List<int>();
			_usedGoals = new List<int>();

			_viewModel = viewModel;
			_setsLength = setsLength;

			LevelModelCreated.AddListener(viewModel.OnLevelModelCreated);
			GameFinished.AddListener(viewModel.OnGameFinished);
		}

		/// <summary>
		/// Create next level model.
		/// </summary>
		public void CreateLevel()
		{
			RowCount++;

			// if RowCount is zero that means game's finished.
			if (RowCount == 0)
			{
				return;
			}

			// Clearing indexes used on previous level.
			_usedIndexes.Clear();

			// Choosing random set.
			SetIndex = ChooseSet(_setsLength);
			var setLength = _viewModel.GetSetCount(SetIndex);

			// Choosing random goal place.
			GoalPlace = ChoosePlace();

			// Choosing random goal index.
			GoalIndex = ChooseGoalIndex(setLength);

			// Generating row of random indexes with goal index on a goal place.
			GenerateRow(setLength);

			// finished generating model
			LevelModelCreated.Invoke(_usedIndexes.ToArray());
		}

		/// <summary>
		/// Checks if the answer is rigth.
		/// </summary>
		/// <param name="place">Chosen place.</param>
		/// <returns></returns>
		public bool IsAnswerRight(int place)
		{
			return place == GoalPlace;
		}

		private int ChoosePlace()
		{
			return _rnd.Next(RowCount * NumberInRow);
		}

		private int ChooseIndex(int setLength)
		{
			var index = _rnd.Next(setLength);

			while (_usedIndexes.Contains(index) || index == GoalIndex)
			{
				index = _rnd.Next(setLength);
			}

			return index;
		}

		private int ChooseGoalIndex(int setLength)
		{
			var index = _rnd.Next(setLength);

			// When used all the goals clear list and start again.
			if (_usedGoals.Count == setLength)
			{
				_usedGoals.Clear();
			}

			while (_usedGoals.Contains(index))
			{
				index = _rnd.Next(setLength);
			}

			_usedGoals.Add(index);

			return index;
		}

		private int ChooseSet(int setsLength)
		{
			return _rnd.Next(setsLength);
		}

		private void GenerateRow(int setLength)
		{
			var n = NumberInRow * RowCount;

			for (int i = 0; i < n; ++i)
			{
				int index;

				if (i == GoalPlace)
				{
					index = GoalIndex;
				}
				else
				{
					index = ChooseIndex(setLength);
				}

				_usedIndexes.Add(index);
			}
		}
	}
}

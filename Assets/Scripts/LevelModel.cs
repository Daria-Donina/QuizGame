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
			get => rowCount;
			private set
			{
				rowCount = value;

				if (value == 4)
				{
					rowCount = 0;
					gameFinished.Invoke();
				}
			}
		}
		public int GoalIndex { get; private set; }
		public int GoalPlace { get; private set; }
		public int SetIndex { get; private set; }

		private List<int> usedGoals;

		private List<int> usedIndexes;

		private System.Random rnd;
		private int setsLength;
		private ViewModel viewModel;

		private UnityEvent<int[]> levelModelCreated = new UnityEvent<int[]>();
		private UnityEvent gameFinished = new UnityEvent();
		private int rowCount;

		public LevelModel(int setsLength, ViewModel viewModel)
		{
			rnd = new System.Random();
			usedIndexes = new List<int>();
			usedGoals = new List<int>();

			this.viewModel = viewModel;
			this.setsLength = setsLength;

			levelModelCreated.AddListener(viewModel.OnLevelModelCreated);
			gameFinished.AddListener(viewModel.OnGameFinished);
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
			usedIndexes.Clear();

			// Choosing random set.
			SetIndex = ChooseSet(setsLength);
			var setLength = viewModel.GetSetCount(SetIndex);

			// Choosing random goal place.
			GoalPlace = ChoosePlace();

			// Choosing random goal index.
			GoalIndex = ChooseGoalIndex(setLength);

			// Generating row of random indexes with goal index on a goal place.
			GenerateRow(setLength);

			// finished generating model
			levelModelCreated.Invoke(usedIndexes.ToArray());
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
			return rnd.Next(RowCount * NumberInRow);
		}

		private int ChooseIndex(int setLength)
		{
			var index = rnd.Next(setLength);

			while (usedIndexes.Contains(index) || index == GoalIndex)
			{
				index = rnd.Next(setLength);
			}

			return index;
		}

		private int ChooseGoalIndex(int setLength)
		{
			var index = rnd.Next(setLength);

			// When used all the goals clear list and start again.
			if (usedGoals.Count == setLength)
			{
				usedGoals.Clear();
			}

			while (usedGoals.Contains(index))
			{
				index = rnd.Next(setLength);
			}

			usedGoals.Add(index);

			return index;
		}

		private int ChooseSet(int setsLength)
		{
			return rnd.Next(setsLength);
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

				usedIndexes.Add(index);
			}
		}
	}
}

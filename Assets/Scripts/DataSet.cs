using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	/// Class containing data set information.
	/// </summary>
	[CreateAssetMenu(fileName = "DataSet", menuName = "Data Set")]
	class DataSet : ScriptableObject
	{
		[SerializeField] private Sprite[] sprites;
		[SerializeField] private string[] names;

		/// <summary>
		/// Number of elements.
		/// </summary>
		public int Count => sprites.Length;

		/// <summary>
		/// Get sprite by index.
		/// </summary>
		/// <param name="index">Index of set element.</param>
		/// <returns>Sprite with corresponding index.</returns>
		public Sprite GetSprite(int index)
		{
			return sprites[index];
		}

		/// <summary>
		/// Get name by index.
		/// </summary>
		/// <param name="index">Index of set element.</param>
		/// <returns>Name of element with corresponding index.</returns>
		public string GetName(int index)
		{
			return names[index];
		}
	}
}

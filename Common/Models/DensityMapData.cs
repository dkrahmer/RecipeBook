namespace Common.Models
{
	public class DensityMapData
	{
		/// <summary>
		/// Primary name of the item.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Alternate names separated by pipe (|).
		/// </summary>
		public string AlternateNames { get; set; }

		/// <summary>
		/// Density in g/ml
		/// </summary>
		public decimal Density { get; set; }
	}
}

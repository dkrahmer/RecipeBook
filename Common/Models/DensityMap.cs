using System.Collections.Generic;

namespace Common.Models
{
	public class DensityMap
	{
		public List<string> Names { get; set; }
		/// <summary>
		/// Density in g/ml
		/// </summary>
		public decimal Density { get; set; }
	}
}

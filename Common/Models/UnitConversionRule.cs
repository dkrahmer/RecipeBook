using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
	public class UnitConversionRule
	{
		public string InputUnit { get; set; }
		public List<string> Rules { get; set; }
		public string ConversionEquation { get; set; }
		public string OutputUnit { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
	public class Tag
	{
		public int TagId { get; set; }
		public string TagName { get; set; }
	}
}

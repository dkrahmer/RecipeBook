using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
	public class RecipeTag
	{
		public int RecipeId { get; set; }
		public int TagId { get; set; }
	}
}

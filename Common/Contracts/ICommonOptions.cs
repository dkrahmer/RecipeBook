using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Contracts
{
	public interface ICommonOptions
	{
		string MySqlConnectionString { get; set; }
	}
}
